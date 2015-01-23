using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LevelMgr {

    private static LevelMgr _inst = null;
    public static LevelMgr inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = new LevelMgr();
            }
            return _inst;
        }
    }
    private LevelMgr(){}
    public Dictionary<GameObject, ActorMgr> dictLevelActor = new Dictionary<GameObject, ActorMgr>();
    
    GameObject CurrentLevel = null;
    ActorMgr CurrentPlayer = new PlayerMgr();

    ActorMgr testMonster = new ActorMgr();
    public void LoadLevel(int nId=1)
    {
        if (CurrentLevel != null)
        {
            CurrentLevel = null;
            Resources.UnloadUnusedAssets();
        }

        string strFilePath = string.Format("Levels/{0}/Level", nId);

        CurrentLevel = MyHelper.InstantiateFromResources(strFilePath);

        LoadPlayer();

        AddMonster("Models/Monster/1/Monster");
    }

    void LoadPlayer()
    {
        if (CurrentLevel!=null)
        {
            Transform ts = MyHelper.FindTransform(CurrentLevel.transform,"DummyPlayerPos");
            if(ts!=null)
            {
                string strFilePath = string.Format("Models/Player/MainPlayer");

                CurrentPlayer.LoadObject(strFilePath);
                CurrentPlayer.mGameObj.transform.position = ts.position;

            }
        }

        CurrentPlayer.SetHPBarStatus(10000000, 10000000);

        CurrentPlayer.ChangeWeapon(WeaponType.eNone);
        CurrentPlayer.ChangeWeapon(WeaponType.eAex);
 //       CurrentPlayer.ChangeWeapon(WeaponType.eGun);
        //CurrentPlayer.ChangeWeapon(WeaponType.eNone);
    }

    ActorMgr AddMonster(string modelPath)
    {
        ActorMgr newActor = new ActorMgr();
        {
            string strFilePath = string.Format(modelPath);

            newActor.LoadObject(strFilePath);
            newActor.mGameObj.transform.position = new Vector3(0, 1, 0);
        }

        newActor.SetHPBarStatus(10000000, 10000000);

        //keep in scene dictionary
        dictLevelActor.Add(newActor.mGameObj, newActor);

        return newActor;
    }
    void DestroyActor(ActorMgr actor)
    {
        dictLevelActor.Remove(actor.mGameObj);


        GameObject.Destroy(actor.mGameObj);
    }

    public ActorMgr GetPlayer()
    {
        return CurrentPlayer;
    }


    public void Update()
    {
        if (CurrentPlayer!=null)
        {
            CurrentPlayer.Update();
        }
    }
}
