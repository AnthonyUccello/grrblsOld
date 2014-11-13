using UnityEngine;
using System.Collections;
using LitJson;
public class M_TypeClass: MonoBehaviour {

	JsonData[] _rows;

	public void initialize(JsonData[] ssObjects)
	{
		_rows = ssObjects;
	}

	public JsonData[] rows{
		get{return _rows;}
	}
}
