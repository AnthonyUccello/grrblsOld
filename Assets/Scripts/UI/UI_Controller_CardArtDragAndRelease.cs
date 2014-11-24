using UnityEngine;
using System.Collections;
using LitJson;
/*
 * Handles drag event where it checks to see if the item is hovered over and allied Grrbl. If so, instantiate the prefab on it
 * 
 * */
public class UI_Controller_CardArtDragAndRelease : UIDragDropItem {

	public GameObject prefab;
	private UI_Controller_CardArt _card;

	// Use this for initialization
	protected override void Start () {
		_card = transform.GetComponent<UI_Controller_CardArt>();
		base.Start();
	}

	protected void OnDragStart()
	{
		//tell card to hide tool tip
		Factory_UI_ToolTips.hideToolTip();
		base.OnDragStart();
	}

	protected override void OnDragDropRelease (GameObject surface)
	{			
		Transform grrbl;
		//if can't afford mana
		/*if(_card.cost > Overseer_PlayerMana.playerMana)
		{
			Debug.Log("Cannot Afford");
		}else */
		if (surface != null && surface.tag=="grrblPlayer")
		{
			grrbl = surface.gameObject.transform;
			grrbl.GetComponent<Manager_Grrbl_Equipment>().equipItem(_card.itemId);
			Factory_UI_Deck.addNewCard(_card);
			//_card.deactivate();
			//turn everything off so it can be activated when a card is placed on it
			//Factory_UI_Deck.queueNewCard(_card);
		}

		gameObject.transform.parent = _card.border;
		gameObject.transform.localPosition = _card.startingLocation;
		base.OnDragDropRelease(surface);
	}
}
