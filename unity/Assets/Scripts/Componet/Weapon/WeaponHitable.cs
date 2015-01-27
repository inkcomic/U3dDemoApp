using UnityEngine;
using System.Collections;

public class WeaponHitable : MonoBehaviour {
    public delegate bool DelegateWeaponHit(Collider other, ActorMgr owner);

    public DelegateWeaponHit delegateWeaponHit = null;
 
    public bool OnWeaponHit(Collider other, ActorMgr owner)
    {
        bool wantDelete = false;
        if (delegateWeaponHit != null)
            wantDelete = delegateWeaponHit(other, owner);

        return wantDelete;
    }
  
}
