using UnityEngine;
using System.Collections;

public class ItemPackMgr:LevelObject{
    ItemPackController controller = null;
    public void Load(ItemPackType t)
    {
        switch(t)
        {
            case ItemPackType.eAex:
                {
                    LoadGameObjectFromPrefab("Models/Item/Prefab/itembox_aex");
                }
                break;
            case ItemPackType.ePistol:
                {
                    LoadGameObjectFromPrefab("Models/Item/Prefab/itembox_pistol");
                }
                break;
        }
        if(mGameObj)
        {
            controller = mGameObj.GetComponent<ItemPackController>();
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
