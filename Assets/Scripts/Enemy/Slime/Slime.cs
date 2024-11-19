using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class Slime : AbstractEnemy
{
    private BoxCollider boxCollider;
    private Vector3 originalColliderSize;
    private Vector3 originalColliderCenter;
    protected override void Awake()
    {
        base.Awake();

        attackDistance = 2f;
        attackCooldown = 1f;
        attackPattern.Add(basicAttack);

        boxCollider = GetComponent<BoxCollider>();
    }
    protected override void Start()
    {
        base.Start();

        originalColliderSize = boxCollider.size;
        originalColliderCenter = boxCollider.center;
    }
    protected override void Update()
    {
        base.Update();
    }

    public override IEnumerator MoveTo(Vector3 targetPosition)
    {
        // Calculate the direction to the target
        Vector3 direction = targetPosition - transform.position;

        if (direction.magnitude <= 0.001f)
        {
            yield break; // Exit if the target position is too close
        }

        // Normalize the direction
        direction.Normalize();

        // Calculate target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0f,90f,0f);
        float stopDistance = enemyState == EnemyState.TRIGGERED ? attackDistance : 0.1f;

        while (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            animator.SetBool("isMoving", true);
            // Rotate toward the target
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * stats.Speed.GetCurrent()
            );

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                stats.Speed.GetCurrent() * Time.deltaTime
            );

            yield return null; // Wait for the next frame
        }
    }
    public void basicAttack() 
    {
        animator.SetTrigger("attackTrigger");
    }
    // Function to modify the Box Collider for attacking
    public void IncreaseBoxCollider()
    {
        if (boxCollider != null)
        {
            // Increase the size
            boxCollider.size = new Vector3(3.5f, 2.7f, 3.0f);

            // Adjust the center
            boxCollider.center = new Vector3(-0.55f, 1.45f, -0.17f);

        }
    }
    public void ResetBoxCollider()
    {
        if (boxCollider != null)
        {
            boxCollider.size = originalColliderSize;
            boxCollider.center = originalColliderCenter;
        }
    }
}
