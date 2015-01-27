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
        if (timeCounter - lastFire>1.0f)
        {
            SetFire(true);

            lastFire = timeCounter;
        }
        timeCounter += Time.deltaTime;


	}


    public override void SetFire(bool bBegin)
    {
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

                    newBullete.Setup(gameObject.transform.forward);
                }
                else
                {
                    Destroy(newObj);
                }
            }
        }
        
    }

}
