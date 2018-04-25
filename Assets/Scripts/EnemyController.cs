using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Aumentar rango vision mientras persigue
// Arreglar el lookAroud
// Suavizar las trnasiciones del cono de vision
// Implementar las puertas
//
// LLamar a la planificacion de las patrullas en la pantalla de carga

public class EnemyController : MonoBehaviour {
    Astar myplanner;
    bool _follow, _replan, _patrolling, _alert, _lookAround;
    List<Vector2> _plan;
    int _current_step, _currentPatrolPlan, _currentAlertPlan;
    List<List<Vector2>> _patrols = null;
    List<Vector2> _patrolPoints = null, _alertPoints = null;
    List<int> _lookingAroundPattern = new List<int>{1,0,-1,-1,0,1};

    private Rigidbody2D rb2d;

    GameObject player;

    Room _currentRoom;
    Vector3 movDirection, lastKnownMovDirection;
    Vector2 lastSightingPoint;
    float lastSightingClock, lookingAroundClock, initAlertClock;
    GameObject _target;


    [SerializeField]
    int _roomId;
    [SerializeField]
    Room _myRoom;

    public float followingTime, lookingAroundTime, alertTime;
    public float _alertRadius;
    


    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        myplanner = GetComponent<Astar>();
        myplanner.InitVars(transform.localScale.x * GetComponent<BoxCollider2D>().size.x * 0.2f);
        player = GameObject.Find("Player");

        _replan = true;
        _follow = false;
        _patrolling = true;
        _alert = false;
        _lookAround = false;

        _currentPatrolPlan = 0;
        _currentAlertPlan = 0;

        movDirection = new Vector3(0, -1, 0);
        _patrolPoints = _myRoom.getPatrol(_roomId);

        for (int i = 0; i < _patrolPoints.Count; i++)
            Debug.DrawLine(_patrolPoints[i], _patrolPoints[i] + new Vector2(0.1f, 0.1f), Color.yellow, 60f);
        _patrols = InitPatrols(_patrolPoints);
    }

    List<List<Vector2>> InitPatrols(List<Vector2> pointsList)
    {
        float fat_dot= 0.0F;
        List <List<Vector2>> plans = new List<List<Vector2>>();
        for(int i = 0; i < pointsList.Count; i++)
        {
            plans.Add(myplanner.planToPosition(pointsList[i], pointsList[(i + 1) % pointsList.Count], fat_dot));
            for(int j = 0; j< plans[i].Count - 1; j++)
            {
                Debug.DrawLine(plans[i][j], plans[i][j + 1], Color.blue, 60f);
            }
        }

        return plans;

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
        //Vector3 fit = transform.position + new Vector3(GetComponent<BoxCollider2D>().offset.x, GetComponent<BoxCollider2D>().offset.y);
        movDirection = (new Vector3(_plan[_current_step].x, _plan[_current_step].y) - transform.position);
        if(movDirection != Vector3.zero) lastKnownMovDirection = movDirection;
        transform.position += movDirection.normalized * Time.deltaTime;
        
        if (Vector3.Distance(new Vector3(_plan[_current_step].x, _plan[_current_step].y), transform.position) < transform.localScale.x * GetComponent<BoxCollider2D>().size.x * 0.2f)
        {

            _current_step++;
            if (_current_step >= _plan.Count)
            {
                if(_follow) _plan.Clear();

                _current_step = 0;
                if (!_follow)
                {
                    lookingAroundClock = Time.time;
                    _lookAround = true;
                }
                _replan = true;
            }
            
        }

        Debug.DrawLine(transform.position, transform.position + movDirection);
    }

    void InitAlertPatrol()
    {
        List <Vector2> alertPatrol = new List<Vector2>();
        List <Vector2> roomAlertPoints = player.GetComponent<PlayerController>().getRoom().getAlertPoints();
        alertPatrol.Add(new Vector2(transform.position.x, transform.position.y));
        for(int i=0; i<roomAlertPoints.Count; i++)
        {
            if(Vector2.Distance(roomAlertPoints[i], lastSightingPoint) < _alertRadius)
            {
                alertPatrol.Add(roomAlertPoints[i]);
            }
        }

        _alertPoints = alertPatrol;

    }

    void PlanAlert()
    {
        _currentAlertPlan = (_currentAlertPlan + 1) % _alertPoints.Count;
        float fat_dot = 0.0F;
        _plan = myplanner.planToPosition(new Vector2(transform.position.x, transform.position.y), _alertPoints[_currentAlertPlan], fat_dot);
    }

    void Update () {

         if (_replan) {
            _replan = false;
            if (_patrolling)
            {
                PlanPatrol();
            }

            if(_follow)
            {
                if (Time.time - lastSightingClock > followingTime)
                {
                    _follow = false;
                    _alert = true;
                    initAlertClock = Time.time;
                    float fat_dot = 0.0f;
                    /*_plan = myplanner.planToPosition(transform.position, _patrolPoints[0], fat_dot);
                    for (int j = 0; j < _plan.Count - 1; j++)
                    {
                        Debug.DrawLine(_plan[j], _plan[j + 1], Color.blue, 60f);
                    }*/
                    _currentPatrolPlan = 0;
                }
                else
                {
                    PlanFollow();
                }

                    
            }

            if(_alert)
            {   
                if(Time.time - initAlertClock < alertTime)
                {
                    if (_alertPoints == null) InitAlertPatrol();
                    PlanAlert();
                }
                else
                {
                    _currentAlertPlan = 0;
                    _alertPoints = null;
                    float fat_dot = 0.0f;
                    _plan = myplanner.planToPosition(transform.position, _patrolPoints[0], fat_dot);
                    _alert = false;
                    _patrolling = true;
                }
                
            }
                
         }

        if (_lookAround && Time.timeScale == 1) LookAround();
        else ExecutePlanStep();    
        
    }

    void LookAround()
    {
        float aux_angle;

        if (Time.time - lookingAroundClock <= lookingAroundTime)
        {
            int index =(int) Mathf.Floor((Time.time - lookingAroundClock) / (lookingAroundTime / _lookingAroundPattern.Count));
            aux_angle = Vector2.SignedAngle(lastKnownMovDirection.normalized, new Vector2(0, 1));
            if (aux_angle < 0) aux_angle = 360 + aux_angle;
            lastKnownMovDirection = new Vector2(Mathf.Sin((aux_angle + (2*_lookingAroundPattern[index]))* Mathf.Deg2Rad), 
                Mathf.Cos((aux_angle + (2 * _lookingAroundPattern[index])) * Mathf.Deg2Rad));

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

    public void FollowSomething(GameObject target = null)
    {
        if(_replan == false) _replan = true;
        if (target != null) _target = target;
        else _target = player;
        lastSightingClock = Time.time;
        lastSightingPoint = _target.transform.position;
        _patrolling = false;
        _follow = true;
        _lookAround = false;
        if (_alert)
        {
            _alert = false;
            _currentAlertPlan = 0;
            _alertPoints = null;
            _plan = null;
        }
    }
}
