using UnityEngine;
using System.Collections;

public class GameActorStatus : MonoBehaviour
{


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public uint nHP = 100;
    public uint nMaxHP = 100;

    public ActorType actorType;

    [HideInInspector]
    public GameObject currentWeapon = null;
    [HideInInspector]
    public ActorMgr myMgr = null;
}
