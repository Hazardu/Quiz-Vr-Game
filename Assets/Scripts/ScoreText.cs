using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
	private void Start()
	{
		textMesh = GetComponent<TMPro.TMP_Text>();
		UpdateText();
	}
	private TMPro.TMP_Text textMesh;
	public void UpdateText()
	{
		string text = GameManager.EndlessMode? GameManager.Points .ToString("N0") : $"{GameManager.Points}/{GameManager.GoalPoints}";
		textMesh.text = text;
	}

}
