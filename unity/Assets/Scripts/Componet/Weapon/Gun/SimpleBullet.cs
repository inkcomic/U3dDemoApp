using UnityEngine;
using System.Collections.Generic;



[RequireComponent(typeof(Collider))]
public class SimpleBullet : MonoBehaviour
{
    [HideInInspector]
    public ActorMgr bulletOwner = null;

    public WeaponType shooterType = WeaponType.ePistol;

    [HideInInspector]
    public Vector3 vecDirection;
    [HideInInspector]
    public float vecSpeed = 0.1f;

    [HideInInspector]
    bool isSleep = true;
	void Awake()
    {
        this.gameObject.SetActive(false);
    }
	void Start () {
        
	}
    public void Setup(Vector3 dir,float speed  = 5.0f)
    {
        this.gameObject.SetActive(true);
        isSleep = false;

        vecDirection = dir;
        vecSpeed = speed;
    }
	// Update is called once per frame
	void Update () {
        if (isSleep)
            return;

        switch (shooterType)
        {
            case WeaponType.ePistol:
                {
                    SimulatePistol();
                }
                break;
        }
	}
   
    void OnTriggerEnter(Collider other)
    {
        WeaponHitable hitable = other.GetComponent<WeaponHitable>();
        bool deleteMe = false;
        if (hitable)
            deleteMe = hitable.OnWeaponHit(other, bulletOwner);

        if (deleteMe)
            Disappear();
    }
    void Disappear()
    {
        GameObject.Destroy(this.gameObject);
    }
    void SimulatePistol()
    {
        this.transform.rotation = Quaternion.LookRotation(vecDirection);

        Vector3 movement = vecDirection * vecSpeed;
        movement *= Time.deltaTime;

        this.transform.position += movement;
    }
}
