using UnityEngine;
using System.Collections;

public class WeaponHitable : MonoBehaviour {
    public delegate bool DelegateWeaponHit(GameObject other, ActorMgr owner, uint nDamage);

    public DelegateWeaponHit delegateWeaponHit = null;

    public bool OnWeaponHit(GameObject other, ActorMgr owner, uint nDamage)
    {
        bool wantDelete = true;
        if (delegateWeaponHit != null)
            wantDelete = delegateWeaponHit(other, owner, nDamage);

        return wantDelete;
    }
  
}
