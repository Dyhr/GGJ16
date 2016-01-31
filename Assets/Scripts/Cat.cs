using UnityEngine;
using Pathfinding;
using System.Collections;
using System;

[RequireComponent(typeof(Human))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AudioSource))]
public class Cat : MonoBehaviour
{

    public float nextWaypointDistance = 0.5f;
    public Vector3 targetPosition;

    public float SlowSpeed;
    public float NormalSpeed;
    public float FastSpeed;

    private Human human;
    private AudioSource purr;
    internal Seeker seeker;
    internal Path path;
    private int currentWaypoint = 0;
    private bool _awaitingPath;

    public Transform Owner;
    public float PurrDist = 1.6f;
    public AudioClip Nya;

    public Transform[] Waypoints;

    private void Start()
    {
        human = GetComponent<Human>();
        seeker = GetComponent<Seeker>();
        purr = GetComponent<AudioSource>();
        seeker.pathCallback += OnPathComplete;

        human.Speed = NormalSpeed;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void Update()
    {

        // Following your path

        if (path != null)
        {
            GetComponent<Rigidbody>().freezeRotation = false;
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
        else
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        // Moving around

        if (path == null && !_awaitingPath)
        {
            StopAllCoroutines();
            StartCoroutine(FindPath());
        }

        // Purr
        if (!purr.isPlaying && Vector3.Distance(transform.position, Owner.position) <= PurrDist)
            purr.Play();
        else if(purr.isPlaying && Vector3.Distance(transform.position, Owner.position) > PurrDist)
            purr.Pause();
    }

    private IEnumerator FindPath()
    {
        _awaitingPath = true;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.0f,8.0f));
        purr.PlayOneShot(Nya, 0.8f);
        if(Waypoints.Length != 0)
        {
            targetPosition = Waypoints[UnityEngine.Random.Range(0, Waypoints.Length)].position;
            seeker.StartPath(transform.position, targetPosition);

            human.Speed = Mathf.Lerp(SlowSpeed, FastSpeed, UnityEngine.Random.value);
        }
        yield return new WaitForSeconds(UnityEngine.Random.Range(3.0f, 8.0f));
        path = null;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        foreach (var point in Waypoints)
            Gizmos.DrawCube(point.position, 0.2f*Vector3.one);
    }
}
