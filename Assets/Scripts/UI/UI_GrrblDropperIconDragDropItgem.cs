using UnityEngine;
using System.Collections;

/*/
Is put on the icon of the grrbl where the player can drag and drop.
grrblDropper/panel/frame/icon
*/
public class UI_GrrblDropperIconDragDropItgem : UIDragDropItem {

	int _amount = 1;
	Transform _parent;
	Vector3 _localPostion;

	protected void OnDragStart()
	{
		//tell card to hide tool tip
		_localPostion = transform.localPosition;
		_parent = transform.parent;
		base.OnDragStart();
	}
	
	protected override void OnDragDropRelease (GameObject surface)
	{	
		if(_amount > Overseer_PlayerMana.playerMana)
		{
			Debug.Log("Cannot Afford");
		}else if (surface != null && surface.tag=="lane1GrrblDropSpot")
		{
			Factory_3D_GrrblSpawner.spawnPlayerGrrbl(1);
		}else if (surface != null && surface.tag=="lane2GrrblDropSpot")
		{
			Factory_3D_GrrblSpawner.spawnPlayerGrrbl(2);
		}else if (surface != null && surface.tag=="lane3GrrblDropSpot")
		{
			Factory_3D_GrrblSpawner.spawnPlayerGrrbl(3);
		}

		transform.parent=_parent;
		transform.localPosition = _localPostion;
		base.OnDragDropRelease(surface);
	}
}
