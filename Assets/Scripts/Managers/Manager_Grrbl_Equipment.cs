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

	//equipment slots
	bool _canEquipArmor;
	bool _canEquipMainHand;
	bool _canEquipOffHand;
	bool _canEquipTwoHand;
	bool _canEquipHelmet;

	//AI
	AI_Grrbl_Behaviour aiGrrblBehaviour;

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

		_canEquipArmor = true;
		_canEquipHelmet = true;
		_canEquipOffHand = true;
		_canEquipTwoHand = true;
		_canEquipMainHand = true;

		aiGrrblBehaviour = transform.GetComponent<AI_Grrbl_Behaviour>();
	}
	
	//pass it the id of the item
	public void equipItem(int itemTypeId)
	{
		JsonData[] items = TypeData.GET_TABLE("items").rows;
		JsonData item = items[itemTypeId];
		if(!canEquipItemSlot((string)item["slot"]))
		{
			Debug.Log("Item slot is filled");
			return;
		}
		string prefabName =(string)item["prefab"];
		//Debug.Log(prefabName);
		GameObject equippedItem = itemGameObjects[prefabName];
		equippedItem.SetActive(true);//enable the 3D model

		//update stats
		JsonData stats = JsonMapper.ToObject((string)item["stats"]);
		transform.GetComponent<Manager_Grrbl_Stats>().updateStats(stats);

		//flag slot as filled
		removeEquipmentSlotAvailablity((string)item["slot"]);
	}

	//Change the walk or stance if its ans armor
	private void updateAnimationBehaviour()
	{

	}

	//return true if the slot they want to equip to is open
	private bool canEquipItemSlot(string type)
	{
		if(type == "main_hand")
		{
			return _canEquipMainHand;
		}else if(type == "off_hand")
		{
			return _canEquipOffHand;
		}else if(type == "two_hand")
		{
			if(_canEquipOffHand == true && _canEquipMainHand == true)
			{
				return true;
			}
			return false;
		}else if(type == "helmet")
		{
			return _canEquipHelmet;
		}else if(type == "armor")
		{
			return _canEquipArmor;
		}

		Debug.Log("Bad item type pass for equip slot check");
		return false;
	}

	//Flags a Grrbl equip slot as filled e.g. canEquipArmor = false
	private void removeEquipmentSlotAvailablity(string type)
	{
		if(type == "main_hand")
		{
			_canEquipMainHand = false;
		}else if(type == "off_hand")
		{
			_canEquipOffHand = false;
		}else if(type == "two_hand")
		{
			_canEquipOffHand = false;
			_canEquipMainHand = false;
		}else if(type == "helmet")
		{
			_canEquipHelmet = false;
		}else if(type == "armor")
		{
			_canEquipArmor = false;
		}
	}
}
