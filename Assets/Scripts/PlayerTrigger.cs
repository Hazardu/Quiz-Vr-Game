using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
	[SerializeField] private UnityEvent onEnter;
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.root.tag == "Player")
		{
			onEnter?.Invoke();
		}
	}
}
