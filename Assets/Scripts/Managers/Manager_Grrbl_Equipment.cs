using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;

/*
 * When a card is dropped on a grrbl this listens for what happend and manages it accordingly
 * 
 * Goes on Grrbl prefab
 * */
public class Manager_Grrbl_Equipment : MonoBehaviour {

	//main hands
	public GameObject knightSword;
	public GameObject samuraiKatana;

	//offHands
	public GameObject samuraiTanto;
	public GameObject knightShield;

	//armor
	public GameObject knightArmor;
	public GameObject samuraiArmor;

	//helmets
	public GameObject knightHelmet;
	public GameObject samuraiHelmet;

	//item map
	private Dictionary<string,GameObject> itemGameObjects;

	void Awake()
	{
		itemGameObjects = new Dictionary<string,GameObject>
		{
			{"knight_sword",knightSword},
			{"samurai_katana",samuraiKatana},
			{"samurai_tanto",samuraiTanto},
			{"knight_shield",knightShield},
			{"knight_armor",knightArmor},
			{"samurai_armor",samuraiArmor},
			{"knight_helmet",knightHelmet},
			{"samurai_helmet",samuraiHelmet}
		};
	}
	
	//pass it the id of the item
	public void equipItem(int itemTypeId)
	{
		JsonData[] items = TypeData.GET_TABLE("items").rows;
		JsonData item = items[itemTypeId];
		string prefabName =(string)item["prefab"];
		//Debug.Log(prefabName);
		GameObject equippedItem = itemGameObjects[prefabName];
		equippedItem.SetActive(true);

		//update stats
		JsonData stats = JsonMapper.ToObject((string)item["stats"]);
		transform.GetComponent<Manager_Grrbl_Stats>().updateStats(stats);
	}
}
