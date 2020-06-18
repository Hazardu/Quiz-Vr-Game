using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	bool opened = false;
	Animator anim;
	private void Start()
	{
		anim = GetComponent<Animator>();
	}
	public void Open()
	{
		if (opened)
			return;
		opened = true;
		anim.Play("Open");

	}

	public void Close()
	{
		if (!opened)
			return;
		opened = false;
		anim.Play("Close");
	}
}
