using UnityEngine;
using System;

public class scriptFun1
{
		public void Update (float delta)
		{
				timer += delta;
				if (timer > 1.0f)
						speed = 180.0f * delta;
				else
						speed = 60.0f * delta;
				if (timer > 2.0f) {
						timer -= 2.0f;
				}
             //   Debug.Log(delta);
		}

        public string GetSpecialName()
        {
            return "Hahah是我";
        }
		public float speed = 0;
		public float timer = 0;
}


