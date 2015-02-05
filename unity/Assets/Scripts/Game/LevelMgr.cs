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

    List<Vector3> listMonsterSpawner = new List<Vector3>();
    public void LoadLevel(int nId=1)
    {
        if (CurrentLevel != null)
        {
            CurrentLevel = null;
            Resources.UnloadUnusedAssets();
        }

        string strFilePath = string.Format("Levels/{0}/Level", nId);

        CurrentLevel = MyHelper.InstantiateFromResources(strFilePath);
        CurrentLevel.name = "Level";
        LoadLevelSpecialPoint();
        LoadPlayer();

        SpawnMonster(1);
    }
    public void LoadLevelSpecialPoint()
    {
        listMonsterSpawner.Clear();

        GameObject go = GameObject.Find("Level/Special");
        foreach (Transform child in go.transform)
        {
            if (child.gameObject.name == "MonsterSpawner")
            {
                listMonsterSpawner.Add(child.position);
            }
        }
    }

    public void SpawnMonster(int nNum)
    {
        //find nearest point
        if (listMonsterSpawner.Count>0)
        {
            ActorMgr player = GetPlayer();
            Vector3 vPlayer = player.mGameObj.transform.position;
            Vector3 vNearest = listMonsterSpawner[0];
            foreach (Vector3 v in listMonsterSpawner)
            {

                if ((v - vPlayer).sqrMagnitude < vNearest.sqrMagnitude)
                {
                    vNearest = v;
                }
            }

            // add monster
            for (int i = 0; i < nNum; i++)
            {
                ActorMgr newAct = AddMonster("Models/Monster/1/Monster");
                Vector3 vec = newAct.mGameObj.transform.position;
                vec.x = i * 2;

                newAct.mGameObj.transform.position = vec;

                newAct.mGameObj.transform.position += vNearest;
            }

        }
        
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
    public void OnActorDead(ActorMgr actor,ActorType tt)
    {
        //random give gift
        if(Random.Range(0,100)<=10)
        {
            PickItemMgr it = AddItemMgr((Random.Range(0, 2) == 0) ? PickItemType.eAex : PickItemType.ePistol);
            it.mGameObj.transform.position = actor.mGameObj.transform.position + new Vector3(0, 0.8f, 0);
            it.ThrowIt((Vector3.zero - actor.mGameObj.transform.transform.forward) + new Vector3(0, 1.0f, 0));
        }
        
        DestroyActor(actor);

        if (tt==ActorType.eMonster)
        {
            if (Random.Range(0, 100) <= 10)
            {
                SpawnMonster(3);
            }
            else
            {
                SpawnMonster(1);
            }
            
        }
        else
        {

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



    public void DespawnPoolObject(Transform instPrefab,float seconds=0.0f, string poolName = "Default")
    {
        SpawnPool shapesPool = PoolManager.Pools[poolName];
        if (shapesPool)
        {
            shapesPool.Despawn(instPrefab, seconds); 
        }
    }
}
