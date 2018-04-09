using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// estado alerta (??)
// LLamar a la planificacion de las patrullas en la pantalla de carga

public class EnemyController : MonoBehaviour {
    Astar myplanner;
    bool _follow, _replan, _patrolling, _lookAround;
    List<Vector2> _plan;
    int _current_step, _currentPatrolPlan;
    GameObject player;
    private Rigidbody2D rb2d;
    List<List<Vector2>> _patrols = null;
    List<Vector2> _patrolPoints = null;
    Room _currentRoom;
    Vector3 movDirection, lastKnownMovDirection;
    float lastSightingClock, lookingAroundClock;
    GameObject _target;


    [SerializeField]
    int _roomId;
    [SerializeField]
    Room _myRoom;

    public float followingTime;
    public float lookingAroundTime;
    


    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        myplanner = GetComponent<Astar>();
        myplanner.InitVars(transform.localScale.x * GetComponent<BoxCollider2D>().size.x /2);
        player = GameObject.Find("Player");
        _replan = true;
        _follow = false;
        _patrolling = true;
        _lookAround = false;
        _currentPatrolPlan = 0;
        movDirection = new Vector3(0, -1, 0);
        _patrolPoints = _myRoom.getPatrol(_roomId);
        for (int i = 0; i < _patrolPoints.Count; i++)
            Debug.DrawLine(_patrolPoints[i], _patrolPoints[i] + new Vector2(0.1f, 0.1f), Color.yellow, 60f);
        InitPatrols();
    }

    void InitPatrols()
    {
        float fat_dot= 0.0F;
        _patrols = new List<List<Vector2>>();
        for(int i = 0; i < _patrolPoints.Count; i++)
        {
            _patrols.Add(myplanner.planToPosition(_patrolPoints[i], _patrolPoints[(i + 1) % _patrolPoints.Count], fat_dot));
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
    }

    void PlanFollow() {
        float distance_to_target = Vector3.Distance(transform.position, _target.transform.position);
        _plan = myplanner.planToPosition(transform.position, _target.transform.position, distance_to_target / 2);
        _follow = true;
        _follow = true;
        _current_step = 0;
    }

    void ExecutePlanStep() {

        movDirection = (new Vector3(_plan[_current_step].x, _plan[_current_step].y) - transform.position);
        if(movDirection != Vector3.zero) lastKnownMovDirection = movDirection;
        transform.position += movDirection.normalized * Time.deltaTime;
        
        if (Vector3.Distance(new Vector3(_plan[_current_step].x, _plan[_current_step].y), transform.position) < transform.localScale.x * GetComponent<BoxCollider2D>().size.x)
        {

            _current_step++;
            if (_current_step >= _plan.Count)
            {
                if(_follow)_plan.Clear();

                _current_step = 0;
                if (!_follow)
                {
                    lookingAroundClock = Time.time;
                    _lookAround = true;
                }
                _replan = true;
            }
            else {
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
                if (Time.time - lastSightingClock > 3)
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

        if (_lookAround) LookAround();
        else ExecutePlanStep();    
        
    }

    void LookAround()
    {
        if(Time.time - lookingAroundClock <= lookingAroundTime)
        {
            // Do funny stuff
        } else
        {
            _lookAround = false;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Player") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }

    public Vector3 getDir()
    {
        return lastKnownMovDirection.normalized;
    }

    public void SetState(bool patrolling, bool following, bool alert, GameObject target = null)
    {
        if(_replan == false) _replan = true;
        if (target != null) _target = target;
        else _target = player;
        lastSightingClock = Time.time;
        _patrolling = patrolling;
        _follow = following;
        _lookAround = false;
    }
}
