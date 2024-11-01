using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyFollow : MonoBehaviour
{
    [SerializeField] protected Transform target;


    [SerializeField] protected float activateDistance;


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

    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }


    protected abstract void PathFollow();


    protected bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

}
