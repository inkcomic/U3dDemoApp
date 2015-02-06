using UnityEngine;
using System.Collections;

public class RangedWeapon : BaseWeapon
{
    public WeaponType gunShooterType = WeaponType.ePistol;

    //public GameObject bulletGameObject = null;

    protected override void Start()
    {
        base.Start();
        weapon_mode = WeaponMode.eRangedWeapon;
    }

    protected override void Destroy()
    {
        base.Destroy();
    }

    float lastFire = 0.0f;
    float timeCounter = 0.0f;
	void Update () {


	}

    bool CanFire()
    {
        if (timeCounter - lastFire>0.2f)
        {
            lastFire = timeCounter;
            return true;
        }
        timeCounter = Time.time;

        return false;
    }
    public override void SetFire(bool bBegin)
    {
        if (!CanFire())
            return;

        base.SetFire(bBegin);

        bBegin = false;

        Transform newObj = null;
        //find pool object 
        switch (gunShooterType)
        {
            case WeaponType.ePistol:
                {
                    newObj = LevelMgr.inst.SpawnPoolObject("pistol_bullet");
                }
                break;
        }

        if (newObj)
        {
            if (newObj)
            {
                newObj.transform.position = gameObject.transform.position;
                newObj.transform.rotation = gameObject.transform.rotation;
                SimpleBullet newBullete = newObj.GetComponent<SimpleBullet>();
                if (newBullete)
                {
                    newBullete.Setup(ownerStatus.myLevelObject as ActorMgr, gameObject.transform.forward);
                }
                else
                {
                 //   Destroy(newObj);
                }
            }
        }
       
        
    }

}
