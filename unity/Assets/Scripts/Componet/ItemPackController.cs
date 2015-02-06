using UnityEngine;
using System.Collections;

public class ItemPackController : FakePhysic{

    public ItemPackType mItemType = ItemPackType.eAex;
	// Use this for initialization
    public override void Start()
    {
        base.Start();   
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
        
	}
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
   

}
