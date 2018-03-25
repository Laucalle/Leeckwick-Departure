using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    [SerializeField]
    List<Vector2> _AlertPoints;

    [SerializeField]
    List<Vector2> _Patrol_1;

    [SerializeField]
    List<Vector2> _Patrol_2;

    [SerializeField]
    List<Vector2> _Patrol_3;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public List<Vector2> getPatrol(int num) {
        if (num == 1) { return _Patrol_1; }
        if (num == 2) { return _Patrol_2; }
        if (num == 3) { return _Patrol_3; }
        return new List<Vector2>();
    }
    public List<Vector2> getAlertPoints() {
        return _AlertPoints;
    }
}
