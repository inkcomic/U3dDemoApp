using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// 
// public class LParticleManager : MonoBehaviour
// {
//     public static LParticleManager Instance
//     {
//         get
//         {
//             return LApplication.Instance.ParticleManager;
//         }
//     }
// 
//     public static GameObject ParticleRoot = null;
//     public static Dictionary<string, GameObject> ParticleRootChildren;
//     public static Dictionary<string, UnityEngine.Object> ParticlePrefabDic = new Dictionary<string, UnityEngine.Object>();
// 
//     public static void SendGameObjectToPool(LLParticle particleInfo, GameObject o)
//     {
//         string name = particleInfo.ParticleName + "Root";
//         var p = ParticleRootChildren[name];
//         o.transform.parent = p.transform;
//         o.transform.localPosition = Vector3.zero;
//     }
// 
//     public LParticleController CreateParticle(int particleID)
//     {
//         // create particle root
//         if (ParticleRoot == null)
//         {
//             ParticleRoot = new GameObject("ParticleRoot");
//             ParticleRoot.transform.position = new Vector3(0, 100000, 0);
//             ParticleRootChildren = new Dictionary<string, GameObject>();
//         }
// 
//         // try get partilce info
//         var particleInfo = LLogicObjectManager.Instance.ParticleCollection.GetParticle(particleID);
//         string name = particleInfo.ParticleName + "Root";
// 
//         if (ParticleRootChildren.ContainsKey(name) == false)
//         {
//             GameObject child = new GameObject(name);
//             child.transform.parent = ParticleRoot.transform;
//             child.transform.localPosition = Vector3.zero;
//             ParticleRootChildren.Add(name, child);
//         }
// 
//         GameObject wanted = null;
//         if (ParticleRootChildren.ContainsKey(name) == true)
//         {
//             GameObject o = ParticleRootChildren[name];
//             if (o.transform.childCount > 0)
//                 wanted = o.transform.GetChild(0).gameObject;
//         }
// 
//         LParticleController ret = null;
//         if (wanted != null)
//             ret = wanted.GetComponent<LParticleController>();
//         else
//             ret = CreateParticle(particleInfo);
// 
//         ret.transform.parent = this.transform;
//         return ret;
//     }
// 
//     private LParticleController CreateParticle(LLParticle particleInfo)
//     {
//         GameObject ret = new GameObject("ParticleController");
// 
//         if (ParticlePrefabDic.ContainsKey(particleInfo.ParticleName) == false)
//             ParticlePrefabDic.Add(particleInfo.ParticleName, Resources.Load(particleInfo.ParticlePath, typeof(GameObject)));
//         // create new gameobject and attach it to ret
//         UnityEngine.Object target = ParticlePrefabDic[particleInfo.ParticleName];
// 
//         LParticleController controller = ret.AddComponent<LParticleController>();
//         controller.transform.parent = ret.transform;
//         controller.transform.localPosition = Vector3.zero;
//         controller.ParticleInfo = particleInfo;
//         controller.prefabObject = target;
// 
//         return controller;
//     }
// 
//     public void Reset()
//     {
//         LHelper.RemoveChildren(this.gameObject);
//     }
// }



public class ParticleMgr {

    GameObject defaultParticleRoot = new GameObject("defaultParticleRoot");
    private static ParticleMgr _inst = null;
    public static ParticleMgr inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = new ParticleMgr();
            }
            return _inst;
        }
    }

    HashSet<GameObject> createdParticles = new HashSet<GameObject>();

    public static void SetActiveRecursively(GameObject target, bool bActive)
    {
#if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
		for (int n = target.transform.childCount-1; 0 <= n; n--)
			if (n < target.transform.childCount)
				SetActiveRecursively(target.transform.GetChild(n).gameObject, bActive);
		target.SetActive(bActive);
#else
        target.SetActiveRecursively(bActive);
#endif
    }

    public GameObject CreateEffect(string filePath,GameObject parentNode=null)
    {
        GameObject createObj = MyHelper.InstantiateFromResources(filePath);

//         if (!parentNode)
//         {
//             createObj.transform.parent = defaultParticleRoot.transform;
//         }
// #if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
//         SetActiveRecursively(createObj, true);
// #endif
        createdParticles.Add(createObj);

        return createObj;
    }


    public GameObject PlayParticle(string filePath, GameObject parentNode = null)
    {
        GameObject createObj = CreateEffect(filePath,parentNode);

        ParticleController pc = createObj.GetComponentInChildren<ParticleController>();

        pc.Play();
        return createObj;
    }


    public void DestroyParticle(GameObject go)
    {
        if (go.transform.parent)
        {
            go.transform.parent = null;
        }
   
        createdParticles.Remove(go);

        GameObject.Destroy(go);
    }
}
