using UnityEngine;
using System.Collections;

public class BaseWeapon : MonoBehaviour {

    Material clone_mat = null;
	// Use this for initialization
	void Start () {
        clone_mat = renderer.material;
	}
	void Destroy()
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

    public enum WeaponMode
    {
        eMeleeWeapon=0,
        eRangedWeapon
    };

    GameObject my_owner = null;
    public WeaponMode weapon_mode = WeaponMode.eMeleeWeapon;
    bool is_firing = false;

    public void Setup(GameObject owner)
    {
        if(my_owner)
        {
            GameActorStatus oldS = my_owner.GetComponent<GameActorStatus>();
            oldS.currentWeapon = null;
        }
       
        GameActorStatus newS = owner.GetComponent<GameActorStatus>();
        newS.currentWeapon = this.gameObject;

        my_owner = owner;
    }
    public void SetInFire(bool bBegin)
    {
        is_firing = bBegin;
    }
 
    void OnTriggerEnter( Collider other )
    {
        
    }
    void OnTriggerExit(Collider other)
    {

    }
    void OnTriggerStay( Collider other )
    {

    }
}
