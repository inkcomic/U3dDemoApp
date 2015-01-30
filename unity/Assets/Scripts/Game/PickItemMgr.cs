using UnityEngine;
using System.Collections;

public class PickItemMgr:LevelObject{
    PickableItemController controller = null;
	public void Load(PickItemType t)
    {
        switch(t)
        {
            case PickItemType.eAex:
                {
                    LoadGameObjectFromPrefab("Models/Item/Prefab/itembox_aex");
                }
                break;
            case PickItemType.ePistol:
                {
                    LoadGameObjectFromPrefab("Models/Item/Prefab/itembox_pistol");
                }
                break;
        }
        if(mGameObj)
        {
            controller = mGameObj.GetComponent<PickableItemController>();
        }
    }

    public void ThrowIt(Vector3 _dir)
    {
        if(controller)
        {
            controller.SetInitVelocity(_dir,2.0f);
        }
    }
}
