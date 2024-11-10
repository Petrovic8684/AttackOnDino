using Pathfinding;
using UnityEngine;

public abstract class EnemyFollow : MonoBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected float maxDistance;
    [SerializeField] protected float minDistance;
    [SerializeField] protected float updateSeconds;
    [SerializeField] protected float speed;
    [SerializeField] protected float nextWaypointDistance;

    protected bool followEnabled = true;
    protected bool directionLookEnabled = true;
    protected Path path;
    protected int currentWaypoint = 0;
    protected Seeker seeker;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, updateSeconds);
    }

    protected virtual void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    protected abstract void PathFollow();

    protected bool TargetInDistance()
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);
        return !TargetTooClose(distance) && !TargetTooFar(distance);
    }

    protected bool TargetTooFar(float distance)
    {
        return distance >= maxDistance;
    }

    protected bool TargetTooClose(float distance)
    {
        return distance <= minDistance;
    }

    protected void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
