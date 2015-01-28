using UnityEngine;
using System.Collections;

public class RangedWeapon : BaseWeapon
{
    public WeaponType gunShooterType = WeaponType.ePistol;

    public GameObject bulletGameObject = null;

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

        if(bulletGameObject)
        {
            GameObject newObj = GameObject.Instantiate(bulletGameObject, gameObject.transform.position, gameObject.transform.rotation) as GameObject;

            if (newObj)
            {
                SimpleBullet newBullete = newObj.GetComponent<SimpleBullet>();
                if (newBullete)
                {
                    newBullete.Setup(ownerStatus.myMgr, gameObject.transform.forward);
                }
                else
                {
                    Destroy(newObj);
                }
            }
        }
        
    }

}
