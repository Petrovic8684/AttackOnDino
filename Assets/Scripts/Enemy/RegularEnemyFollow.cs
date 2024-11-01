using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularEnemyFollow : EnemyFollow
{

    [SerializeField] private RaycastHit2D isGrounded;

    private bool isOnCoolDown;

    [SerializeField]private float jumpForce;

    [SerializeField] private float jumpNodeHeightRequirement;

    [SerializeField] private float jumpModifier;

    [SerializeField] private float jumpCheckOffset;

    private bool jumpEnabled, isJumping, isInAir;

    [SerializeField] private Vector3 startOffset;


    protected override void Start()
    {
        isOnCoolDown = false;
        isJumping = false;
        isInAir = false;
        jumpEnabled = true;
        base.Start();
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset, transform.position.z);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;

        // Jump
        if (jumpEnabled && isGrounded && !isInAir && !isOnCoolDown)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                if (isInAir) return;
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                StartCoroutine(JumpCoolDown());

            }
        }
        if (isGrounded)
        {
            isJumping = false;
            isInAir = false;
        }
        else
        {
            isInAir = true;
        }

        // Movement
        rb.velocity = new Vector2(force.x, rb.velocity.y);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }


    

    private IEnumerator JumpCoolDown()
    {
        isOnCoolDown = true;
        yield return new WaitForSeconds(1f);
        isOnCoolDown = false;
    }

}
