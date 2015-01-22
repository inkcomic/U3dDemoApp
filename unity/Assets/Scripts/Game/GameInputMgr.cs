using UnityEngine;
using System.Collections;

public class GameInputMgr : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        {
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

            // Target direction relative to the camera
            Vector3 targetDirection = h * right + v * forward;

            LevelMgr.inst.GetPlayer().mController.targetDirection = targetDirection;


        }
	}
}
