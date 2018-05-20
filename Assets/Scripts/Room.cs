using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {


    [SerializeField]
    List<Transform> alert_marking_objects;

    List<Vector2> _AlertPoints = new List<Vector2>();

    [SerializeField]
    List<Transform> patrol_1_marking_objects;

    List<Vector2> _Patrol_1 = new List<Vector2>();

    [SerializeField]
    List<Transform> patrol_2_marking_objects;

    List<Vector2> _Patrol_2 = new List<Vector2>();

    [SerializeField]
    List<Transform> patrol_3_marking_objects;

    List<Vector2> _Patrol_3 = new List<Vector2>();

    // Use this for initialization
    void Awake () {
        for (int i = 0; i < alert_marking_objects.Count; i++) {
            _AlertPoints.Add(new Vector2(alert_marking_objects[i].position.x, alert_marking_objects[i].position.y));
        }
        for (int i = 0; i < patrol_1_marking_objects.Count; i++)
        {
            _Patrol_1.Add(new Vector2(patrol_1_marking_objects[i].position.x, patrol_1_marking_objects[i].position.y ));
        }
        for (int i = 0; i < patrol_2_marking_objects.Count; i++)
        {
            _Patrol_2.Add( new Vector2(patrol_2_marking_objects[i].position.x, patrol_2_marking_objects[i].position.y ));
        }
        for (int i = 0; i < patrol_3_marking_objects.Count; i++)
        {
            _Patrol_3.Add(new Vector2(patrol_3_marking_objects[i].position.x , patrol_3_marking_objects[i].position.y));
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<Vector2> getPatrol(int num) {

        List<Vector2> L = new List<Vector2>();

        if (num == 1) { L = _Patrol_1; }
        if (num == 2) { L = _Patrol_2; }
        if (num == 3) { L = _Patrol_3; }



        return L;
    }

    public List<Vector2> getAlertPoints() {
        return _AlertPoints;
    }

}
