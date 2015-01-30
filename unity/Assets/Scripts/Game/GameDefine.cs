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
public enum PickItemType
{
    eAex = WeaponType.eAex,
    ePistol = WeaponType.ePistol,
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

    static public float ViewportRatio = 1.0f;

    static public Vector3 FakeGravity = Vector3.down*5.0f;
}
