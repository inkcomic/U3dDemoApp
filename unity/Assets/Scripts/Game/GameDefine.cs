using UnityEngine;
using System.Collections;

public class GlobalDefine {

	 private static GlobalDefine _inst = null;
    public static GlobalDefine inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = new GlobalDefine();
            }
            return _inst;
        }
    }
    private GlobalDefine() { }



    public float ViewportRatio = 1.0f;
}
