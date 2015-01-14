using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState{
    private static GameState _inst = null;
    public static GameState inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = new GameState();
            }
            return _inst;
        }
    }

   public bool ResUpdateDone = false;


//     IEnumerator LoadUIBundle(AssetBundle res)
//      {
//          AssetBundleRequest req = res.LoadAsync("LoinPanel", typeof(GameObject));
//          while (req.isDone == false)
//              yield return null;
//          GameObject objGUIRes = null;
// 
//          objGUIRes = req.asset as GameObject;
// 
//          res.Unload(false);
//  
//          GameObject _father = GameObject.Find("Canvas/UICamera/Panel");
//          {
//              GameObject ret = GameObject.Instantiate(objGUIRes) as GameObject;
//              ret.name = objGUIRes.name;
//              ret.transform.parent = _father.transform;
//              ret.transform.localPosition = Vector3.zero;
//              ret.transform.localScale = Vector3.one;
//          }
//      }

	public void ResourceUpdateDone()
    {
        ResUpdateDone = true;

       


        //测试加载 Prefab UI,优先用热更新中的资源查找，没有则用包内的资源
        {
//             LocalVersion.ResInfo test;
//             if (ResmgrNative.Instance.verLocal.groups["test1"].listfiles.TryGetValue("ui.assetbundle", out test))
//             {
//                 test.BeginLoadAssetBundle((res, tag) =>
//                 {
//                     GameObject objGUIRes = null;
// 
//                     objGUIRes = res.Load("LoinPanel", typeof(GameObject)) as GameObject;
// 
//                     res.Unload(false);
// 
//                     GameObject _father = GameObject.Find("Canvas/UICamera/Panel");
//                     {
//                         GameObject ret = GameObject.Instantiate(objGUIRes) as GameObject;
//                         ret.name = objGUIRes.name;
//                         ret.transform.parent = _father.transform;
//                         ret.transform.localPosition = Vector3.zero;
//                         ret.transform.localScale = Vector3.one;
//                     }
//                 });
//             }

//             if (ResmgrNative.Instance.verLocal.groups["test1"].listfiles.TryGetValue("ui.assetbundle", out test))
//             {
//                // if ((test.state & LocalVersion.ResState.ResState_UseDownloaded) == LocalVersion.ResState.ResState_UseDownloaded)
//                 {
//                     test.BeginLoadAssetBundle((res, tag) =>
//                     {
//                         resdown.inst.StartChildCoroutine(LoadUIBundle(res));
// 
// //                         objGUIRes = res.Load("LoinPanel", typeof(GameObject)) as GameObject;
// // 
// //                         res.Unload(false);
// // 
// //                         GameObject _father = GameObject.Find("Canvas/UICamera/Panel");
// //                         {
// //                             GameObject ret = GameObject.Instantiate(objGUIRes) as GameObject;
// //                             ret.name = objGUIRes.name;
// //                             ret.transform.parent = _father.transform;
// //                             ret.transform.localPosition = Vector3.zero;
// //                             ret.transform.localScale = Vector3.one;
// //                         }
// // 
//                      });
//                 }
// 
//             }
//             else
//             {
//                 objGUIRes = Resources.Load("DefaultRes/LoinPanel") as GameObject;
// 
//                 GameObject _father = GameObject.Find("Canvas/UICamera/Panel");
//                 {
//                     GameObject ret = GameObject.Instantiate(objGUIRes) as GameObject;
//                     ret.name = objGUIRes.name;
//                     ret.transform.parent = _father.transform;
//                     ret.transform.localPosition = Vector3.zero;
//                     ret.transform.localScale = Vector3.one;
// 
//                 }
//             }

            
        }

        MyScriptInterface.inst.Start();
//         //try load GUI 
//         foreach (var file in ResmgrNative.Instance.verLocal.groups["test1_ios"].listfiles.Values)
//         {
//             if (file.FileName.Contains(".jpg"))
//             {
//                 file.BeginLoadTexture2D((tex, tag) =>
//                 {
//                     loadedTexs.Add(tex);
//                 });
//             }
//         }
    }

    public void Update()
    {
        if (ResUpdateDone)
        {
            MyScriptInterface.inst.Update();
        }
    }


    public void OnGUI()
    {
        if (ResUpdateDone)
        {
            MyScriptInterface.inst.OnGUI();
        }
    }
}
