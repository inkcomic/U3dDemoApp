﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
public class ActorMgr :LevelObject{
    public LevelObjectInfo mStatus = null;
    public ActorController mController = null;
    public HPBar mHPBar = null;

    public WeaponHitable mWeaponHitable = null;


    public Data.ActorStatus mActorStatus = new Data.ActorStatus();
    public void LoadObject(string strPath)
    {
        LoadGameObjectFromPrefab(strPath);

        mStatus = mGameObj.GetComponent<LevelObjectInfo>();
        //mController = mGameObj.GetComponent<ActorController>();
        mController = mGameObj.GetComponent<ActorController>();
        mHPBar = mGameObj.GetComponent<HPBar>();

        mStatus.myLevelObject = this;

        mWeaponHitable = mGameObj.GetComponent<WeaponHitable>();
        //register weapon hit event
        mWeaponHitable.delegateWeaponHit += OnWeaponHitDamage;
        
        //register event
        mController.animDoneDelegate += OnAnimDoneDelegate;
        mController.triggerEnterDelegate += OnTriggerEnter;

    }
    public bool GetHPBarStatus(out uint nHP, out uint nMax)
    {
        if (mActorStatus != null)
        {
            nMax = mActorStatus.nMaxHP;
            nHP = mActorStatus.nHP;
            return true;
        }
        nHP = 0;
        nMax = 0;
        return false;
    }

    public void SetHPBarStatus(uint nHP, uint nMax = 0)
    {
        if (mActorStatus != null)
        {
            if (nMax == 0)
                mActorStatus.nMaxHP = 1;
            else
                mActorStatus.nMaxHP = nMax;

            mActorStatus.nHP = nHP;
        }

        //temp call
        {
            if (mHPBar != null)
            {
                mHPBar.nHP = mActorStatus.nHP;
                mHPBar.nMaxHP = mActorStatus.nMaxHP;
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
                        strFilePath = string.Format("Models/Weapon/Melee/Prefab/aex");
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
                LevelMgr.inst.DestroyLevelObject(o);
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
            if (mActorStatus.nHP >= 0)
            {
                if (mActorStatus.nHP > nDamage)
                {
                    mActorStatus.nHP -= nDamage;
                    bKilled = false;
                }
                else
                {
                    mActorStatus.nHP = 0;
                    bKilled = true;
                }
            }

        }

        //do effect
        {
            Transform newObj = LevelMgr.inst.SpawnPoolObject("effect_blood");

            GameObject go = newObj.gameObject;
            go.transform.position = this.mGameObj.transform.position;
            go.transform.rotation = this.mGameObj.transform.rotation;

            //ParticleMgr.inst.DestroyParticle(go,true);
            //ParticleSystem ps = go.GetComponent<ParticleSystem>();
        }
        //temp call
        {
            if (mHPBar != null)
            {
                mHPBar.nHP = mActorStatus.nHP;
                mHPBar.nMaxHP = mActorStatus.nMaxHP;
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

    bool OnWeaponHitDamage(GameObject other, ActorMgr owner,uint nDamage)
    {
        //LevelObjectInfo sat = other.GetComponent<LevelObjectInfo>();
        //if (sat != null && sat.myMgr!=null)
        {
            if (mGameObj != other)
            {
                if (OnDamage(nDamage, this.mGameObj))
                {
                    LevelMgr.inst.OnActorDead(this, mStatus.actorType);

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



    public virtual bool OnWeaponHit(GameObject other, ActorMgr owner, uint nDamage)
    {
        return OnWeaponHitDamage(other, owner,nDamage);
    }



    public virtual void OnCollisionEnter(Collision col)
    {

    }
    public virtual void OnTriggerEnter(Collider cd)
    {

    }
}
