using UnityEngine;
using System.Collections;

public class FakePhysic : MonoBehaviour {
    bool mIsSleep = false;
    Vector3 mTargetDirection = Vector3.zero;
    float mMoveSpeed = 0.0f;

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
        if (!mIsSleep)
        {
            if (mMoveSpeed>0.0f)
            {
                transform.position += mTargetDirection * mMoveSpeed;
                transform.rotation.SetLookRotation(mTargetDirection);
            }
            if (mIsGraviry)
                transform.position += GlobalDefine.FakeGravity;

            UpdateDropGround();

        }
	}
    public void SetSleep()
    {
        mIsSleep = true;
    }
    void UpdateDropGround()
    {
        BoxCollider cder = this.GetComponent<BoxCollider>();
        if (cder)
        {
            float fHalf = cder.size.y * 0.5f;

            RaycastHit hitInfo;
            bool findObj = Physics.Raycast(transform.position, GlobalDefine.FakeGravity, out hitInfo, fHalf);

            if (findObj)
            {
                transform.position = hitInfo.point + Vector3.up * fHalf;
                SetSleep();
            }
        }

    }

    public void SetInitVelocity(Vector3 targetDirection,float moveSpeed)
    {
        mTargetDirection = targetDirection;
        mMoveSpeed = moveSpeed;

        mIsSleep = false;
    }
}
