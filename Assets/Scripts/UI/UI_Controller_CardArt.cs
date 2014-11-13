using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using LitJson;

/*
 * Manages the icon when this is in a button. Also stores information for drop release
 * 
 * */
public class UI_Controller_CardArt : MonoBehaviour 
{
	public Transform border;//parent of the icon
	public Vector3 scale;
	private UISprite _uiSprite;
	int _cardId;
	int _itemId;
	int _cost;
	Vector3 _startingLocation;
	
	float _toolTipDelayTimer = 0.01f;//0.75
	bool _displayToolTip = false;
	bool _timerIsRunning = false;

	void Awake()
	{
		_uiSprite = transform.GetComponent<UISprite>();
	}

	void Start()
	{
		_startingLocation = transform.localPosition;
	}

	//call this after assigning a new item id
	void updateVisual()
	{
		JsonData[] cards = TypeData.GET_TABLE("cards").rows;
		string spriteName = (string)cards[cardId]["card_art"];
		if(_uiSprite.atlas.GetSprite(spriteName) == null)
		{
			Debug.Log("Sprite name " + spriteName + " is not found");
		}
		transform.GetComponent<UISprite>().spriteName = spriteName;
		transform.GetComponentInChildren<UILabel>().text = ((int)cards[cardId]["cost"]).ToString();
		_cost = (int)cards[cardId]["cost"];
	}

	public int cost
	{
		get{return _cost;}
	}

	public int cardId
	{
		set{_cardId=value;}
		get{return _cardId;}
	}

	public int itemId
	{
		set{_itemId=value;}
		get{return _itemId;}
	}

	//get the position that the item icons original position is;
	public Vector3 startingLocation
	{
		set{_startingLocation=value;}
		get{return _startingLocation;}
	}

	//tells it to turn off all its stuff
	public void deactivate()
	{
		GetComponent<UISprite>().enabled = false;
		GetComponent<BoxCollider>().enabled = false;
		transform.GetComponentInChildren<UILabel>().enabled = false;
	}

	//turn on all its stuff
	public void activate()
	{
		updateVisual();
		GetComponent<UISprite>().enabled = true;
		GetComponent<BoxCollider>().enabled = true;
		transform.GetComponentInChildren<UILabel>().enabled = true;
	}

	void OnHover( bool isOver )
	{
		//add a timer
		if(isOver)
		{
			StartCoroutine(delayTimer());
		}else
		{
			hideToolTip();
		}
	}
	
	IEnumerator delayTimer()
	{
		if(!_timerIsRunning)
		{
			_timerIsRunning = true;
			yield return new WaitForSeconds(_toolTipDelayTimer);
			displayToolTip();
			_timerIsRunning = false;
		}
	}
	
	void displayToolTip()
	{
		Factory_UI_ToolTips.createCardToolTip(_cardId, gameObject);
	}
	
	void hideToolTip()
	{
		Factory_UI_ToolTips.hideToolTip();
	}
}
