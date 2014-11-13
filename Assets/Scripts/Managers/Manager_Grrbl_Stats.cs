using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
/**
 * Handles stats
 * Handles updates from equipment
 * Accept damage and process dodge
 * Tells AI when to kill itself
 * 
 * Goes on Grrbl Prefab
 * */
public class Manager_Grrbl_Stats : MonoBehaviour {

	public GameObject hpBar;
	public int health = 100;
	public int healthmax = 100;
	public int damage = 25;
	public int accuracy = 75;
	public float dodgeChance = 10f;
	public int critChance = 10;
	public double critBonusDamage = 1.5f;
	public bool killed = false;
	public double attackSpeed = 1.2f;

	Manager_Grrbl_Stats _target;
	AI_Grrbl_Behaviour selfAI;
	float _hpBarMaxScale;

	private Dictionary<string,object> _attackPayload;

	void Awake()
	{
		selfAI = transform.GetComponent<AI_Grrbl_Behaviour>();
	}

	// Use this for initialization
	void Start () {

		updatePayload();
		_hpBarMaxScale = 1;
	}

	//set baseline stats
	void initStats()
	{

	}

	public void acceptAttack(Dictionary<string,object> attackPayload)
	{
		int damage = (int)attackPayload["damage"];
		//roll dodge
		if(!attackConnected(attackPayload))
		{
			return;
		}

		if(criticalHitOccured(attackPayload))
		{
			damage = (int)((double)damage*(double)attackPayload["critBonusDamage"]);
		}
		//take damage
		health -= damage;
		updateHealthBar();
		if(health<=0 && !killed)
		{
			killed = true;
			selfAI.killSelf();
		}
		//check for dead and kill self
	}

	void updateHealthBar()
	{
		//0 causes assignment crash
		_hpBarMaxScale = healthmax/100; //based of original healthMax
		float x = _hpBarMaxScale * health/healthmax>0?(_hpBarMaxScale * health/healthmax):0.01f;
		hpBar.transform.localScale = new Vector3(x,hpBar.transform.localScale.y,hpBar.transform.localScale.z); 
	}

	//check for dogdge
	private bool attackConnected(Dictionary<string,object> attackPayload)
	{
		float hit = Random.Range(0,100f) - (int)attackPayload["accuracy"] + dodgeChance;
		if(hit <= accuracy)
		{
			return true;
		}
		return false;
	}

	private bool criticalHitOccured(Dictionary<string,object> attackPayload)
	{
		int chanceToCrit = Random.Range(0,100);
		if(chanceToCrit<=(int)attackPayload["critChance"])
		{
			return true;
		}
		return false;
	}

	private IEnumerator selectTarget()
	{
		while(true)
		{
			if(_target==null)
			{
				//assign new target
			}

			yield return 0;
		}

	}

	public Dictionary<string,object> attackPayload
	{
		get{
			return _attackPayload;
		}
	}

	public void updateStats(JsonData data)
	{
		if(JsonData.JsonDataContainsKey(data,"damage"))
		{
			damage+=(int)data["damage"];
		}
		if(JsonData.JsonDataContainsKey(data,"dodge"))
		{
			dodgeChance+=(int)data["dodge"];
		}
		if(JsonData.JsonDataContainsKey(data,"critChance"))
		{
			critChance+=(int)data["critChance"];
		}
		if(JsonData.JsonDataContainsKey(data,"critBonusDamage"))
		{
			critBonusDamage+=(double)data["critBonusDamage"];
		}
		if(JsonData.JsonDataContainsKey(data,"health"))
		{
			health += (int)data["health"];
			healthmax += (int)data["health"];
			updateHealthBar();
		}
		updatePayload();
	}

	//update the delivery object that actually tells how much damage is done
	void updatePayload()
	{
		_attackPayload = new Dictionary<string,object>
		{
			{"damage",damage},
			{"dodgeChance",dodgeChance},
			{"accuracy",accuracy},
			{"critChance",critChance},
			{"critBonusDamage",critBonusDamage}
		};
	}

}
