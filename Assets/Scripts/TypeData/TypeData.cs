using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class TypeData : MonoBehaviour {

	public static M_TypeCards mTypeCards;
	public static M_TypeItems mTypeItems;

	static Dictionary<string,M_TypeClass> _typeTables = new Dictionary<string, M_TypeClass>();
	void Awake()
	{
		mTypeCards = GameObject.Find("M_TypeCards").GetComponent<M_TypeCards>();
		mTypeItems = GameObject.Find("M_TypeItems").GetComponent<M_TypeItems>();
	}

	//takes a sheet and sets it to the appropriate type class
	public static void REGISTER_CLASS(string sheetName, JsonData[] ssObjects)
	{
		if(sheetName == "cards")
		{
			mTypeCards.initialize(ssObjects);
			_typeTables.Add("cards",mTypeCards);
		}

		if(sheetName == "items")
		{
			mTypeItems.initialize(ssObjects);
			_typeTables.Add("items",mTypeItems);
		}
	}

	//return a type table
	public static M_TypeClass GET_TABLE(string tableName)
	{
		return _typeTables[tableName] as M_TypeClass;
	}
}
