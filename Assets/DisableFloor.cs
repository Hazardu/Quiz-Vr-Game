using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFloor : MonoBehaviour
{
	int disabledCount = 0;
	public void DisableNext()
	{
		transform.GetChild(disabledCount++).gameObject.SetActive(false);
	}
}
