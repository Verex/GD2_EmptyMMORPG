using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
	[SerializeField] private Text scoreText;

	public void SetScoreText(int score)
	{
		scoreText.text = score.ToString();
	}
}
