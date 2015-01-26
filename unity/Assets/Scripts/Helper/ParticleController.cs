using System;
using UnityEngine;
using System.Collections;


public class ParticleController : MonoBehaviour
{
    ParticleSystem myPS = null;
	public event EventHandler AnimationEndCallBack;
	
	
    void Awake()
    {
        myPS = GetComponent<ParticleSystem>();
    }
	public void Play(bool repeat = false, EventHandler callback = null)
	{
		AnimationEndCallBack += callback;

        myPS.Play();
	}
    void Destroy()
    {
        myPS = null;
    }
    	
	public void Update()
	{

        if (!IsAliveAnimation())
		{
			//this.CreateParticleInstance();
			
			if (this.AnimationEndCallBack != null)
				this.AnimationEndCallBack(this, EventArgs.Empty);
	

			{
				this.AnimationEndCallBack = null;

                ParticleMgr.inst.DestroyParticle(this.transform.parent.gameObject);

               // MyHelper.RemoveChildren(this.gameObject);
				//DestroyImmediate(particle);
				//LParticleManager.SendGameObjectToPool(this.ParticleInfo, this.gameObject);
				
			}
		}
	}
	
	public bool IsAliveAnimation()
	{
        if (myPS)
        {
            if (myPS.IsAlive())
                return true;
            else
                return false;
        }

		return false;
	}
}
