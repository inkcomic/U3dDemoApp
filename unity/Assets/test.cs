using UnityEngine;
using System.Collections;
using PathologicalGames;
public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int count = 10;// this.spawnAmount;
        Transform inst;
        SpawnPool shapesPool = PoolManager.Pools["bulletpool"];
        //while (count > 0)
        //{
            // Spawn in a line, just for fun
            inst = shapesPool.Spawn(this.transform);
            inst.localPosition = new Vector3((10 + 2) - count, 0, 0);
            count--;

            //  yield return new WaitForSeconds(1);
        //}

        //  this.StartCoroutine(Despawner());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   
}
