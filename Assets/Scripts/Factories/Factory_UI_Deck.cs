using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;

/**
 * Creates the cards and fills the deck
 * */
public static class Factory_UI_Deck {

	public static GameObject deck;
	static Dictionary<UI_Controller_CardArt,bool> playerCards = new Dictionary<UI_Controller_CardArt,bool>();
	static JsonData[] cards = TypeData.GET_TABLE("cards").rows;

	//Puts cards at the top of the screen
	public static void dealFirstHand()
	{
		deck = GameObject.FindGameObjectWithTag("deck");
		for(int i =0;i<10;i++)
		{
			GameObject card =  NGUITools.AddChild(deck,Resources.Load("Prefabs/UI/CardSlot") as GameObject);
			assignRandomCard(card.GetComponent<UI_Controller_CardSlot>().cardArt);
			card.transform.parent = deck.transform;
			card.transform.localScale = new Vector3(0.75f,0.75f,0.75f);
			//UIPanel display = card.transform.GetComponentInChildren<UIPanel>();
			//card.transform.position = new Vector3(display.width/2,display.height/2,0);
			playerCards.Add(card.GetComponent<UI_Controller_CardSlot>().cardArt,false);
			card.GetComponent<UI_Controller_CardSlot>().cardArt.activate();
		}
		
		deck.GetComponent<UIGrid>().Reposition();
	}

	static void assignRandomCard(UI_Controller_CardArt card)
	{
		//Debug.Log(itemSlot.transform.name);
		int rand = UnityEngine.Random.Range(0,cards.Length);
		//Debug.Log("added " + cards[rand]["name"]);
		card.cardId = (int)cards[rand]["id"];
		card.itemId = (int)cards[rand]["item_id"];
	}

	static public void queueNewCard(UI_Controller_CardArt card)
	{
		playerCards[card] = true;
		deck.GetComponent<MonoBehaviour>().StartCoroutine(beginNewCardTime(card));
	}

	static IEnumerator beginNewCardTime(UI_Controller_CardArt card)
	{
		//get used cards count
		int usedCards = 0;
		foreach(KeyValuePair<UI_Controller_CardArt,bool> kvp in playerCards)
		{
			if(kvp.Value==true)
			{
				usedCards++;
			}
		}
		yield return new WaitForSeconds(3f*usedCards);
		assignRandomCard(card);
		card.activate();
	}
}
