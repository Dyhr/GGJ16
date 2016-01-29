using System.Linq;
using UnityEngine;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;

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

    public List<string> Todo = new List<string>();

    private Human human;
    internal Seeker seeker;
    internal Path path;
    private int currentWaypoint = 0;
    private bool _awaitingPath;

    private Interactable Task;
    private bool doing = false;

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

        if(Task != null)
        {
            if (!doing && Vector3.Distance(transform.position,Task.transform.position) < Task.InteractionDistance)
            {
                doing = true;
                path = null;
            }
            else if (doing)
            {
                doing = false;
                if (Todo.Contains(Task.Name)) {
                    Todo.Remove(Task.Name);
                }
                Task = null;
            }
        }

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

            var mask = 1 << LayerMask.NameToLayer("Target");
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100, mask))
            {
                Task = hit.transform.parent.GetComponent<Interactable>();
                doing = false;

                targetPosition = Task.transform.position;
                _awaitingPath = true;
                seeker.StartPath(transform.position, targetPosition);
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red, 0.5f);
            }
            else if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100))
            {
                Task = null;
                doing = false;
                targetPosition = hit.point;
                _awaitingPath = true;
                seeker.StartPath(transform.position, targetPosition);
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red, 0.5f);
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
