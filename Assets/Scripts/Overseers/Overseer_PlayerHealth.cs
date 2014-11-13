using UnityEngine;
using System.Collections;

/*
 * Manages health for player and AI.
 * 
 * Also manages the display on the screen for HP
 * */
public static class Overseer_PlayerHealth {

	static UILabel _playerHealthDisplay;
	static UILabel _aiHealthDisplay;

	static int _playerHealth = 20;
	static int _aiHealth = 20;

	public static void init()
	{
		_aiHealthDisplay = GameObject.Find("aiHealth").GetComponent<UILabel>();
		_playerHealthDisplay = GameObject.Find("playerHealth").GetComponent<UILabel>();
		_playerHealthDisplay.text = "Player Health: " + _playerHealth;
		_aiHealthDisplay.text = "AI Health: " + _aiHealth;
	}

	public static void reduceAIHealth()
	{
		_aiHealth--;
		_aiHealthDisplay.text = "AI Health: " + _aiHealth;
	}

	public static void reducePlayerHealth()
	{
		_playerHealth--;
		_playerHealthDisplay.text = "Player Health: " + _playerHealth;
	}


}
