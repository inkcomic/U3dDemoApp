using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorMgr :LevelObject{
    public GameActorStatus mStatus = null;
    public ActorController mController = null;
    public HPBar mHPBar = null;

    public WeaponHitable mWeaponHitable = null;
    public void LoadObject(string strPath)
    {
        LoadGameObjectFromPrefab(strPath);

        mStatus = mGameObj.GetComponent<GameActorStatus>();
        mController = mGameObj.GetComponent<ActorController>();
        mHPBar = mGameObj.GetComponent<HPBar>();

        mStatus.myMgr = this;

        mWeaponHitable = mGameObj.GetComponent<WeaponHitable>();
        //register weapon hit event
        mWeaponHitable.delegateWeaponHit += OnWeaponHitDamage;
        
        //register event
        mController.animDoneDelegate += OnAnimDoneDelegate;
        mController.triggerEnterDelegate += OnTriggerEnter;

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

        //temp call
        {
            if (mHPBar != null)
            {
                mHPBar.nHP = mStatus.nHP;
                mHPBar.nMaxHP = mStatus.nMaxHP;
            }
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
                case WeaponType.ePistol:
                    {
                        strFilePath = string.Format("Models/Weapon/Ranged/Prefab/pistol");
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
                    wp.transform.rotation = ts.rotation;
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
       //                     ((MeleeWeapon)(w)).meleeHitDelegate += OnMeleeHitDelegate;
                        }
                        
                    }
                }
            }
        }
    }

    public bool OnDamage(uint nDamage,GameObject other)
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


        //do effect
        {
            GameObject go = ParticleMgr.inst.PlayParticle("Effect/Prefab/1");
            go.transform.position = this.mGameObj.transform.position;
            go.transform.rotation = this.mGameObj.transform.rotation;
            //ParticleSystem ps = go.GetComponent<ParticleSystem>();
        }
        //temp call
        {
            if (mHPBar != null)
            {
                mHPBar.nHP = mStatus.nHP;
                mHPBar.nMaxHP = mStatus.nMaxHP;
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

    bool OnWeaponHitDamage(GameObject other, ActorMgr owner)
    {
        //GameActorStatus sat = other.GetComponent<GameActorStatus>();
        //if (sat != null && sat.myMgr!=null)
        {
            if (mGameObj != other)
            {
                if(OnDamage(100, this.mGameObj))
                {
                   LevelMgr.inst.OnPlayerDead(this);

                }
                return true;
            }
        }

        return false;
    }


    public override void Update()
    {
        base.Update();

    }



    public virtual bool OnWeaponHit(GameObject other, ActorMgr owner)
    {
        return OnWeaponHitDamage(other, owner);
    }



    public virtual void OnCollisionEnter(Collision col)
    {

    }
    public virtual void OnTriggerEnter(Collider cd)
    {

    }
}
