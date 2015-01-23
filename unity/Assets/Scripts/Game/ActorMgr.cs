using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorMgr {
    public GameObject mGameObj = null;
    public GameActorStatus mStatus = null;
    public ActorController mController = null;
    public void LoadObject(string strPath)
    {
        if(mGameObj!=null)
            GameObject.Destroy(mGameObj);

        mGameObj = MyHelper.InstantiateFromResources(strPath);

        mStatus = mGameObj.GetComponent<GameActorStatus>();
        mController = mGameObj.GetComponent<ActorController>();

        //register animDone event
        mController.animDoneDelegate += OnAnimDoneDelegate;
    }
    public bool GetHPBarStatus(out uint nHP, out uint nMax)
    {
        if (mStatus != null)
        {
            nMax = mStatus.nMaxHP;
            nHP = mStatus.nHP;
            return true;
        }
        nHP = 0;
        nMax = 0;
        return false;
    }

    public void SetHPBarStatus(uint nHP, uint nMax = 0)
    {
        if (mStatus != null)
        {
            if (nMax == 0)
                mStatus.nMaxHP = 1;
            else
                mStatus.nMaxHP = nMax;

            mStatus.nHP = nHP;
        }
    }

    public void ChangeWeapon(WeaponType tp)
    {
        if (mGameObj != null)
        {
            string strFilePath = "";
            switch (tp)
            {
                case WeaponType.eAex:
                    {
                        strFilePath = string.Format("Models/Weapon/Prefab/aex");
                    }
                    break;
                case WeaponType.eGun:
                    {
                        strFilePath = string.Format("Models/Weapon/Prefab/gun");
                    }
                    break;
            }

            Transform ts = MyHelper.FindTransform(mGameObj.transform, "weapon_locator");

            //destroy all weapon under hand
            List<GameObject> old_ilst = new List<GameObject>();

            for (int i = 0; i < ts.childCount; i++)
            {
                old_ilst.Add(ts.GetChild(i).gameObject);
            }
            //ts.DetachChildren();
            foreach (var o in old_ilst)
            {
                GameObject.Destroy(o);
            }

            if (mStatus!=null)
            {
                mStatus.currentWeapon = null;
            }

            if (tp != WeaponType.eNone)
            {
                //attach new weapon
                if (ts != null)
                {
                    GameObject wp = MyHelper.InstantiateFromResources(strFilePath);

                    wp.transform.position = ts.position;
                    wp.transform.parent = ts.transform;

                    BaseWeapon w = wp.GetComponent<BaseWeapon>();
                    if(w!=null)
                        w.Setup(mGameObj);
                }

                //register event
                {
                    if (mStatus.currentWeapon != null)
                    {
                        BaseWeapon w = mStatus.currentWeapon.GetComponent<BaseWeapon>();
                        if (w.weapon_mode == WeaponMode.eMeleeWeapon)
                        {
                            ((MeleeWeapon)(w)).meleeHitDelegate += OnMeleeHitDelegate;
                        }
                        
                    }
                }
            }
        }
    }

    public bool Damage(uint nDamage)
    {
        bool bKilled = false;
        if (mGameObj != null)
        {
            if (mStatus.nHP >= 0)
            {
                if (mStatus.nHP > nDamage)
                {
                    mStatus.nHP -= nDamage;
                    bKilled = false;
                }
                else
                {
                    mStatus.nHP = 0;
                    bKilled = true;
                }
            }

        }

        return bKilled;
    }

    public void ActorFire()
    {
        if (mStatus != null&&mController!=null)
        {
            mController.DidMeleeAttack();

            SetWeaponFire(true);
        }
    }
    void SetWeaponFire(bool bValue)
    {
        if(mStatus.currentWeapon!=null)
        {
            BaseWeapon w = mStatus.currentWeapon.GetComponent<BaseWeapon>();
            w.SetFire(bValue);
        }
    }

    void OnAnimDoneDelegate(ActorController.CharacterState state)
    {
        SetWeaponFire(false);
    }

    void OnMeleeHitDelegate(Collider other)
    {
        int i = 0;
        i++;
    }
}
