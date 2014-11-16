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

	//Stats
	public int health = 100;
	public int healthmax = 100;
	public int damage = 25;
	public int accuracy = 75;
	public int critChance = 10;
	public int armor = 0;
	public int block = 0;
	public double critBonusDamage = 1.5f;
	public double attackSpeed = 1.2f;
	public double dodgeChance = 10f;

	//flags
	public bool killed = false;

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

	//Does all calculations for:
	//Crit Chance
	//Dodge chance
	//Block chance
	//Armor
	//Bonus crit damage
	public void acceptAttack(Dictionary<string,object> attackPayload)
	{
		int damage = (int)attackPayload["damage"];
		//roll dodge
		if(attackDodged(attackPayload))
		{
			return;
		}

		if(criticalHitOccured(attackPayload))
		{
			damage = (int)((double)damage*(double)attackPayload["critBonusDamage"]);
		}
		//take damage
		health -= cacluateDamage(damage);
		updateHealthBar();
		if(health<=0 && !killed)
		{
			killed = true;
			selfAI.killSelf();
		}
		//check for dead and kill self
	}

	int cacluateDamage(int damage)
	{
		if(damage - armor < 0)
		{
			return 0;
		}

		return damage - armor;
	}

	void updateHealthBar()
	{
		//0 causes assignment crash
		_hpBarMaxScale = healthmax/100; //based of original healthMax
		float x = _hpBarMaxScale * health/healthmax>0?(_hpBarMaxScale * health/healthmax):0.01f;
		hpBar.transform.localScale = new Vector3(x,hpBar.transform.localScale.y,hpBar.transform.localScale.z); 
	}

	//check for dogdge
	private bool attackDodged(Dictionary<string,object> attackPayload)
	{
		double hit = Random.Range(0,100f) - (int)attackPayload["accuracy"] + dodgeChance;
		if(accuracy >= hit)
		{
			return false;//it was a hit
		}
		return true;
	}

	private bool criticalHitOccured(Dictionary<string,object> attackPayload)
	{
		int chanceToCrit = Random.Range(0,100);
		if((int)attackPayload["critChance"] >= chanceToCrit)
		{
			return true;
		}
		return false;
	}

	public Dictionary<string,object> attackPayload
	{
		get{return _attackPayload;}
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
		if(JsonData.JsonDataContainsKey(data,"block"))
		{
			block+=(int)data["block"];
		}
		if(JsonData.JsonDataContainsKey(data,"critChance"))
		{
			critChance+=(int)data["critChance"];
		}
		if(JsonData.JsonDataContainsKey(data,"critBonusDamage"))
		{
			critBonusDamage+=(double)data["critBonusDamage"];
		}
		if(JsonData.JsonDataContainsKey(data,"attackSpeed"))
		{
			attackSpeed+=(double)data["attackSpeed"];
		}
		if(JsonData.JsonDataContainsKey(data,"health"))
		{
			health += (int)data["health"];
			healthmax += (int)data["health"];
			updateHealthBar();
		}
		if(JsonData.JsonDataContainsKey(data,"armor"))
		{
			critBonusDamage+=(int)data["armor"];
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
			{"critBonusDamage",critBonusDamage},
			{"armor",armor},
			{"block",block}
		};
	}

}
