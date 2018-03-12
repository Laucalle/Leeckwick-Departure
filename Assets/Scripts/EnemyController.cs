using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TO DO: patrullas - condicion de carrera del metodo Start
// poda A*
// completar persecucion: el enemigo conoce la ubicacion y planifica aunque te muevas
// Vision / oido
// estado alerta (??)

public class EnemyController : MonoBehaviour {
    Astar myplanner;
    bool _follow, _replan;
    List<Vector2> _plan;
    int _current_step;
    GameObject player;

    // Use this for initialization
    void Start () {
        myplanner = GetComponent<Astar>();
        myplanner.gen_step = transform.localScale.x * GetComponent<BoxCollider2D>().size.x;
        player = GameObject.Find("Player");
        Debug.Log(myplanner.gen_step);
        _replan = true;
        _follow = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (_replan) {
            _replan = false;
            if(_follow)
            {
                float distance_to_target = Vector3.Distance(transform.position, player.transform.position);

                _plan = myplanner.planToPosition(transform.position, player.transform.position,distance_to_target/2);
                Debug.Log("New Plan distance" + Vector2.Distance(_plan[_plan.Count - 1], new Vector2(player.transform.position.x, player.transform.position.y)));
                _follow = true;
                _current_step = 0;
            }
                
        }
        
        Vector3 direction = ( new Vector3(_plan[_current_step].x, _plan[_current_step].y) - transform.position);
        transform.position += direction.normalized * Time.deltaTime;
        
        if (Vector3.Distance(new Vector3(_plan[_current_step].x, _plan[_current_step].y), transform.position) < transform.localScale.x * GetComponent<BoxCollider2D>().size.x)
        {
            
            _current_step++;
            if (_current_step >= _plan.Count)
            {
                _plan.Clear();
                _current_step = 0;
                _replan = true;
            }
        }
        
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Player") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
            

    }
}
