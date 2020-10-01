using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piedestal : MonoBehaviour
{
	public Animator animator;
	public string ButtonPressAnimation = "Button Press";

	public AnswerManager answerManager;
	public int ansNr;
	public ParticleSystem correctFX, incorrectFX;
	public Transform target;
	public Transform child;
	public Rigidbody childRB;


	// Update is called once per frame
	void Update()
	{
		if (child == null || !child.gameObject.activeSelf)
			return;
		if (childRB == null)
			childRB = child.GetComponent<Rigidbody>();
		//child.Rotate(new Vector3(10, 20, 10) * Time.deltaTime);
		if (child != Grabber.instance.grabbed)
		{
			child.position = Vector3.Slerp(child.position, target.position, 5 * Time.deltaTime);
			childRB.useGravity = false;
			childRB.velocity = Vector3.zero;
			childRB.angularVelocity = Vector3.zero;
		}
		else if (Grabber.instance.holdL || Grabber.instance.holdR)
		{

			childRB.useGravity = true;
		}
	}
	public void OnLeverPulled()
	{
		if (answerManager.Submit(ansNr))
		{
			correctFX.Play();
		}
		else
		{
			incorrectFX.Play();
		}
	}
	public void OnButtonPress()
	{
		animator.Play(ButtonPressAnimation);
		if (answerManager.Submit(ansNr))
		{
			correctFX.Play();
		}
		else
		{
			incorrectFX.Play();
		}
	}

	public LeverAction leveraction;
	public void OnReset()
	{
		leveraction.ResetLever();
	}



}
