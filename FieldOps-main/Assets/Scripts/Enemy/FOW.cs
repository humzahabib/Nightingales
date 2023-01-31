using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class FOW : MonoBehaviour
{

    GameObjectEvent WitnessAddedEvent = new GameObjectEvent();

    FieldOfViewDrawing fieldOfViewDrawing;

    [SerializeField]
    float meshResolution;

    [SerializeField]
    float edgeDistanceThreshold;

    float shootingRange;
    public Action<ENEMYSTATES> StateIconChangedEvent;

    [SerializeField]
    LayerMask obstacleMask;

    bool isShooting = false;
    UnityEvent<Transform> playerDetectedEvent;
    UnityEvent<Vector2> suspectDetectedEvent;

    UnityEvent<Transform> playerInRangeEvent;

    [SerializeField]
    LayerMask allSuspectLayers;

    [SerializeField]
    MeshFilter viewMeshFilter;

    Mesh viewMesh;

    [SerializeField]
    int edgeResolveIterations;


    public float ViewRadius
    {
        get { return viewRadius; }
    }


    public bool IsShooting
    {
        set { isShooting = value; }
    }

    public UnityEvent<Transform> PlayerDetectedEvent
    {
        get { return playerDetectedEvent; }
    }

    public UnityEvent<Vector2> SuspectDetectedEvent
    {
        get { return suspectDetectedEvent; }
    }

    public UnityEvent<Transform> PlayerInRangeEvent
    {
        get { return playerInRangeEvent; }
    }


    public float viewRadius;


    [Range(0, 360)]
    public float viewAngle;


    private void Awake()
    {


        playerDetectedEvent = new UnityEvent<Transform>();
        suspectDetectedEvent = new UnityEvent<Vector2>();
        playerInRangeEvent = new UnityEvent<Transform>();

        viewMesh = new Mesh();
        viewMesh.name = "ViewMesh";
        viewMeshFilter.mesh = viewMesh;

    }

    private void Start()
    {
        InvokeRepeating("FindSuspaciousActivity", 3, .25f);

        EventManager.AddInvoker(GAMEOBJECTEVENTS.WITNESSADDEDEVENT, WitnessAddedEvent);

        fieldOfViewDrawing = new FieldOfViewDrawing(viewAngle, viewRadius, meshResolution,
        edgeResolveIterations, edgeResolveIterations, viewMesh,
        viewMeshFilter, obstacleMask, transform);
    }


    private void OnDestroy()
    {
        fieldOfViewDrawing.OnDestroy();
    }


    void FindSuspaciousActivity()
    {
        Collider2D[] suspectsInRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, allSuspectLayers);
        foreach (Collider2D suspect in suspectsInRadius)
        {
            Vector2 dirToSuspect = (Vector2)suspect.transform.position - (Vector2)transform.position;
            if (Vector2.Angle(transform.up, dirToSuspect) < viewAngle / 2)
            {

                float dis = Vector2.Distance(transform.position, suspect.transform.position);
                RaycastHit2D hit2D = Physics2D.Raycast(transform.position, dirToSuspect, dis);
                if (hit2D == suspect)
                {
                    if (hit2D.transform.tag.Equals("Player"))
                    {
                        WitnessAddedEvent.Invoke(this.gameObject);
                        if (hit2D.distance < viewRadius / 1.2f)
                            playerInRangeEvent.Invoke(suspect.transform);
                        else if (hit2D.distance > shootingRange)
                            playerDetectedEvent.Invoke(suspect.transform);
                    }
                    else if (hit2D.transform.tag == "Suspect")
                    {
                        Debug.LogWarning(suspect.name);
                        suspectDetectedEvent.Invoke(suspect.transform.position);
                    }

                }
            }
        }
    }


    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepSize = viewAngle / stepCount;

        List<Vector2> viewPoints = new List<Vector2>();

        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = -transform.eulerAngles.z - viewAngle / 2 + stepSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance)
                > edgeDistanceThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector2.zero)
                        viewPoints.Add(edge.pointA);
                    if (edge.pointB != Vector2.zero)
                        viewPoints.Add(edge.pointB);
                }


            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        foreach (Vector2 point in viewPoints)
        {
            Vector2 dir = point - (Vector2)transform.position;
            Debug.DrawLine(transform.position, point, Color.red);
        }

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1].z = 0;
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);



            if (i < vertexCount - 2)
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

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;

        Vector2 minPoint = Vector2.zero;
        Vector2 maxPoint = Vector2.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;

            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;

            if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }

        }

        return new EdgeInfo(minPoint, maxPoint);

    }


    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector2 direction = MyMath.DirFromAngle(transform, globalAngle, true);
        RaycastHit2D hit2D = Physics2D.Raycast((Vector2)transform.position, direction, viewRadius, obstacleMask);
        if (hit2D)
            return new ViewCastInfo(true, hit2D.point, hit2D.distance, globalAngle);
        else
            return new ViewCastInfo(false, (Vector2)transform.position + direction * viewRadius, viewRadius, globalAngle);
    }




    public struct ViewCastInfo
    {
        public bool hit;
        public Vector2 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector2 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector2 pointA;
        public Vector2 pointB;

        public EdgeInfo(Vector2 _pointA, Vector2 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }

    }

}
