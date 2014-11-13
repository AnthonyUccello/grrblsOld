using UnityEngine;
using System.Collections;

//Keeps Track of all textures
public class Utility_Textures : MonoBehaviour {

	public Sprite[] textures;
	public string[] names;

	// Use this for initialization
	void Start () 
	{
		textures = Resources.LoadAll<Sprite>("SpriteSheets/SpriteSheet");
		names = new string[textures.Length];

		for(int i=0; i< names.Length; i++) 
		{
			names[i] = textures[i].name;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
