using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class MeleeWeapon : BaseWeapon {

    //event delegate
     public delegate void MeleeHitDelegate(Collider other);
     public MeleeHitDelegate meleeHitDelegate = null;


    HashSet<Collider> alreadySendMsg=new HashSet<Collider>();
	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
    protected override void Destroy()
    {
        base.Destroy();
	}

    void OnTriggerEnter(Collider other)
    {

    }
    void OnTriggerExit(Collider other)
    {

    }
    void OnTriggerStay(Collider other)
    {
        if (is_firing)
        {
            if (alreadySendMsg.Add(other))
            {
                if (meleeHitDelegate!=null)
                {
                    meleeHitDelegate(other);
                }
            }
        }
    }


    public override void SetFire(bool bBegin)
    {
        base.SetFire(bBegin);

        if (!bBegin)
            alreadySendMsg.Clear();

    }

}
