using UnityEngine;
using System.Collections;

public class PlayerInputManager : MonoBehaviour {

    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
        
        EasyButton.On_ButtonPress += On_ButtonPress;
        EasyButton.On_ButtonUp += On_ButtonUp;
        //EasyButton.On_ButtonDown += On_ButtonDown;
    }

    void Fire()
    {
        //if (buttonName=="Fire"){
        //Instantiate(bullet, gun.transform.position, gun.rotation);
        //}		
    }


    void OnDisable()
    {
        EasyJoystick.On_JoystickMove -= On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
        //		EasyButton.On_ButtonPress -= On_ButtonPress;
        EasyButton.On_ButtonUp -= On_ButtonUp;
    }

    void OnDestroy()
    {
        EasyJoystick.On_JoystickMove -= On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
        //		EasyButton.On_ButtonPress -= On_ButtonPress;
        EasyButton.On_ButtonUp -= On_ButtonUp;
    }

    void Start()
    {
        //model = transform.FindChild("Model").transform;
        //gun = transform.FindChild("Gun").transform;
    }

    void On_JoystickMove(MovingJoystick move)
    {
        if(move.joystickName=="MainJoystick")
        {
            PlayerMgr act = LevelMgr.inst.GetPlayer() as PlayerMgr;
            act.OnPadMove(move.joystickAxis);
        }
        else
        {
            PlayerMgr act = LevelMgr.inst.GetPlayer() as PlayerMgr;
            act.OnAttackPadMovePressing(move.joystickAxis);
        }
        

    }

    void On_JoystickMoveEnd(MovingJoystick move)
    {
        if (move.joystickName == "MainJoystick")
        {
            PlayerMgr act = LevelMgr.inst.GetPlayer() as PlayerMgr;
            act.OnPadMove(move.joystickAxis);
        }
        else
        {
            PlayerMgr act = LevelMgr.inst.GetPlayer() as PlayerMgr;
            act.OnAttackPadMoveUp();
        }
       
    }

    
    void On_ButtonPress (string buttonName)
    {
        if (buttonName == "FireButton")
        {
            PlayerMgr act = LevelMgr.inst.GetPlayer() as PlayerMgr;
            act.OnPadBtnPressing();
        }
    }

    void On_ButtonUp(string buttonName)
    {
        if (buttonName == "FireButton")
        {
            PlayerMgr act = LevelMgr.inst.GetPlayer() as PlayerMgr;
            act.OnPadBtnUp();
        }
    }	
}



