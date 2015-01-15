using UnityEngine;
using System.Collections;



public class Controller : MonoBehaviour {
	
	public const int HERO_UP= 0;
	public const int HERO_RIGHT= 1;
	public const int HERO_DOWN= 2;
	public const int HERO_LEFT= 3;
	
	public int state = 0;
	public int backState = 0;

	public MPJoystick moveJoystick;  
	public void Awake() {
		
	}
	
	
	void Start () {
		state = HERO_DOWN;
	}
	
	
	void Update () {
		
    float touchKey_x =  moveJoystick.position.x;  
    float touchKey_y =  moveJoystick.position.y;  
		
      
     
    if(touchKey_x == -1){  
       setHeroState(HERO_LEFT);
          
    }else if(touchKey_x == 1){  
       setHeroState(HERO_RIGHT);
          
    }  
     
    if(touchKey_y == -1){  
        setHeroState(HERO_DOWN);
 
    }else if(touchKey_y == 1){  
    	setHeroState(HERO_UP);         
    }  
	
	if(touchKey_x == 0 && touchKey_y ==0){
		animation.Play();
	}
	
		
	}
	
	public void setHeroState(int newState)
	{
		
		
		int rotateValue = (newState - state) * 90;
		Vector3 transformValue = new Vector3();
		
		
		animation.Play("Run");

        bool bChangeface = false;
		switch(newState){
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
