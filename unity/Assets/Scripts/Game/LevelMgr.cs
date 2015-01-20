using UnityEngine;
using System.Collections;

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
    }

    void SetHPBarStatus(GameObject go,uint nHP,uint nMax=0)
    {
        if (go != null)
        {
            HPBar hp = go.GetComponent<HPBar>();
            if (nMax==0)
                hp.nMaxHP = 1;
            else
                hp.nMaxHP = nMax;

            hp.nHP = nHP;
        }
    }
    bool GetHPBarStatus(GameObject go, out uint nHP,out uint nMax)
    {
        if (go != null)
        {
            HPBar hp = CurrentLevel.GetComponent<HPBar>();
            nMax = hp.nMaxHP;
            nHP = hp.nHP;
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
            HPBar hp = go.GetComponent<HPBar>();

            if (hp.nHP>=0)
            {
                if (hp.nHP > nDamage)
                {
                    hp.nHP -= nDamage;
                    bKilled = false;
                }
                else
                {
                    hp.nHP = 0;
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
}
