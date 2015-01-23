﻿using UnityEngine;
using System.Collections;

public class PlayerMgr : ActorMgr{

    bool isAutoAtkMode = true;
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
        ActorMgr actMgr = LevelMgr.inst.GetPlayer();
        if (actMgr != null&&actMgr.mController!=null)
        {
            ActorController _act = actMgr.mController;

            Transform cameraTransform = Camera.main.transform;
            // Forward vector relative to the camera along the x-z plane	
            Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;

            // Right vector relative to the camera
            // Always orthogonal to the forward vector
            Vector3 right = new Vector3(forward.z, 0, -forward.x);

            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            // Are we moving backwards or looking backwards
            if (v < -0.2f)
                _act.movingBack = true;
            else
                _act.movingBack = false;

            bool wasMoving = _act.isMoving;
            _act.isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;

            // Target direction relative to the camera
            Vector3 targetDirection = h * right + v * forward;

            _act.targetDirection = targetDirection;

            if (isAutoAtkMode)
            {
                GameObject nearestGO = null;
                float lastSqrMag=-1;
                foreach(var o in LevelMgr.inst.dictLevelActor)
                {
                    Vector3 vec = o.Key.transform.position - mGameObj.transform.position;
                    float _sqrMag = vec.sqrMagnitude;

                    if (nearestGO == null || _sqrMag < lastSqrMag)
                    {
                        nearestGO = o.Key;
                        lastSqrMag = _sqrMag;
                    }
                }

                if (nearestGO!=null)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        _act.orientationVec = nearestGO.gameObject.transform.position- mGameObj.transform.position;
                        _act.manulOrientation = true;

                        ActorFire();
                    }
                    else
                    {
                        _act.manulOrientation = false;
                    }
                }
            }
            else
             //build orientation pad
            {
                float v1 = 0;
                float h1 = 0;

                if (Input.GetKey(KeyCode.J))
                {
                    h1 = -1;
                }
                else if (Input.GetKey(KeyCode.L))
                {
                    h1 = 1;
                }

                if (Input.GetKey(KeyCode.I))
                {
                    v1 = 1;
                }
                else if (Input.GetKey(KeyCode.K))
                {
                    v1 = -1;
                }

                if (h1 != 0 || v1!=0)
                {
                    // Target direction relative to the camera
                    Vector3 orientation = h1 * right + v1 * forward;

                    _act.orientationVec = orientation;
                    _act.manulOrientation = true;
                }
                else
                {
                    _act.manulOrientation = false;
                }
            }
        }
    }
}
