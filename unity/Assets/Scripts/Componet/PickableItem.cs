using UnityEngine;
using System.Collections;

public class PickableItem : MonoBehaviour {
    bool mIsAttachedGround = false;
    
    public PickItemType mItemType = PickItemType.eAex;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
        if(!mIsAttachedGround)
        {
            UpdateDropGround();
        }
	}

    void UpdateDropGround()
    {
        BoxCollider cder = this.GetComponent<BoxCollider>();
        if (cder)
        {
            float fHalf = cder.size.y *0.5f;

            RaycastHit hitInfo;
            bool findObj = Physics.Raycast(transform.position, Vector3.down,out hitInfo, fHalf);

            if(findObj)
            {
                transform.position = hitInfo.point + Vector3.up * fHalf;
                mIsAttachedGround = true;
            }
            else
            {
                transform.position += Vector3.down * fHalf;
            }

        }
        
    }

}
