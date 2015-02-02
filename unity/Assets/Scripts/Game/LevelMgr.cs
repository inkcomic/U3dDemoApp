using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

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
    public Dictionary<GameObject, LevelObject> dictLevelObject = new Dictionary<GameObject, LevelObject>();
    
    GameObject CurrentLevel = null;
    ActorMgr CurrentPlayer = new PlayerMgr();

    ActorMgr testMonster = new ActorMgr();

    public List<LevelObject> listWantDestroyLevelObject = new List<LevelObject>();
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

        CurrentPlayer.SetHPBarStatus(1000, 1000);

        CurrentPlayer.ChangeWeapon(WeaponType.eNone);
        //CurrentPlayer.ChangeWeapon(WeaponType.eAex);
        CurrentPlayer.ChangeWeapon(WeaponType.ePistol);

        AddLevelObject(CurrentPlayer);
    }

    ActorMgr AddMonster(string modelPath)
    {
        ActorMgr newActor = new ActorMgr();
        {
            string strFilePath = string.Format(modelPath);

            newActor.LoadObject(strFilePath);
            newActor.mGameObj.transform.position = new Vector3(0, 1, 0);
        }

        newActor.SetHPBarStatus(1000, 1000);

        //keep in scene dictionary
        //dictLevelActor.Add(newActor.mGameObj, newActor);
        AddLevelObject(newActor);
        return newActor;
    }
    public PickItemMgr AddItemMgr(PickItemType t)
    {
        PickItemMgr newActor = new PickItemMgr();
        newActor.Load(t);
        AddLevelObject(newActor);
        return newActor;
    }
//     ActorMgr AddPickItem(PickItemType t)
//     {
//         ActorMgr newActor = new ActorMgr();
//         {
//             string strFilePath = string.Format(modelPath);
// 
//             newActor.LoadObject(strFilePath);
//             newActor.mGameObj.transform.position = new Vector3(0, 1, 0);
//         }
// 
//         newActor.SetHPBarStatus(1000, 1000);
// 
//         //keep in scene dictionary
//         dictLevelActor.Add(newActor.mGameObj, newActor);
// 
//         return newActor;
//     }

    void DestroyActor(ActorMgr actor)
    {
        //dictLevelActor.Remove(actor.mGameObj);

        WantDestroyLevelObject(actor);
        /*GameObject.Destroy(actor.mGameObj);*/
    }

    public ActorMgr GetPlayer()
    {
        return CurrentPlayer;
    }


    public void Update()
    {
//         if (CurrentPlayer!=null)
//         {
//             CurrentPlayer.Update();
//         }

        foreach (LevelObject o in dictLevelObject.Values)
        {
            o.Update();
        }

        for (int i = 0; i < listWantDestroyLevelObject.Count;i++ )
        {
            DestroyLevelObject(listWantDestroyLevelObject[i]);
        }
        listWantDestroyLevelObject.Clear();
    }

    public void AddLevelObject(LevelObject lo)
    {
        if(dictLevelObject.ContainsKey(lo.mGameObj))
        {
            DestroyLevelObject(lo);
        }
        dictLevelObject.Add(lo.mGameObj,lo);
    }
    void WantDestroyLevelObject(LevelObject lo)
    {
        listWantDestroyLevelObject.Add(lo);
    }
    public void DestroyLevelObject(LevelObject lo)
    {
        dictLevelObject.Remove(lo.mGameObj);
        lo.Destroy();
    }
    public void DestroyLevelObject(GameObject go)
    {
        LevelObject lo;
        if(dictLevelObject.TryGetValue(go,out lo))
        {
            DestroyLevelObject(lo);
        }
        else
        {
            GameObject.Destroy(go);
        }
        
    }
    public void OnPlayerDead(ActorMgr actor)
    {
        DestroyActor(actor);



        
        for(int i=0;i<1;i++)
        {
            ActorMgr newAct = AddMonster("Models/Monster/1/Monster");
            Vector3 vec = newAct.mGameObj.transform.position;
            vec.x = i * 2;

            newAct.mGameObj.transform.position = vec;
        }
        
       // n++;
    }

    public Transform SpawnPoolObject(string preloadPrefabName, string poolName = "Default", string prefabLoadName = "")
    {
        SpawnPool shapesPool = PoolManager.Pools[poolName];
        Transform instPrefab = null;
        Transform instSpawn =null;
        if (shapesPool)
        {
            //try find preload prefab object
            if (shapesPool.prefabs.ContainsKey(preloadPrefabName))
            {
                instPrefab = shapesPool.prefabs[preloadPrefabName];
            }
            else
            {
                if (prefabLoadName != "")
                {
                    GameObject go = MyHelper.InstantiateFromResources(prefabLoadName);
                    if(go)
                        instPrefab = go.transform;
                }
            }
            
        }

        if (instPrefab)
        {
            instSpawn = shapesPool.Spawn(instPrefab);
        }
        return instSpawn;
    }



    public void DespawnPoolObject(Transform instPrefab, string poolName = "Default")
    {
        SpawnPool shapesPool = PoolManager.Pools[poolName];
        if (shapesPool)
        {
            shapesPool.Despawn(instPrefab); 
        }
    }
}
