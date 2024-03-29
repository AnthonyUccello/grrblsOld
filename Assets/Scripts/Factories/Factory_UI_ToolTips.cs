﻿using UnityEngine;
using System.Collections;
using LitJson;

/*
 * When a card or grrbl is hovered over, this creates the tip. 
 * Cards call card tooltip create, and grrbls call grrbl tool tip create
 * */
public static class Factory_UI_ToolTips {

	//static Helper helper = GameObject.Find("Helper").GetComponent<Helper>();
	static GameObject _cardToolTip;
	//static GameObject _grrblToolTip;
	static GameObject _uiRoot = GameObject.Find("UI Root");
	static JsonData[] cards = TypeData.GET_TABLE("cards").rows;
	static JsonData[] items = TypeData.GET_TABLE("items").rows;

	public static void createCardToolTip(int cardId, GameObject card)
	{
		if(_cardToolTip!=null)//if there is already a tip when this loaded, destroy it and create a new one
		{
			GameObject.Destroy(_cardToolTip);
		}

		createBackground(card,cardId);
		createTooltipStats(cardId);
	}

	//creates the background image
	static void createBackground(GameObject card, int cardId)
	{
		_cardToolTip = NGUITools.AddChild(card,Resources.Load("Prefabs/UI/cardToolTip") as GameObject);
		Transform t = card.transform;
		//this positions the arrow to be just below the card
		_cardToolTip.transform.localPosition = new Vector3(t.localPosition.x + 36,t.localPosition.y-100,0);
		_cardToolTip.transform.parent = _uiRoot.transform;
	}

	//create the tool tip values and populate inside the displaying tool tip
	static void createTooltipStats(int cardId)
	{
		JsonData card =  cards[cardId];
		JsonData item = items[(int)card["item_id"]];
		//Debug.Log("Item Is " + (string)item["stats"]);
		JsonData stats = JsonMapper.ToObject((string)item["stats"]);
		int count = 0;
		if(JsonData.JsonDataContainsKey(stats,"damage"))
		{
			createToolTipValue("damage",stats["damage"].ToString(),count);
			count++;
		}
		if(JsonData.JsonDataContainsKey(stats,"health"))
		{
			createToolTipValue("health",stats["health"].ToString(),count);
			count++;
		}
		if(JsonData.JsonDataContainsKey(stats,"dodge"))
		{
			createToolTipValue("dodge",stats["dodge"].ToString(),count);
			count++;
		}
		if(JsonData.JsonDataContainsKey(stats,"attackSpeed"))
		{
			createToolTipValue("attackSpeed",stats["attackSpeed"].ToString(),count);
			count++;
		}
		if(JsonData.JsonDataContainsKey(stats,"critChance"))
		{
			createToolTipValue("critChance",stats["critChance"].ToString(),count);
			count++;
		}
		if(JsonData.JsonDataContainsKey(stats,"critMultiplier"))
		{
			createToolTipValue("critMultiplier",stats["critMultiplier"].ToString(),count);
			count++;
		}
		if(JsonData.JsonDataContainsKey(stats,"armor"))
		{
			createToolTipValue("armor",stats["armor"].ToString(),count);
			count++;
		}
	}

	//creates a single tooltip
	static void createToolTipValue(string spriteName,string value,int offsetCount)
	{
		GameObject cardToolTipValue = NGUITools.AddChild(_cardToolTip,Resources.Load("Prefabs/UI/cardToolTipValue") as GameObject);
		Transform t = _cardToolTip.transform;
		cardToolTipValue.transform.localPosition = new Vector3(0,-70-(30*offsetCount),0);

		//set values
		UI_Controller_CardToolTipValue cardValues = cardToolTipValue.GetComponent<UI_Controller_CardToolTipValue>();
		cardValues.icon.spriteName = spriteName;
		cardValues.value.text = value;
	}

	//remove the tool tip from view
	public static void hideToolTip()
	{
		GameObject.Destroy(_cardToolTip);
	}
}