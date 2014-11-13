using UnityEngine;
using System.Collections;

//manage mana for the AI and YOU
public static class Overseer_PlayerMana {

	static float _manaRechargeTime = 2f;
	static Utility_CoroutineSource _coroutineSource;
	static int _playerMana;
	static int _aiMana;

	static UILabel _playerManaDisplay;
	static UILabel _aiManaDisplay;

	static public void init()
	{
		_coroutineSource = GameObject.Find("Utility_CoroutineSource").GetComponent<Utility_CoroutineSource>();
		_coroutineSource.StartCoroutine(tick());
		_playerMana = 1;
		_aiMana = 1;
		_aiManaDisplay = GameObject.Find("aiMana").GetComponent<UILabel>();
		_playerManaDisplay = GameObject.Find("playerMana").GetComponent<UILabel>();
		_playerManaDisplay.text = "Player Mana: " + _playerMana;
		_aiManaDisplay.text = "AI Mana: " + _aiMana;
	}

	static public IEnumerator tick()
	{
		while(true)
		{
			yield return new WaitForSeconds(_manaRechargeTime);
			_aiMana++;
			if(_aiMana>10) _aiMana = 10;
			_playerMana++;
			if(_playerMana>10) _playerMana = 10;
			_playerManaDisplay.text = "Player Mana: " + _playerMana;
			_aiManaDisplay.text = "AI Mana: " + _aiMana;
		}
	}

	static public void spendPlayerMana(int i)
	{
		_playerMana -= i;
		_playerManaDisplay.text = "Player Mana: " + _playerMana;
	}

	static public void spendAIMana(int i)
	{
		_aiMana -= i;
		_aiManaDisplay.text = "AI Mana: " + _aiMana;
	}

	static public int playerMana
	{
		get{return _playerMana;}
	}

	static public int aiMana
	{
		get{return _aiMana;}
	}
	
}
