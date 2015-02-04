using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class MeleeWeapon : BaseWeapon {

    //event delegate
    //public delegate void MeleeHitDelegate(GameObject other);
    // public MeleeHitDelegate meleeHitDelegate = null;


   // HashSet<GameObject> alreadySendMsg = new HashSet<GameObject>();

    ActorMgr mOwnerActor = null;
	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
    protected override void Destroy()
    {
        base.Destroy();
	}

    public override void Setup(GameObject owner)
    {
        base.Setup(owner);

        mOwnerActor = ownerStatus.myMgr;
    }

    void OnTriggerEnter(Collider other)
    {
        if (is_firing)
        {
            WeaponHitable hitable = other.GetComponent<WeaponHitable>();
            if (hitable)
                hitable.OnWeaponHit(mOwnerActor.mGameObj, ownerStatus.myMgr,500);
        }
       

    }
    void OnTriggerExit(Collider other)
    {

    }
    void OnTriggerStay(Collider other)
    {
//         if (is_firing)
//         {
//             if (alreadySendMsg.Add(other.gameObject))
//             {
//                 if (meleeHitDelegate!=null)
//                 {
//                     meleeHitDelegate(other.gameObject);
//                 }
//             }
//         }
    }


    public override void SetFire(bool bBegin)
    {
        base.SetFire(bBegin);

//         if (!bBegin)
//             alreadySendMsg.Clear();

    }

}
