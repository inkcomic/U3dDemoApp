using UnityEngine;
using System.Collections;

public class AttackModeSwitcher : MonoBehaviour {

    bool isAutoAtkMode = true;

    public GameObject mAtkBtn=null;
    public GameObject mAtkPad = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Switcher()
    {
        isAutoAtkMode = !isAutoAtkMode;

        if (mAtkBtn && mAtkPad)
        {
            mAtkBtn.SetActive(false);
            mAtkPad.SetActive(false);

            if (isAutoAtkMode)
            {
                mAtkBtn.SetActive(true);
            }
            else
            {
                mAtkPad.SetActive(true);
            }

            PlayerMgr player = LevelMgr.inst.GetPlayer() as PlayerMgr;
            if (player!=null)
            {
                player.SwitchAtkMode(isAutoAtkMode);
            }
        }
    }
}
