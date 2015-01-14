using UnityEngine;
using System.Collections;
using System;

public class UIMain  {

	static GameObject button ;

	static GameObject button1 ;

	//UICommon里面调用脚本的Start方法，并且传入了名子的字符串
    static void Start(string root)
    {
        if (root == "init")
        {
            //Main Point load UI

            App.LoadGameObjectFromAssetBundle("demo","ui.assetbundle","LoinPanel","Canvas/UICamera/Panel");

        }
    }
}

