using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour {

    public EnemyController enemyRef;
    public float hearRadius;

    [SerializeField]
    public LayerMask playerMask;
    [SerializeField]
    public LayerMask distractionMask;

    [HideInInspector]
    public List<Transform> audibleTargets = new List<Transform>();

    // Use this for initialization
    void Start () {

        StartCoroutine("FindTargetsWithDelay", .2f);
        enemyRef = GetComponent<EnemyController>();

    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindAudibleTargets();
        }
    }

    void FindAudibleTargets()
    {
        audibleTargets.Clear();
        Collider2D[] targetsInHearRadius = Physics2D.OverlapCircleAll(transform.position, hearRadius, playerMask);
        Collider2D[] distractionsInHearRadius = Physics2D.OverlapCircleAll(transform.position, hearRadius, distractionMask);

        for (int i = 0; i < targetsInHearRadius.Length; i++)
        {
            Transform target = targetsInHearRadius[i].transform;
            if (target.gameObject.GetComponent<PlayerController>().isAudible())
            {
                audibleTargets.Add(target);
                GetComponent<EnemyController>().SetState(false, true, false);
            }
            
        }
        /*
        if (targetsInHearRadius.Length == 0) {
            for (int i = 0; i < distractionsInHearRadius.Length; i++)
            {
                Transform target = distractionsInHearRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                audibleTargets.Add(target);
                GetComponent<EnemyController>().SetState(false, true, false, distractionsInHearRadius[i].transform.gameObject);
            }
        }
        */
    }

    // Update is called once per frame
    void Update () {
		
	}
}
