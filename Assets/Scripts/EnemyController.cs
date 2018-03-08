using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    int sum = 0;
    Astar myplanner;
    // Use this for initialization
    void Start () {
        myplanner = GetComponent<Astar>();
	}
	
	// Update is called once per frame
	void Update () {
        if (sum == 0) {
            sum++;
            List<Vector2> plan = myplanner.planToPosition(transform.position, new Vector2(0, 0));
            Debug.Log("Plan size" + plan.Count);
            plan.ForEach(x => Debug.Log(x));
            
        }
        
    }
}
