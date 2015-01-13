using UnityEngine;
using System.Collections;
using System;

//This is Dynmic Script
public class ScriptMain
{
    static scriptFun1 f1 = new scriptFun1();



    static void Run()
    {
        App.onupdate += Update;
        App.onclick += Click;
        App.AddButton(new Rect(0, 200, 200, 50), "state1", 1);
        App.AddButton(new Rect(200, 200, 200, 50), "state2", 2);
        App.AddButton(new Rect(400, 200, 200, 50), "state3", 3);
        App.AddButton(new Rect(0, 250, 200, 50), "Play", 10);
        //、、App.AddButton(new Rect(200, 250, 200, 50), "Stop", 11);
        App.AddButton(new Rect(200, 250, 200, 50), f1.GetSpecialName(), 11);
        Debug.Log("ScriptMain Start.");

       // Action<AssetBundle, string> act = (AssetBundle res, string tag) => 
       // {
       //     GameObject objGUIRes = null;

       //     objGUIRes = res.Load("LoinPanel", typeof(GameObject)) as GameObject;

       //     res.Unload(false);

       //     GameObject _father = GameObject.Find("Canvas/UICamera/Panel");
       //     {
       //         GameObject ret = GameObject.Instantiate(objGUIRes) as GameObject;
       //         ret.name = objGUIRes.name;
       //         ret.transform.parent = _father.transform;
       //         ret.transform.localPosition = Vector3.zero;
       //         ret.transform.localScale = Vector3.one;
       //     }
       //};
       // Action<AssetBundle, string> act = null;
        //App.onLoadAssetBundle += (AssetBundle res, string tag) => { };
        //测试加载 Prefab UI,优先用热更新中的资源查找，没有则用包内的资源
        App.LoadAssetBundle("test1", "ui.assetbundle", (AssetBundle res, string tag) => 
        {
            GameObject objGUIRes = null;

            objGUIRes = (GameObject)res.Load("LoinPanel", typeof(GameObject));

            res.Unload(false);

            GameObject _father = GameObject.Find("Canvas/UICamera/Panel");
            {
                GameObject ret = (GameObject)GameObject.Instantiate(objGUIRes);
                ret.name = objGUIRes.name;
                ret.transform.parent = _father.transform;
                ret.transform.localPosition = Vector3.zero;
                ret.transform.localScale = Vector3.one;
            }
       });
        
    }

    static void Update()
    {
        f1.Update(Time.deltaTime);
        
       // int c = 0;
        if (curState != null)
        {
            curState.OnUpdate();
        }
        //Debug.Log("ScriptMain Update.");
    }
    static void Click(int i)
    {
        if (i == 1)
        {
            ChangeState(new State1());
        }
        if (i == 2)
        {
            ChangeState(new State2());
        }
        if (i == 3)
        {
            ChangeState(new State3());
        }
        if(i==10)
        {
            if (curState != null)
                curState.ClickEvent(1);
        }
        if (i == 11)
        {
            if (curState != null)
                curState.ClickEvent(2);
        }
    }

    static IState curState;
    static void ChangeState(IState state)
    {
        if (curState != null)
            curState.OnExit();
        curState = state;
        if (curState != null)
            curState.OnInit();
    }
}
