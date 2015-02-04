using UnityEngine;
using System.Collections;

public class FakePhysic : MonoBehaviour {
    bool mIsSleep = false;
    Vector3 mTargetVelocity = Vector3.zero;
   
    public bool mIsGraviry=true;
    public bool IsUseable()
    {
        return mIsSleep;
    }
	// Use this for initialization
    public virtual void Start()
    {
        
	}
	
	// Update is called once per frame
	public virtual void Update () {
        
	}
    public virtual void FixedUpdate()
    {
        if (!mIsSleep)
        {

            if (mIsGraviry)
            {
                mTargetVelocity += GlobalDefine.FakeGravity * Time.fixedDeltaTime;
            }
            if (mTargetVelocity.magnitude > 0.0f)
            {
                transform.rotation.SetLookRotation(mTargetVelocity.normalized);
                transform.position += mTargetVelocity * Time.fixedDeltaTime;
            }

            UpdateDropGround();

        }
    }
    public void SetSleep()
    {
        mIsSleep = true;

        mTargetVelocity = Vector3.zero;
    }
    void UpdateDropGround()
    {
        BoxCollider cder = this.GetComponent<BoxCollider>();
        if (cder)
        {
            float fHalf = cder.size.y * 0.5f;

            RaycastHit hitInfo;

            Vector3 nextS = GlobalDefine.FakeGravity * Time.fixedDeltaTime * Time.fixedDeltaTime * 100.0f;

            LayerMask mask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Monster");
            mask = ~mask;
            bool findObj = Physics.Raycast(transform.position, nextS, out hitInfo, nextS.magnitude, mask);

            if (findObj)
            {
                transform.position = hitInfo.point + Vector3.up * fHalf;
                SetSleep();
            }
        }

    }

    public void SetInitVelocity(Vector3 targetDirection,float moveSpeed)
    {
        mTargetVelocity = targetDirection * moveSpeed;
        mIsSleep = false;
    }
}
