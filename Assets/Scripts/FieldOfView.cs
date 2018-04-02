using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{

    public struct viewCastInfo
    {
        public bool _hit;
        public Vector2 _point;
        public float _dst;
        public float _angle;

        public viewCastInfo(bool hit, Vector3 point, float dst, float angle)
        {
            _hit = hit;
            _point = point;
            _dst = dst;
            _angle = angle;
        }
    }

    public float viewRadius;
    public float meshResolution;
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    [Range(0, 360)]
    public float viewAngle;
    public EnemyController enemyRef;

    [SerializeField]
    public LayerMask playerMask;
    [SerializeField]
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
        enemyRef = GetComponent<EnemyController>();
        viewMesh = new Mesh();
        viewMesh.name = "viewMesh";
        viewMeshFilter.mesh = viewMesh;
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Debug.Log("Hola");
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(GetComponent<EnemyController>().getDir(), dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    GetComponent<EnemyController>().SetState(false, true, false);
                }
            }
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        float aux_angle;
        if (!angleIsGlobal)
        {
            //angleInDegrees += Mathf.Atan(GetComponent<EnemyController>().getDir().y / GetComponent<EnemyController>().getDir().x);
            aux_angle = Vector2.SignedAngle(GetComponent<EnemyController>().getDir(), new Vector2(0, 1));
            //aux_angle = Mathf.Atan(GetComponent<EnemyController>().getDir().y / GetComponent<EnemyController>().getDir().x) * Mathf.Rad2Deg;

            if (aux_angle < 0) aux_angle = 360 + aux_angle;

            angleInDegrees += aux_angle;
            Debug.Log("Suma: " + aux_angle + " Resultado: " + angleInDegrees);
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    void drawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        viewCastInfo newViewCast;
        List<Vector3> viewPoints = new List<Vector3>();

        for(int i = 0; i <= stepCount; i++)
        {
            float angle = Vector2.SignedAngle(GetComponent<EnemyController>().getDir(), new Vector2(0, 1));
            if (angle < 0) angle = 360 + angle;
            angle = angle - viewAngle / 2 + stepAngleSize * i;
            Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);
            newViewCast = viewCast(angle);
            viewPoints.Add(newViewCast._point);
        }

        int vertexCount = viewPoints.Count + 1;

        Vector3[] vertices = new Vector3[vertexCount];

        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for(int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = viewPoints[i];
            if(i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
            
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private viewCastInfo viewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);

        if (hit.collider != null)
        {
            return new viewCastInfo(true, hit.point, hit.distance, globalAngle);
        } else
        {
            return new viewCastInfo(false, transform.position + dir  *viewRadius , viewRadius, globalAngle);
        }
    }

    private void Update()
    {
        drawFieldOfView();
    }
}
