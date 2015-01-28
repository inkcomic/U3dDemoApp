using UnityEngine;
using System.Collections;

/**
 *  @Author : www.xuanyusong.com 
 */

[RequireComponent(typeof(CharacterController))]

public class ActorController : MonoBehaviour
{

    public AnimationClip idleAnimation ;
    public AnimationClip walkAnimation ;
    public AnimationClip runAnimation ;
    public AnimationClip jumpPoseAnimation;
    public AnimationClip attack_meleeAnimation;

    public float walkMaxAnimationSpeed  = 0.75f;
    public float trotMaxAnimationSpeed  = 1.0f;
    public float runMaxAnimationSpeed  = 1.0f;
    public float jumpAnimationSpeed  = 1.15f;
    public float landAnimationSpeed  = 1.0f;
    public float meleAttackAnimationSpeed = 1.0f;
    

    private Animation _animation;

    public enum CharacterState 
    {
	    Idle = 0,
	    Walking = 1,
	    Trotting = 2,
	    Running = 3,
	    Jumping = 4,
        AttackMelee = 5,
        AttackRange = 6,
    }

    private CharacterState _characterState;

    // The speed when walking
    float walkSpeed = 2.0f;
    // after trotAfterSeconds of walking we trot with trotSpeed
    float trotSpeed = 4.0f;
    // when pressing "Fire3" button (cmd) we start running
    float runSpeed = 6.0f;

    float inAirControlAcceleration = 3.0f;

    // How high do we jump when pressing jump and letting go immediately
    float jumpHeight = 1.2f;//0.5f;

    // The gravity for the character
    float gravity = 20.0f;
    // The gravity in controlled descent mode
    float speedSmoothing = 10.0f;
    float rotateSpeed = 500.0f;
    float trotAfterSeconds = 3.0f;

    bool canJump = true;

    private float jumpRepeatTime = 0.05f;
    private float jumpTimeout = 0.15f;
    private float groundedTimeout = 0.25f;

    // The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
    private float lockCameraTimer = 0.0f;

    // The current move direction in x-z
    private Vector3 moveDirection = Vector3.zero;
    // The current vertical speed
    private float verticalSpeed = 0.0f;
    // The current x-z move speed
    private float moveSpeed = 0.0f;

    // The last collision flags returned from controller.Move
    private CollisionFlags collisionFlags; 

    // Are we jumping? (Initiated with jump button and not grounded yet)
    private bool jumping = false;
    private bool jumpingReachedApex = false;

    [HideInInspector]
    // Are we moving backwards (This locks the camera to not do a 180 degree spin)
    public  bool movingBack = false;
    [HideInInspector]
    // Is the user pressing any keys?
    public bool isMoving = false;
    // When did the user start walking (Used for going into trot after a while)
    private float walkTimeStart = 0.0f;
    // Last time the jump button was clicked down
    private float lastJumpButtonTime = -10.0f;
    // Last time we performed a jump
    private float lastJumpTime = -1.0f;

    // the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
    private float lastJumpStartHeight = 0.0f;

    private Vector3 inAirVelocity = Vector3.zero;

    private float lastGroundedTime = 0.0f;

    private bool isControllable = true;

    private bool isMeleeAttacking = false;

    public Vector3 targetDirection;

    public bool needRun = false;

    public Vector3 orientationVec = new Vector3(0,0,0);
    public bool manulOrientation = false;

    public bool forceWalking = false;
    ///
    //event delegate
    public delegate void AnimDoneDelegate(CharacterState state);
    public AnimDoneDelegate animDoneDelegate = null;

    public delegate void CollisionEnterDelegate(Collision col);
    public CollisionEnterDelegate collisionEnterDelegate = null;
    void Awake()
    {
        moveDirection = transform.TransformDirection(Vector3.forward);

        _animation = GetComponentInChildren<Animation>();
        if (!_animation)
            Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");

        /*
    public var idleAnimation : AnimationClip;
    public var walkAnimation : AnimationClip;
    public var runAnimation : AnimationClip;
    public var jumpPoseAnimation : AnimationClip;	
        */
        if (!idleAnimation)
        {
            _animation = null;
            Debug.Log("No idle animation found. Turning off animations.");
        }
        if (!walkAnimation)
        {
            _animation = null;
            Debug.Log("No walk animation found. Turning off animations.");
        }
        if (!runAnimation)
        {
            _animation = null;
            Debug.Log("No run animation found. Turning off animations.");
        }
        if (!jumpPoseAnimation && canJump)
        {
            _animation = null;
            Debug.Log("No jump animation found and the character has canJump enabled. Turning off animations.");
        }

        if (!attack_meleeAnimation)
        {
            _animation = null;
            Debug.Log("No attack animation found. Turning off animations.");
        }
//         else
//         {
//             AnimationEvent startEvent = new AnimationEvent();
//             startEvent.time = attack_meleeAnimation.length;
//             startEvent.functionName = "OnAttack0CallBack";
//             _animation.GetClip(attack_meleeAnimation.name).AddEvent(startEvent);
//         }

    }

    void UpdateSmoothedMovementDirection ()
    {
 	    bool grounded = IsGrounded();
        {
            // Grounded controls
            if (grounded)
            {
                // Lock camera for short period when transitioning moving & standing still
                lockCameraTimer += Time.deltaTime;
//                  if (isMoving != wasMoving)
//                      lockCameraTimer = 0.0f;

                // We store speed and direction seperately,
                // so that when the character stands still we still have a valid forward direction
                // moveDirection is always normalized, and we only update it if there is user input.
                if (targetDirection != Vector3.zero)
                {
                    // If we are really slow, just snap to the target direction
                    if (moveSpeed < walkSpeed * 0.9f && grounded)
                    {
                        moveDirection = targetDirection.normalized;
                    }
                    // Otherwise smoothly turn towards it
                    else
                    {
                        moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);

                        moveDirection = moveDirection.normalized;
                    }
                }

                // Smooth the speed based on the current target direction
                float curSmooth = speedSmoothing * Time.deltaTime;

                // Choose target speed
                //* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
                float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

                _characterState = CharacterState.Idle;

                if (isMeleeAttacking )
                {
                    _characterState = CharacterState.AttackMelee;
                    targetSpeed *= runSpeed;
                }
                else if (needRun/*Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift)*/ | isMoving)
                {
                    targetSpeed *= runSpeed;
                    _characterState = CharacterState.Running;
                }
                else if (Time.time - trotAfterSeconds > walkTimeStart)
                {
                    targetSpeed *= trotSpeed;
                    _characterState = CharacterState.Trotting;
                }
                else
                {
                    targetSpeed *= walkSpeed;
                    _characterState = CharacterState.Walking;
                }

                if (forceWalking)
                {
                    targetSpeed /= 2;
                }
                moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);

                // Reset walk time start when we slow down
                if (moveSpeed < walkSpeed * 0.3f)
                    walkTimeStart = Time.time;
            }
            // In air controls
            else
            {
                // Lock camera while in air
                if (jumping)
                    lockCameraTimer = 0.0f;

                if (isMoving)
                    inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
            }

        }
	
    }

    void ApplyJumping ()
    {
	    // Prevent jumping too fast after each other
	    if (lastJumpTime + jumpRepeatTime > Time.time)
		    return;

	    if (IsGrounded()) {
		    // Jump
		    // - Only when pressing the button down
		    // - With a timeout so you can press the button slightly before landing		
		    if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) {
			    verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
			   // SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
                DidJump();
		    }
	    }
    }

    void ApplyGravity ()
    {
	    if (isControllable)	// don't move player at all if not controllable.
	    {
		    // Apply gravity
		    bool jumpButton = Input.GetButton("Jump");

		    // When we reach the apex of the jump we send out a message
		    if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
		    {
			    jumpingReachedApex = true;
			    SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
		    }

		    if (IsGrounded ())
			    verticalSpeed = 0.0f;
		    else
			    verticalSpeed -= gravity * Time.deltaTime;
	    }
    }

    float CalculateJumpVerticalSpeed (float targetJumpHeight)
    {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
	    return Mathf.Sqrt(2 * targetJumpHeight * gravity);
    }

    void  DidJump ()
    {
	    jumping = true;
	    jumpingReachedApex = false;
	    lastJumpTime = Time.time;
	    lastJumpStartHeight = transform.position.y;
	    lastJumpButtonTime = -10;

	    _characterState = CharacterState.Jumping;
    }

    public void DidMeleeAttack()
    {
        isMeleeAttacking = true;
        _characterState = CharacterState.AttackMelee;
    }

    void DidMeleeAttackDone()
    {
        isMeleeAttacking = false;
        _characterState = CharacterState.Idle;

        if (animDoneDelegate != null) 
            animDoneDelegate(_characterState);
    }
    void  Update() {

	    if (!isControllable)
	    {
		    // kill all inputs if not controllable.
		    Input.ResetInputAxes();
	    }

        if (Input.GetButtonDown("Jump"))
        {
            lastJumpButtonTime = Time.time;
        }

        if (isMeleeAttacking)
        {
            //reset anim
            if(_animation[attack_meleeAnimation.name].time>=_animation[attack_meleeAnimation.name].length)
            {
                _animation[attack_meleeAnimation.name].time = 0;
                DidMeleeAttackDone();
            }
        }
        
	    UpdateSmoothedMovementDirection();

	    // Apply gravity
	    // - extra power jump modifies gravity
	    // - controlledDescent mode modifies gravity
	    ApplyGravity ();

	    // Apply jumping logic
	    ApplyJumping ();

	    if(IsMoving())
	    {
		    //var newPos = transform.position + (transform.rotation * Vector3.forward * moveSpeed);
            var newPos = transform.position + (moveDirection.normalized *  moveSpeed);

            var heropos = transform.position;

            if (Terrain.activeTerrain)
            {
                newPos.y = Terrain.activeTerrain.SampleHeight(newPos);
                heropos.y = Terrain.activeTerrain.SampleHeight(transform.position); 
            }

		
		    Debug.DrawLine(heropos,newPos,Color.red);
	
		    var c = moveSpeed;
		    var b = newPos.y - heropos.y;
		    if(b > 0)
		    {
			    var a = Mathf.Sqrt(Mathf.Pow(c,2) - Mathf.Pow(b,2));
			    moveSpeed = a;
		    }
	    }	
	
		
	    // Calculate actual motion
	    Vector3 movement = moveDirection * moveSpeed + new Vector3 (0, verticalSpeed, 0) + inAirVelocity;
	    movement *= Time.deltaTime;

	    // Move the controller
	    CharacterController controller = GetComponent<CharacterController>();
	    collisionFlags = controller.Move(movement);

	    // ANIMATION sector
	    if(_animation) {
            if (_characterState == CharacterState.AttackMelee)
            {
                _animation[attack_meleeAnimation.name].speed = meleAttackAnimationSpeed;
                _animation.CrossFade(attack_meleeAnimation.name);
            }
		    else if(_characterState == CharacterState.Jumping) 
		    {
			    if(!jumpingReachedApex) {
				    _animation[jumpPoseAnimation.name].speed = jumpAnimationSpeed;
				    _animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
				    _animation.CrossFade(jumpPoseAnimation.name);
			    } else {
				    _animation[jumpPoseAnimation.name].speed = -landAnimationSpeed;
				    _animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
				    _animation.CrossFade(jumpPoseAnimation.name);				
			    }
		    } 
		    else 
		    {
			    if(controller.velocity.sqrMagnitude < 0.1f) {
				    _animation.CrossFade(idleAnimation.name);
			    }
			    else 
			    {
				    if(_characterState == CharacterState.Running) {
					    _animation[runAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, runMaxAnimationSpeed);
					    _animation.CrossFade(runAnimation.name);	
				    }
				    else if(_characterState == CharacterState.Trotting) {
					    _animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, trotMaxAnimationSpeed);
					    _animation.CrossFade(walkAnimation.name);	
				    }
				    else if(_characterState == CharacterState.Walking) {
					    _animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
					    _animation.CrossFade(walkAnimation.name);	
				    }

			    }
		    }
	    }
	    // ANIMATION sector

        if (orientationVec==Vector3.zero)
        {
            orientationVec = moveDirection;
        }

	    // Set rotation to the move direction
	    //if (IsGrounded())
	    {
            if (!manulOrientation)
                orientationVec = moveDirection;

            transform.rotation = Quaternion.LookRotation(orientationVec);

	    }	
// 	    else
// 	    {
// 		    Vector3 xzMove = movement;
// 		    xzMove.y = 0;
// 		    if (xzMove.sqrMagnitude > 0.001f)
// 		    {
// 			    transform.rotation = Quaternion.LookRotation(xzMove);
// 		    }
// 	    }	

	    // We are in jump mode but just became grounded
	    if (IsGrounded())
	    {
		    lastGroundedTime = Time.time;
		    inAirVelocity = Vector3.zero;
		    if (jumping)
		    {
			    jumping = false;
			    SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
		    }
	    }
    }

    void  OnControllerColliderHit (ControllerColliderHit hit )
    {
    //	Debug.DrawRay(hit.point, hit.normal);
	    if (hit.moveDirection.y > 0.01f) 
		    return;
    }

    float GetSpeed () {
	    return moveSpeed;
    }

    public bool IsJumping () {
	    return jumping;
    }

    bool IsGrounded () {
	    return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

    Vector3 GetDirection () {
	    return moveDirection;
    }

    public bool IsMovingBackwards () {
	    return movingBack;
    }

    public float GetLockCameraTimer () 
    {
	    return lockCameraTimer;
    }

    bool IsMoving ()
    {
	    return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
    }

    bool HasJumpReachedApex ()
    {
	    return jumpingReachedApex;
    }

    bool IsGroundedWithTimeout ()
    {
	    return lastGroundedTime + groundedTimeout > Time.time;
    }

    void Reset ()
    {
	    gameObject.tag = "Player";
    }

    void OnCollisionEnter (Collision collision)
    {
        if (collisionEnterDelegate != null)
        {
            collisionEnterDelegate(collision);
        }
        

    }
}