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
    GameObject CurrentPlayer = null;
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

                CurrentPlayer = MyHelper.InstantiateFromResources(strFilePath);

                CurrentPlayer.transform.position = ts.position;
            }
        }

        SetHPBarStatus(CurrentPlayer, 10000000, 10000000);

        ChangeWeapon(CurrentPlayer,WeaponType.eNone);
        ChangeWeapon(CurrentPlayer, WeaponType.eAex);
        ChangeWeapon(CurrentPlayer, WeaponType.eGun);
        ChangeWeapon(CurrentPlayer, WeaponType.eNone);
    }

    void SetHPBarStatus(GameObject go,uint nHP,uint nMax=0)
    {
        if (go != null)
        {
            GameActorStatus status = go.GetComponent<GameActorStatus>();
            if (nMax==0)
                status.nMaxHP = 1;
            else
                status.nMaxHP = nMax;

            status.nHP = nHP;
        }
    }
    bool GetHPBarStatus(GameObject go, out uint nHP,out uint nMax)
    {
        if (go != null)
        {
            GameActorStatus status = CurrentLevel.GetComponent<GameActorStatus>();
            nMax = status.nMaxHP;
            nHP = status.nHP;
            return true;
        }
        nHP = 0;
        nMax = 0;
        return false;
    }


    public bool DamageIt(GameObject go, uint nDamage)
    {
        bool bKilled = false;
        if (go != null)
        {
            GameActorStatus status = go.GetComponent<GameActorStatus>();

            if (status.nHP >= 0)
            {
                if (status.nHP > nDamage)
                {
                    status.nHP -= nDamage;
                    bKilled = false;
                }
                else
                {
                    status.nHP = 0;
                    bKilled = true;
                }
            }

        }

        return bKilled;
    }
    public GameObject GetPlayer()
    {
        return CurrentPlayer;
    }



    public void ChangeWeapon(GameObject go,WeaponType tp)
    {
        if (go != null)
        {
            string strFilePath = "";
            switch(tp)
            {
                case WeaponType.eAex:
                    {
                        strFilePath = string.Format("Models/Weapon/Prefab/aex");
                    }
                    break;
                case WeaponType.eGun:
                    {
                        strFilePath = string.Format("Models/Weapon/Prefab/gun");
                    }
                    break;
            }

            Transform ts = MyHelper.FindTransform(go.transform, "weapon_locator");
            
            //destroy all weapon under hand
            List<Transform> old_ilst = new List<Transform>();

            for (int i = 0; i < ts.childCount;i++ )
            {
                old_ilst.Add(ts.GetChild(i));
            }
            //ts.DetachChildren();
            foreach(var o in old_ilst)
            {
                GameObject.Destroy(o);
            }

            GameActorStatus s = go.GetComponent<GameActorStatus>();
            s.currentWeapon = null;

            if (tp != WeaponType.eNone)
            {
                //attach new weapon
                if (ts != null)
                {
                    GameObject wp = MyHelper.InstantiateFromResources(strFilePath);

                    wp.transform.position = ts.position;
                    wp.transform.parent = ts.transform;

                    BaseWeapon w = wp.GetComponent<BaseWeapon>();
                    w.Setup(go);
                }
            }
        }
    }
    public void ActorFireOnce(GameObject go)
    {
        if (go != null)
        {
            GameActorStatus s = go.GetComponent<GameActorStatus>();
            if(s.currentWeapon!=null)
            {
                BaseWeapon w = s.currentWeapon.GetComponent<BaseWeapon>();
                //w.
            }
        }
    }
}
