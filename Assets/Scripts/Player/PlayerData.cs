using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
	public string Username;
	public int Score;
	public int LastServerID;

	public void Reset()
	{
		Username = "";
		Score = 0;
	}

	public PlayerData(string username, int score)
	{
		Username = username;
		Score = score;

		LastServerID = -1;
	}
}
