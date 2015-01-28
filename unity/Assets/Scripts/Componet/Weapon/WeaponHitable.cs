using UnityEngine;
using System.Collections;

public class WeaponHitable : MonoBehaviour {
    public delegate bool DelegateWeaponHit(GameObject other, ActorMgr owner);

    public DelegateWeaponHit delegateWeaponHit = null;
 
    public bool OnWeaponHit(GameObject other, ActorMgr owner)
    {
        bool wantDelete = true;
        if (delegateWeaponHit != null)
            wantDelete = delegateWeaponHit(other, owner);

        return wantDelete;
    }
  
}
