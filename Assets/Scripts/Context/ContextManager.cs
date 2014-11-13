using UnityEngine;
using System.Collections;

public class ContextManager : MonoBehaviour {
	
	public void beginGame()
	{
		Factory_3D_GrrblSpawner.beginSpawn();
		Factory_UI_Deck.dealFirstHand();
		Overseer_PlayerHealth.init();
		Overseer_PlayerMana.init();
	}
}
