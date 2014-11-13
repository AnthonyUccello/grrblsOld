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

	public GameObject grrbl;
	/*public Transform helmTransform;
	public Transform armorTransform;
	public Transform offHandTransform;
	public Transform mainHandTransform;*/
	Transform _slot;

	//pass it the id of the item
	public void equipItem(int itemTypeId)
	{
		JsonData[] items = TypeData.GET_TABLE("items").rows;
		JsonData item = items[itemTypeId];
		/*if((string)item["slot"] == "armor")
		{
			//_slot = grrbl.GetComponent<C_GrrblEquipmentSlot>().armorTransform;
		}else if((string)item["slot"] == "helmet")
		{
			//_slot = grrbl.GetComponent<C_GrrblEquipmentSlot>().helmTransform;
		}else if((string)item["slot"] == "offHand")
		{
			_slot = grrbl.GetComponent<C_GrrblEquipmentSlot>().offHandTransform;
		}else if((string)item["slot"] == "mainHand")
		{
			_slot = grrbl.GetComponent<C_GrrblEquipmentSlot>().mainHandTransform;
		}else
		{
			Debug.Log("Bad item type passed");
		}*/

		//Debug.Log("Making " +(string)item["prefab"]);
		/*Debug.Log((string)item["prefab"]);
		GameObject obj = Instantiate(Resources.Load("Prefabs/3D/" + (string)item["prefab"])) as GameObject;
		if(obj==null)
		{
			Debug.Log("Error prefab name " + item["name"] + " does not exist as prefab");
		}
		obj.transform.parent = _slot;
		obj.transform.localPosition = new Vector3(0,0,0);


		//update stats
		JsonData stats = JsonMapper.ToObject((string)item["stats"]);
		grrbl.GetComponent<Manager_Grrbl_Stats>().updateStats(stats);*/
	}
}
