using UnityEngine;
using System.Collections;

public class UI_Controller_CardSlot : MonoBehaviour 
{
	UI_Controller_CardArt _cardArt;

	void Awake()
	{
		_cardArt = transform.GetComponentInChildren<UI_Controller_CardArt>();
	}

	public UI_Controller_CardArt cardArt
	{
		get{return _cardArt;}
	}
}
