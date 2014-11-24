using UnityEngine;
using System.Collections;
using LitJson;

/*
 * Spawns the Grrbls at specific time intervals
 * 
 * Currently doing shitty AI, which needs to be re-done as an AI_OpponentPlayer
 * */
public static class Factory_3D_GrrblSpawner 
{

	private static Utility_CoroutineSource helper = GameObject.Find("Utility_CoroutineSource").GetComponent<Utility_CoroutineSource>();

	static Transform lane1AISpawnSpot = GameObject.Find("lane1AISpawnSpot").transform;
	static Transform lane2AISpawnSpot = GameObject.Find("lane2AISpawnSpot").transform;
	static Transform lane3AISpawnSpot = GameObject.Find("lane3AISpawnSpot").transform;
	
	static Transform lane1PlayerSpawnSpot = GameObject.Find("lane1PlayerSpawnSpot").transform;
	static Transform lane2PlayerSpawnSpot = GameObject.Find("lane2PlayerSpawnSpot").transform;
	static Transform lane3PlayerSpawnSpot = GameObject.Find("lane3PlayerSpawnSpot").transform;

	public static float spawnTimer = 15f;
	static string spawnPath = "Prefabs/3D/grrbl";

	static int[] _armor = new int[]{1,4,9,12};
	static int[] _mainHandWeapons = new int[]{0,6,11,14,15};
	static int[] _offHandWeapons = new int[]{3,7,10};
	static int[] _helmets = new int[]{2,5,8,13};

	public static void beginSpawn()
	{
		helper.StartCoroutine(spawnGrrbls());
	}

	static IEnumerator spawnGrrbls()
	{
		while(true)
		{
			int rand = Random.Range(1,4);
			spawnAIGrrbl(rand);
			yield return new WaitForSeconds(spawnTimer);
		}

	}

	public static void spawnPlayerGrrbl(int lane)
	{
		//spawn player grrbls
		GameObject grrbl = MonoBehaviour.Instantiate(Resources.Load(spawnPath)) as GameObject;
		Vector3 spawnLocation = new Vector3(0,0,0);
		Transform endLocation = null;
		switch(lane)
		{
			case 1: 
			{
				spawnLocation = lane1PlayerSpawnSpot.transform.position;
				endLocation = lane1AISpawnSpot.transform;
				break;
			}
			case 2: 
			{
				spawnLocation = lane2PlayerSpawnSpot.transform.position;
				endLocation = lane2AISpawnSpot.transform;
				break;
			}
			case 3: 
			{
				spawnLocation = lane3PlayerSpawnSpot.transform.position;
				endLocation = lane3AISpawnSpot.transform;
				break;
			}
			default: Debug.Log("bad lane");
				break;
		}
		grrbl.transform.position = spawnLocation;
		grrbl.transform.LookAt(endLocation);
		grrbl.GetComponent<AI_Grrbl_Behaviour>().endDestination = endLocation;
		grrbl.layer=9;//player1
		grrbl.GetComponent<AI_Grrbl_Behaviour>().init();
		grrbl.tag="grrblPlayer";
	}
	
	public static void spawnAIGrrbl(int lane)
	{
		GameObject grrbl = MonoBehaviour.Instantiate(Resources.Load(spawnPath)) as GameObject;
		Vector3 spawnLocation = new Vector3(0,0,0);
		Transform endLocation = null;
		switch(lane)
		{
			case 1: 
			{
				endLocation = lane1PlayerSpawnSpot.transform;
				spawnLocation = lane1AISpawnSpot.transform.position;
				break;
			}
			case 2: 
			{
				endLocation = lane2PlayerSpawnSpot.transform;
				spawnLocation = lane2AISpawnSpot.transform.position;
				break;
			}
			case 3: 
			{
				endLocation = lane3PlayerSpawnSpot.transform;
				spawnLocation = lane3AISpawnSpot.transform.position;
				break;
			}
			default: Debug.Log("bad lane");
				break;
		}
		grrbl.transform.position = spawnLocation;
		grrbl.transform.LookAt(endLocation);
		grrbl.GetComponent<AI_Grrbl_Behaviour>().endDestination = endLocation;
		grrbl.layer=10;//playerAI
		grrbl.GetComponent<AI_Grrbl_Behaviour>().init();
		grrbl.tag="grrblAI";
			//assign 2 items to the grrbl
		assignRandomItemsToGrrbl(grrbl,2);
	}

	static void assignRandomItemsToGrrbl(GameObject grrbl,int amount)
	{
		Manager_Grrbl_Equipment equip = grrbl.GetComponent<Manager_Grrbl_Equipment>();
		//get all armor items
		equip.equipItem(getRandomNumberFromItemIdArray(_armor));

		//get offhands
		equip.equipItem(getRandomNumberFromItemIdArray(_offHandWeapons));

		//get main hands
		equip.equipItem(getRandomNumberFromItemIdArray(_mainHandWeapons));

		//get helmets
		equip.equipItem(getRandomNumberFromItemIdArray(_helmets));
	}

	//Given an array of item ids, returns one index at random
	static int getRandomNumberFromItemIdArray(int[] array)
	{
		int rand = Random.Range(0,array.Length);
		return array[rand];
	}	

	
}
