using UnityEngine;
using System.Collections;

public class BaseWeapon : MonoBehaviour {

    Material clone_mat = null;

    protected GameActorStatus ownerStatus = null;

	// Use this for initialization
	protected virtual void Start () {
        clone_mat = renderer.material;
	}
    protected virtual void Destroy()
    {
        Object.Destroy(clone_mat);
    }
	// Update is called once per frame
	void Update () {
        
        //debug
        if (is_firing)
        {
            clone_mat.color = new Color(1, 0, 0, 1);
        }
        else
        {
            clone_mat.color = new Color(1, 1, 1, 1);
        }
	}

    //GameObject my_owner = null;

    [HideInInspector]
    public WeaponMode weapon_mode = WeaponMode.eMeleeWeapon;
    protected bool is_firing = false;

    public virtual void Setup(GameObject owner)
    {
        if (ownerStatus)
        {
           // GameActorStatus oldS = my_owner.GetComponent<GameActorStatus>();
            ownerStatus.currentWeapon = null;
        }

        ownerStatus = owner.GetComponent<GameActorStatus>();
        ownerStatus.currentWeapon = this.gameObject;

        //my_owner = owner;
    }
    public virtual void SetFire(bool bBegin)
    {
        is_firing = bBegin;
    }
 
}
