using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UICommon : MonoBehaviour {

	void Start ()
	{
		ScriptMgr.Instance.LoadProject();
		ScriptMgr.Instance.Execute("UIMain.Start(\""+name+"\");");
	}
}
