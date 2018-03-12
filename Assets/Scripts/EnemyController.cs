using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TO DO: patrullas - condicion de carrera del metodo Start
// poda A*
// completar persecucion: el enemigo conoce la ubicacion y planifica aunque te muevas
// Vision / oido
// estado alerta (??)

public class EnemyController : MonoBehaviour {
    int sum = 0;
    Astar myplanner;
    bool _moving;
    List<Vector2> _plan;
    int _current_step;

    // Use this for initialization
    void Start () {
        myplanner = GetComponent<Astar>();
	}
	
	// Update is called once per frame
	void Update () {
        if (sum == 0) {
            sum++;
            _plan = myplanner.planToPosition(transform.position, new Vector2(5, 2));
            Debug.Log("Plan size" + _plan.Count);
            _plan.ForEach(x => Debug.Log(x));
            _moving = true;
            _current_step = 0;
            
        }
        if (_moving) {
            Vector3 direction = ( new Vector3(_plan[_current_step].x, _plan[_current_step].y) - transform.position);
            transform.position += direction.normalized * Time.deltaTime;

            if (Vector3.Distance(new Vector3(_plan[_current_step].x, _plan[_current_step].y), transform.position) < 0.3f)
            {
                _current_step++;
                if (_current_step >= _plan.Count)
                {
                    _moving = false;
                    _current_step = 0;
                }
            }
        }
        
    }
}
