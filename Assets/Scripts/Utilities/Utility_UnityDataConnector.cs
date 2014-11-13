using System.Collections;
using UnityEngine;
using LitJson;
using System.Collections.Generic;
using System.ComponentModel;

public class Utility_UnityDataConnector : MonoBehaviour
{
	public ContextManager contextManager;

	public string webServiceUrl = "";
	public string spreadsheetId = "";
	public string[] sheetNames;
	public string password = "";
	public float maxWaitTime = 10f;

	string currentStatus;
	JsonData[] ssObjects;

	int _count;

	void Start ()
	{
		_count = 0;
		foreach(string sheetName in sheetNames)
		{
			StartCoroutine(GetData(sheetName));   
		}
	}

	IEnumerator GetData(string sheetName)
	{
		string connectionString = webServiceUrl + "?ssid=" + spreadsheetId + "&sheet=" + sheetName + "&pass=" + password + "&action=GetData";
		//Debug.Log("Connecting to webservice on " + connectionString);
		
		WWW www = new WWW(connectionString);
		
		float elapsedTime = 0.0f;
		currentStatus = "Etablishing Connection... ";
		
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= maxWaitTime)
			{
				currentStatus = "Max wait time reached, connection aborted.";
				Debug.Log(currentStatus);
				break;
			}
			
			yield return null;  
		}
		//Debug.Log(currentStatus);

		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			currentStatus = "Connection error after" + elapsedTime.ToString() + "seconds: " + www.error;
			Debug.LogError(currentStatus);
			yield break;
		}
		//Debug.Log(currentStatus);
		string response = www.text;
		//Debug.Log(elapsedTime + " : " + response);
		currentStatus = "Connection stablished, parsing data...";
		
		if (response == "\"Incorrect Password.\"")
		{
			currentStatus = "Connection error: Incorrect Password.";
			Debug.LogError(currentStatus);
			yield break;
		}

		try 
		{
			ssObjects = JsonMapper.ToObject<JsonData[]>(response);
		}
		catch
		{
			currentStatus = "Data error: could not parse retrieved data as json.";
			Debug.LogError(currentStatus);
			yield break;
		}
		
		currentStatus = "Data Successfully Retrieved!";

		TypeData.REGISTER_CLASS(sheetName,ssObjects);

		_count++;
		if(_count==sheetNames.Length)
		{
			contextManager.beginGame();
		}
	}

}
	
	