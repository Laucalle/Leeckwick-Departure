﻿using System.Collections;
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
        for (int i = 0; i < _AlertPoints.Count; i++) {
            _AlertPoints[i] = new Vector2(_AlertPoints[i].x + transform.position.x, _AlertPoints[i].y + transform.position.y);
        }
        for (int i = 0; i < _Patrol_1.Count; i++)
        {
            _Patrol_1[i] = new Vector2(_Patrol_1[i].x + transform.position.x, _Patrol_1[i].y + transform.position.y);
        }
        for (int i = 0; i < _Patrol_2.Count; i++)
        {
            _Patrol_2[i] = new Vector2(_Patrol_2[i].x + transform.position.x, _Patrol_2[i].y + transform.position.y);
        }
        for (int i = 0; i < _Patrol_3.Count; i++)
        {
            _Patrol_3[i] = new Vector2(_Patrol_3[i].x + transform.position.x, _Patrol_3[i].y + transform.position.y);
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
