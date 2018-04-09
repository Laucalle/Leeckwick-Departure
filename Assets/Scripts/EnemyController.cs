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
    Vector3 movDirection;
    float timeLastSighting;
    GameObject _target;


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
        movDirection = new Vector3(0, -1, 0);
        //_patrolPoints = new List<Vector2>();
        _patrolPoints = _myRoom.getPatrol(_roomId);
        for (int i = 0; i < _patrolPoints.Count; i++)
            Debug.DrawLine(_patrolPoints[i], _patrolPoints[i] + new Vector2(0.1f, 0.1f), Color.yellow, 60f);
        InitPatrols();
    }

    void InitPatrols()
    {
        float fat_dot= 0.0F;
        Debug.Log(transform.localScale.x +"*"+ GetComponent<BoxCollider2D>().size.x+"="+transform.localScale.x * GetComponent<BoxCollider2D>().size.x);
        _patrols = new List<List<Vector2>>();
        for(int i = 0; i < _patrolPoints.Count; i++)
        {
            //Debug.Log(myplanner.planToPosition(_patrolPoints[i], _patrolPoints[(i + 1) % _patrolPoints.Count], fat_dot).Count);
            _patrols.Add(myplanner.planToPosition(_patrolPoints[i], _patrolPoints[(i + 1) % _patrolPoints.Count], fat_dot));
            Debug.Log("Planning from "+ _patrolPoints[i] +" to " + _patrolPoints[(i + 1) % _patrolPoints.Count]+ " with " + _patrols[i].Count);

            for(int j = 0; j< _patrols[i].Count - 1; j++)
            {
                Debug.DrawLine(_patrols[i][j], _patrols[i][j + 1], Color.blue, 60f);
            }
        }

        

    }

    void PlanPatrol()
    {
        _plan = _patrols[_currentPatrolPlan];
        _currentPatrolPlan = (_currentPatrolPlan + 1) % _patrolPoints.Count;

        Debug.Log(_plan.Count);
    }

    void PlanFollow() {
        float distance_to_target = Vector3.Distance(transform.position, _target.transform.position);

        //if (distance_to_target < 2.0f) distance_to_target = 0.0f;
        _plan = myplanner.planToPosition(transform.position, _target.transform.position, distance_to_target / 2);
        Debug.Log(_plan.Count);
        Debug.Log("New Plan distance" + Vector2.Distance(_plan[_plan.Count - 1], new Vector2(_target.transform.position.x, _target.transform.position.y)));
        _follow = true;
        _current_step = 0;
    }

    void ExecutePlanStep() {

        movDirection = (new Vector3(_plan[_current_step].x, _plan[_current_step].y) - transform.position);
        //rb2d.velocity = direction * 1.0f;
        transform.position += movDirection.normalized * Time.deltaTime;
        
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

        Debug.DrawLine(transform.position, transform.position + movDirection);
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
                if (Time.time - timeLastSighting > 3)
                {
                    _follow = false;
                    _patrolling = true;
                    float fat_dot = 0.0f;
                    _plan = myplanner.planToPosition(transform.position, _patrolPoints[0], fat_dot);
                    _currentPatrolPlan = 0;
                }
                else
                {
                    PlanFollow();
                }

                    
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

    public Vector3 getDir()
    {

        return movDirection;

    }

    public void SetState(bool patrolling, bool following, bool alert, GameObject target = null)
    {
        if(_replan == false) _replan = true;
        if (target != null) _target = target;
        else _target = player;
        timeLastSighting = Time.time;
        _patrolling = patrolling;
        _follow = following;
    }
}
