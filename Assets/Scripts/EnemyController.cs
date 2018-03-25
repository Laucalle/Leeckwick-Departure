using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TO DO: patrullas - condicion de carrera del metodo Start
// poda A*
// completar persecucion: el enemigo conoce la ubicacion y planifica aunque te muevas
// Vision / oido
// estado alerta (??)
// LLamar a la planificacion de las patrullas en la pantalla de carga

public class EnemyController : MonoBehaviour {
    Astar myplanner;
    bool _follow, _replan, _patrolling;
    List<Vector2> _plan;
    int _current_step, _currentPatrolPlan;
    GameObject player;
    private Rigidbody2D rb2d;
    List<List<Vector2>> _patrols = null;
    List<Vector2> _patrolPoints = null;
    Room _currentRoom;

    [SerializeField]
    int _roomId;
    [SerializeField]
    Room _myRoom;
    


    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        myplanner = GetComponent<Astar>();
        myplanner.InitVars(transform.localScale.x * GetComponent<BoxCollider2D>().size.x /2);
        player = GameObject.Find("Player");
        _replan = true;
        _follow = false;
        _patrolling = true;
        _currentPatrolPlan = 0;
        _patrolPoints = new List<Vector2>();

    }

    void InitPatrols()
    {
        _patrols = new List<List<Vector2>>();
        for(int i = 0; i < _patrolPoints.Count; i++)
        {

            _patrols.Add(myplanner.planToPosition(_patrolPoints[i], _patrolPoints[(i + 1) % _patrolPoints.Count], 0.5f));

        }

    }

    void PlanPatrol()
    {
        if (_patrolPoints == null || _patrolPoints.Count == 0)
        {
            Debug.Log("Entrando");
            _patrolPoints = _myRoom.getPatrol(_roomId);
            InitPatrols();
        }

        
        _plan = _patrols[_currentPatrolPlan];
        _currentPatrolPlan = (_currentPatrolPlan + 1) % _patrolPoints.Count;

        Debug.Log(_plan.Count);
    }

    void PlanFollow() {
        float distance_to_target = Vector3.Distance(transform.position, player.transform.position);

        //if (distance_to_target < 2.0f) distance_to_target = 0.0f;
        _plan = myplanner.planToPosition(transform.position, player.transform.position, distance_to_target / 2);
        Debug.Log(_plan.Count);
        Debug.Log("New Plan distance" + Vector2.Distance(_plan[_plan.Count - 1], new Vector2(player.transform.position.x, player.transform.position.y)));
        _follow = true;
        _current_step = 0;
    }

    void ExecutePlanStep() {
        
        Vector3 direction = (new Vector3(_plan[_current_step].x, _plan[_current_step].y) - transform.position);
        //rb2d.velocity = direction * 1.0f;
        transform.position += direction.normalized * Time.deltaTime;
        
        if (Vector3.Distance(new Vector3(_plan[_current_step].x, _plan[_current_step].y), transform.position) < transform.localScale.x * GetComponent<BoxCollider2D>().size.x)
        {

            _current_step++;
            if (_current_step >= _plan.Count)
            {
                if(_follow)_plan.Clear();

                _current_step = 0;
                _replan = true;
            }
            else {
                Debug.LogWarning(_current_step + " of " + _plan.Count);
            }
        }
    }
	// Update is called once per frame
	void Update () {

        if (_replan) {
            _replan = false;
            if (_patrolling)
            {
                PlanPatrol();
            }

            if(_follow)
            {
                PlanFollow();
            }
                
        }
    
        ExecutePlanStep();    
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Player") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
            

    }
}
