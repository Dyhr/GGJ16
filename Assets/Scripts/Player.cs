using System.Linq;
using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent(typeof(Human))]
[RequireComponent(typeof(Seeker))]
public class Player : MonoBehaviour
{
    public float nextWaypointDistance = 0.3f;
    public Vector3 targetPosition;

    public float SlowSpeed;
    public float NormalSpeed;
    public float FastSpeed;

    public bool Control = true;

    private Human human;
    internal Seeker seeker;
    internal Path path;
    private int currentWaypoint = 0;
    private bool _awaitingPath;

    private void Start()
    {
        human = GetComponent<Human>();
        seeker = GetComponent<Seeker>();
        seeker.pathCallback += OnPathComplete;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void Update()
    {
        if (!Control) return;

        if (path != null)
        {
            if (currentWaypoint >= path.vectorPath.Count)
            {
                path = null;
                return;
            }

            Debug.DrawLine(transform.position, transform.position + transform.forward * nextWaypointDistance, Color.blue, 0.1f, true);

            human.InputControl = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
                currentWaypoint++;
        }
        else if (!_awaitingPath)
        {
            // Idle here
        }

        var cam = Camera.main;
        if (cam != null)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100))
            {
                targetPosition = hit.point;
                _awaitingPath = true;
                seeker.StartPath(transform.position, targetPosition);
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red, 0.1f);
            }
        }

        human.Speed = NormalSpeed;
    }

    public void Interrupt()
    {
        path = null;
    }

    public void OnPathComplete(Path p)
    {
        if (p.error) return;
        path = p;
        currentWaypoint = 0;
        _awaitingPath = false;
    }
}
