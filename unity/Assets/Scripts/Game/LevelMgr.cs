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


    GameObject CurrentLevel = null;
    ActorMgr CurrentPlayer = new ActorMgr();
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

  


    public ActorMgr GetPlayer()
    {
        return CurrentPlayer;
    }


}
