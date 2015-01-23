using UnityEngine;
using System.Collections;

public class MonsterAI : MonoBehaviour {

    ActorController actor_controller = null;
    // Use this for initialization
	void Start () {
        actor_controller = GetComponent<ActorController>();

	}
	
	// Update is called once per frame
	void Update () {

        if (actor_controller!=null)
        {
            {
                ActorMgr actor = LevelMgr.inst.GetPlayer();
                Vector3 vec = actor.mGameObj.transform.position - transform.position;

                if(vec.magnitude>1)
                {
					vec.y=0;
                    actor_controller.targetDirection=vec;

                    if(vec.magnitude>5)
                    {
                        actor_controller.needRun = true;
                    }
                    else
                    {
                        actor_controller.needRun = false;
                    }
                }
                else
                {
                    actor_controller.targetDirection=Vector3.zero;
                }
            }
        }

        //LevelMgr.inst.GetPlayer().Damage(100);
	}
}
