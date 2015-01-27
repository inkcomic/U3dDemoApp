using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class UIMain
{

    static GameObject button = null;

    static GameObject button1=null;

    //UICommon里面调用脚本的Start方法，并且传入了名子的字符串
    static void Start(string root)
    {
        if (root == "init")
        {
            //Main Point load UI

            App.LoadGameObjectFromAssetBundle("demo", "ui.assetbundle", "LoinPanel", "Canvas/UICamera/Panel", () =>
            {
                GameObject obj = GameObject.Find("Canvas/UICamera/Panel/LoinPanel/Button");
                button = obj;
                //监听按钮事件
                 {
                     //方法一
                     EventTriggerListener.Get(button).onClick = Click;
                 }
            });
           
        }
    }

    static void doClick()
    {
        GameObject obj1 = GameObject.Find("Canvas/UICamera/Panel/LoinPanel/InputFieldAccount");
        GameObject obj2 = GameObject.Find("Canvas/UICamera/Panel/LoinPanel/InputFieldPasswd");

        InputField inputLogin = obj1.GetComponent<InputField>();
        InputField inputPasswd = obj2.GetComponent<InputField>();
        if (inputLogin != null)
        {
            Debug.Log("LoginText:"+inputLogin.text);
        }
        if (inputPasswd != null)
        {
            Debug.Log("PasswdText:" + inputPasswd.text);
        }
        
    }
    //当按钮点击的时候在脚本中得到回调
    static void Click(GameObject go)
    {
        //根据不同的按钮执行不同的逻辑
        if (go == button)
        {
            doClick();
        }
        else if (go.Equals(button1))
        {
            
        }
    }
}

