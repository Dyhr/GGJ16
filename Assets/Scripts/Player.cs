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
    public List<int> DoFirst = new List<int>();
    public List<bool> Hidden = new List<bool>();

    private Human human;
    internal Seeker seeker;
    internal Path path;
    private int currentWaypoint = 0;
    private bool _awaitingPath;

    private Interactable Task;
    public bool doing = false;

    public ClickMe AttPrefab;
    public Animator Manimator;

    private void Start()
    {
        human = GetComponent<Human>();
        seeker = GetComponent<Seeker>();
        seeker.pathCallback += OnPathComplete;

        UpdateAtt();
    }

    private void UpdateAtt()
    {
        if (Todo.Count == 0) return;

        foreach (var item in FindObjectsOfType<Interactable>())
        {
            item.Attention(CanDo(item.Name), AttPrefab);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public bool CanDo(string task)
    {
        if (!Todo.Contains(task)) return false;
        var i = Todo.IndexOf(task);
        return DoFirst[i] < 0;
    }
    public void RemoveTask(string task)
    {
        var i = Todo.IndexOf(task);
        Todo.RemoveAt(i);
        DoFirst.RemoveAt(i);
        Hidden.RemoveAt(i);
        for (int j = 0; j < Hidden.Count; ++j) {
            if (DoFirst[j] == i)
                DoFirst[j] = -1;
            else if(j >= i && DoFirst[j] >= i)
                DoFirst[j]--;
        }
    }

    public void Update()
    {
        Manimator.SetBool("Acting", doing);
        Manimator.SetFloat("Speed", 0);

        if (!Control) return;

        // Following your path

        if (path != null)
        {
            Manimator.SetFloat("Speed", 1);
            GetComponent<Rigidbody>().freezeRotation = false;
            if (currentWaypoint >= path.vectorPath.Count)
            {
                Debug.Log("Went there!");
                path = null;
                return;
            }

            Debug.DrawLine(transform.position, transform.position + transform.forward * nextWaypointDistance, Color.blue, 0.1f, true);

            human.InputControl = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
                currentWaypoint++;
        }
        else
        {
            Manimator.SetFloat("Speed", 0);
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        // Moving around

        var cam = Camera.main;
        if (cam != null)
        {

            var mask = 1 << LayerMask.NameToLayer("Target");
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100, mask) && !(doing && Task != null)) // Change this when adding interactions
            {
                Task = hit.transform.parent.GetComponent<Interactable>();
                //doing = false;
                Debug.Log("Gonna do this thing! " + Task.Name);

                if (!doing)
                {
                    targetPosition = Task.transform.position;
                    _awaitingPath = true;
                    seeker.StartPath(transform.position, targetPosition);
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red, 0.5f);
                }
            }
            else if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100) && !doing)
            {
                if (hit.transform.tag != "NoMove")
                {
                    Debug.Log("Gonna go there!");
                    Task = null;
                    doing = false;
                    targetPosition = hit.point;
                    _awaitingPath = true;
                    seeker.StartPath(transform.position, targetPosition);
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red, 0.5f);
                }
            }
        }

        // Doing tasks

        if (Task != null)
        {
            if (!doing && Vector3.Distance(transform.position, Task.transform.position) < Task.InteractionDistance)
            {
                if (Task.Done)
                {
                    Debug.Log("Already did that!");
                }
                else
                {
                    if (CanDo(Task.Name))
                    {
                        doing = true;
                    }
                    else
                    {
                        Debug.Log("Can't do that yet!");
                    }
                }
                path = null;
            }
            else if (doing)
            {
                if (Task.Done)
                {
                    doing = false;
                    Debug.Log("Did it!");
                    if (Todo.Contains(Task.Name))
                    {
                        RemoveTask(Task.Name);
                    }
                    UpdateAtt();
                    Task = null;
                    path = null;
                    return;
                }
                else
                {
                    Task.Interact(this);
                }
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
