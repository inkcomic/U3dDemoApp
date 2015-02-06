using UnityEngine;
using System.Collections;

public class LevelObjectInfo : MonoBehaviour
{


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //public uint nHP = 100;
    //public uint nMaxHP = 100;

    public LevelObjectType actorType;

    [HideInInspector]
    public GameObject currentWeapon = null;
    [HideInInspector]
    public LevelObject myLevelObject = null;
}
