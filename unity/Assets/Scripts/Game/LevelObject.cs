using UnityEngine;
using System.Collections;

public class LevelObject {
    public GameObject mGameObj = null;
    protected virtual GameObject LoadGameObjectFromPrefab(string strPath)
    {
        UnloadGameObject();

        mGameObj = MyHelper.InstantiateFromResources(strPath);

        return mGameObj;
    }
    protected virtual void UnloadGameObject()
    {
         if(mGameObj!=null)
         {
             GameObject.Destroy(mGameObj);
             mGameObj=null;
         }
    }
    virtual public void Destroy()
    {
        UnloadGameObject();
    }
    public virtual void Update() { }

    //~LevelObject()
    //{
    //    Destroy();
    //}
}
