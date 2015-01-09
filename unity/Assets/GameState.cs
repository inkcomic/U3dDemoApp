using UnityEngine;
using System.Collections;

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

	public void ResourceUpdateDone()
    {
        ResUpdateDone = true;

        MyScriptMain.inst.Start();
    }

    public void Update()
    {
        if (ResUpdateDone)
        {
            MyScriptMain.inst.Update();
        }
    }


    public void OnGUI()
    {
        if (ResUpdateDone)
        {
            MyScriptMain.inst.OnGUI();
        }
    }
}
