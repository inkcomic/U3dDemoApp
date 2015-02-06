using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerMgr : ActorMgr{
    
    GameObject mNearestGO = null;

    bool isAutoAtkMode = true;
    bool isAtkBtnPressing = false;
    bool isAtkJoyPressing = false;
	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();

        UpdateInput();
	}


    void UpdateInput()
    {
#if UNITY_EDITOR
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        OnPadMove(new Vector2(h, v));


        if (Input.GetKey(KeyCode.Alpha1))
        {
            LevelMgr.inst.GetPlayer().ChangeWeapon(WeaponType.eAex);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            LevelMgr.inst.GetPlayer().ChangeWeapon(WeaponType.ePistol);
        }


        if (Input.GetButton("Fire2"))
        {
            OnPadBtnPressing();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            OnPadBtnUp();
        }
#endif
    }


    public override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col);

        //check item
        ItemPackController item = col.collider.GetComponent<ItemPackController>();
        if (item)
        {
            switch (item.mItemType)
            {
                case ItemPackType.eAex:
                    {
                        ChangeWeapon((WeaponType)item.mItemType);
                    }
                    break;
                case ItemPackType.ePistol:
                    {
                        ChangeWeapon((WeaponType)item.mItemType);
                    }
                    break;
            }

            LevelMgr.inst.DestroyLevelObject(item.gameObject);
            /*GameObject.Destroy(item.gameObject);*/
        }
    }

    public override void OnTriggerEnter(Collider cd)
    {
        base.OnTriggerEnter(cd);

        //check item
        ItemPackController item = cd.GetComponent<ItemPackController>();
        if (item && item.IsUseable())
        {
            //throw old weapon (create a fake one)
            if (mStatus.currentWeapon != null)
            {
                BaseWeapon w = mStatus.currentWeapon.GetComponent<BaseWeapon>();
                if (w.weapon_mode == WeaponMode.eMeleeWeapon)
                {
                    ItemPackMgr it = LevelMgr.inst.AddItemMgr(ItemPackType.eAex);
                    it.mGameObj.transform.position = mGameObj.transform.position + new Vector3(0, 0.8f, 0);
                    it.ThrowIt(mGameObj.transform.transform.forward + new Vector3(0, 1.0f, 0));
                }
                else
                {
                    ItemPackMgr it = LevelMgr.inst.AddItemMgr(ItemPackType.ePistol);
                    it.mGameObj.transform.position = mGameObj.transform.position + new Vector3(0, 0.8f, 0);
                    it.ThrowIt(mGameObj.transform.transform.forward + new Vector3(0, 1.0f, 0));
                }

            }


            switch (item.mItemType)
            {
                case ItemPackType.eAex:
                    {
                        ChangeWeapon((WeaponType)item.mItemType);
                    }
                    break;
                case ItemPackType.ePistol:
                    {
                        ChangeWeapon((WeaponType)item.mItemType);
                    }
                    break;
            }

            LevelMgr.inst.DestroyLevelObject(item.gameObject);

            
            
        }
    }
        
    public void OnPadMove(Vector2 axis)
    {
        Transform cameraTransform = Camera.main.transform;
        // Forward vector relative to the camera along the x-z plane	
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        // Right vector relative to the camera
        // Always orthogonal to the forward vector
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float v = axis.y;
        float h = axis.x;

        // Are we moving backwards or looking backwards
        if (v < -0.2f)
            mController.movingBack = true;
        else
            mController.movingBack = false;

        bool wasMoving = mController.isMoving;
        mController.isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;

        // Target direction relative to the camera
        Vector3 targetDirection = h * right + v * forward;

        mController.targetDirection = targetDirection;


        if (isAutoAtkMode)
        {
            
        }
        else
        //build orientation pad
        {
            mNearestGO = null;

            if ((h != 0 || v != 0) && !isAtkJoyPressing)
            {
                //see OnAttackPadMovePressing 
                // Target direction relative to the camera
                Vector3 orientation = h * right + v * forward;

                orientation.y = 0;
                mController.orientationVec = orientation;
                mController.manulOrientation = true;
            }
            else
            {
                mController.manulOrientation = false;
            }
        }
    }


    GameObject GetNearestMonster(float radius=3.5f)
    {
        GameObject nearestGO = null;
        List<GameObject> listObj = new List<GameObject>();

        Vector3 v = mGameObj.transform.position;
        Collider []cds = Physics.OverlapSphere(v, radius);
        if (cds!=null)
        {
            for (int i = 0; i < cds.Length;i++ )
            {
                Collider c = cds[i];
                
                //if (sta && (sta.actorType == ActorType.eMonster))
                if (c.gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    listObj.Add(c.gameObject);
                }
            }

           
            {
                float lastSqrMag = -1;
                //foreach(var o in LevelMgr.inst.dictLevelActor)
                foreach (var o in listObj)
                {
                    if (o.layer == LayerMask.NameToLayer("Monster"))
                    {
                        Vector3 vec = o.transform.position - mGameObj.transform.position;
                        float _sqrMag = vec.sqrMagnitude;

                        if (nearestGO == null || _sqrMag < lastSqrMag)
                        {
                            nearestGO = o;
                            lastSqrMag = _sqrMag;
                        }
                    }

                }
            }
        }

        return nearestGO;
    }
    public void OnPadBtnPressing()
    {
        isAtkBtnPressing = true;

        mNearestGO = GetNearestMonster();

        if (mNearestGO != null)
        {
            mController.orientationVec = mNearestGO.gameObject.transform.position - mGameObj.transform.position;
            mController.orientationVec.y = 0;
            mController.manulOrientation = true;
        }
        

        
        ActorFire();

        mController.forceWalking = true;
    }
    public void OnPadBtnUp()
    {
        isAtkBtnPressing = false;

        ActorController _act = mController;
        _act.forceWalking = false;

       // mController.moveDirection = mController.targetDirection = mController.orientationVec;
        mController.manulOrientation = false;
        mNearestGO = null;

    }

    public void OnAttackPadMovePressing(Vector2 axis)
    {
        float v = axis.y;
        float h = axis.x;
        Transform cameraTransform = Camera.main.transform;
        // Forward vector relative to the camera along the x-z plane	
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        
        Vector3 targetDirection = h * right + v * forward;

        mController.orientationVec = targetDirection;
        mController.orientationVec.y = 0;

        mController.manulOrientation = true;




        isAtkJoyPressing = true;
        ActorFire();
        mController.forceWalking = true;

    }
    public void OnAttackPadMoveUp()
    {
        //mController.moveDirection = mController.targetDirection = mController.orientationVec;
        mController.manulOrientation = false;

        isAtkJoyPressing = false;
        mController.forceWalking = false;

    }
    public void SwitchAtkMode(bool autoAtkMode)
    {
        isAutoAtkMode = autoAtkMode;
    }
}
