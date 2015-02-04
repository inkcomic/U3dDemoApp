using UnityEngine;
using System.Collections;

public class AttackModeSwitcher : MonoBehaviour {

    bool isAutoAtkMode = true;

    public GameObject mFireBtn=null;
    public GameObject mFirePad = null;
	// Use this for initialization
	void Start () {

        EasyButton.On_ButtonDown += On_ButtonDown;

        Switcher();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Switcher()
    {
        isAutoAtkMode = !isAutoAtkMode;

        if (mFireBtn && mFirePad)
        {
            mFireBtn.SetActive(false);
            mFirePad.SetActive(false);

            if (isAutoAtkMode)
            {
                mFireBtn.SetActive(true);
            }
            else
            {
                mFirePad.SetActive(true);
            }

            PlayerMgr player = LevelMgr.inst.GetPlayer() as PlayerMgr;
            if (player!=null)
            {
                player.SwitchAtkMode(isAutoAtkMode);
            }
        }
    }

    void On_ButtonDown(string buttonName)
    {
        if (buttonName == "SwitcherButton")
        {
            Switcher();
        }
    }
}
