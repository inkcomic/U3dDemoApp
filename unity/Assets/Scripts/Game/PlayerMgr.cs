using UnityEngine;
using System.Collections;

public class PlayerMgr : ActorMgr{

	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();

        UpdateInput();
	}


    void UpdateInput()
    {
        ActorMgr actMgr = LevelMgr.inst.GetPlayer();
        if (actMgr != null)
        {
            ActorController _act = actMgr.mController;

            Transform cameraTransform = Camera.main.transform;
            // Forward vector relative to the camera along the x-z plane	
            Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;

            // Right vector relative to the camera
            // Always orthogonal to the forward vector
            Vector3 right = new Vector3(forward.z, 0, -forward.x);

            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            // Are we moving backwards or looking backwards
            if (v < -0.2f)
                _act.movingBack = true;
            else
                _act.movingBack = false;

            bool wasMoving = _act.isMoving;
            _act.isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;

            // Target direction relative to the camera
            Vector3 targetDirection = h * right + v * forward;

            _act.targetDirection = targetDirection;
        }

        if (Input.GetKey(KeyCode.C))
        {
            ActorFire();
        }
    }
}
