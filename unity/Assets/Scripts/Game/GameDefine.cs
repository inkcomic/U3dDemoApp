using UnityEngine;
using System.Collections;

public enum WeaponMode
{
    eMeleeWeapon=0,
    eRangedWeapon
};
public enum ActorType
{
    eMonster=0,
    eMainPlayer,
    
};
public enum WeaponType
{
    eNone = 0,
    //melee weapon
    eAex,

    //ranged weapon
    ePistol,
};

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
