using UnityEngine;
using System.Collections;

public class ActorController : MonoBehaviour
{

    public const int HERO_UP = 0;
    public const int HERO_RIGHT = 1;
    public const int HERO_DOWN = 2;
    public const int HERO_LEFT = 3;

    public int state = 0;
    public int backState = 0;


    public const string ANIM_NAME_IDLE = "Idle";
    public const string ANIM_NAME_RUN = "Run";
    public const string ANIM_NAME_JUMP = "jump";
    public const string ANIM_NAME_ATTACK_00 = "Attack00";

    public class AnimState
    {
        public Vector2 position = new Vector2(0,0);
        public bool isJumping = false;
        public bool isAttack00 = false;

    };
    AnimState anim_state = new AnimState();
    public MPJoystick moveJoystick;
    public void Awake()
    {

    }


    void Start()
    {
        state = HERO_DOWN;
    }

    void ComputeAnimLogic()
    {
        anim_state.position = moveJoystick.position;

        if (anim_state.isJumping)
        {

            
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(gameObject.animation.PlayWithOptions(
                ANIM_NAME_JUMP,
                () =>
                {
                    Debug.Log("ANIMATION FINISHED, LOADING SCENE X");
                    anim_state.isJumping = false;
                }
                ));
                anim_state.isJumping = true;
//                 // if (!animation.GetCurrentAnimatorStateInfo(0);("ANIM_NAME_JUMP"))
//                 {
//                     anim_state.isJumping = false;
//                 }
            }
        }
       

        if (Input.GetMouseButtonDown(0))
        {
            anim_state.isAttack00 = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            anim_state.isAttack00 = false;
        }
    }
    void Update()
    {
        //需要在其他地方计算好
        ComputeAnimLogic();


        float touchKey_x = anim_state.position.x;
        float touchKey_y = anim_state.position.y;



        if (touchKey_x == -1)
        {
            setMoveState(HERO_LEFT);

        }
        else if (touchKey_x == 1)
        {
            setMoveState(HERO_RIGHT);

        }

        if (touchKey_y == -1)
        {
            setMoveState(HERO_DOWN);

        }
        else if (touchKey_y == 1)
        {
            setMoveState(HERO_UP);
        }

        if (anim_state.isAttack00)
        {
            animation.CrossFade(ANIM_NAME_ATTACK_00);
        }
        else if (anim_state.isJumping)
        {
            //animation.CrossFade(ANIM_NAME_JUMP);

        }
        else if ((touchKey_x == 1) || (touchKey_x == -1) || (touchKey_y == 1) || (touchKey_y == -1))
        {
            animation.CrossFade(ANIM_NAME_RUN);
        }
        else if (touchKey_x == 0 && touchKey_y == 0)
        {
            animation.CrossFade(ANIM_NAME_IDLE);
        }


    }

    public void setMoveState(int newState)
    {


        int rotateValue = (newState - state) * 90;
        Vector3 transformValue = new Vector3();


        

        bool bChangeface = false;
        switch (newState)
        {
            case HERO_UP:
                //transformValue = Vector3.forward * Time.deltaTime;
                transformValue = Vector3.up * Time.deltaTime;
                rotateValue = 0;

                break;
            case HERO_DOWN:
                //transformValue = -Vector3.forward * Time.deltaTime;
                transformValue = -Vector3.up * Time.deltaTime;
                rotateValue = 0;
                break;
            case HERO_LEFT:
                transformValue = Vector3.left * Time.deltaTime;
                bChangeface = true;
                break;
            case HERO_RIGHT:
                transformValue = -Vector3.left * Time.deltaTime;
                bChangeface = true;
                break;
        }

        transform.Rotate(Vector3.up, rotateValue);

        transform.Translate(transformValue, Space.World);

        backState = state;

        if (bChangeface)
            state = newState;

    }

}
