using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Enemy
{
    public class SlimeController : AbstractEnemy
    {
        protected override void Awake()
        {
            base.Awake();

            attackDistance = 2f;
            attackCooldown = 1f;
            attackPattern.Add(jumpAttack);
            attackPattern.Add(biteAttack);
        }
        protected override void Start()
        {
            base.Start();

            stats.Speed.SetFlat(2f);
        }
        protected override void Update()
        {
            base.Update();

            if (GetState() == EnemyState.TRIGGERED) stats.Speed.SetFlat(4f);
            else stats.Speed.SetFlat(1f);
        }
        public override void idle()
        {
            if (stats.Life.GetCurrent() <= 0)
            {
                SetState(EnemyState.DEAD);
            }
            else if (distanceBetweenPlayer <= stats.TriggeredDistance.GetAppliedTotal())
            {
                SetState(EnemyState.TRIGGERED);
            }
            else
            {
                animator.SetBool("isMoving", false);
                animator.SetBool("isDancing", false);
                if (idleTime <= 0)
                {
                    // Randomly decide between moving or staying still
                    bool willMove = Random.value > 0.5f; // 50% chance to move or stay idle
                    if (willMove)
                    {
                        animator.SetBool("isMoving", true);
                        Vector3 randomDirection = Random.insideUnitSphere * stats.IdleRadius.GetAppliedTotal();
                        randomDirection += transform.position; // Offset by current position
                        randomDirection.y = transform.position.y; // Maintain current Y position

                        StopAllCoroutines();
                        StartCoroutine(MoveTo(randomDirection));
                    }
                    else
                    {
                        bool willDance = Random.value > 0.5f;
                        if (willDance) animator.SetBool("isDancing", true);
                        else animator.SetBool("isMoving", false);
                    }
                    // Set a new idle duration (2-5 seconds)
                    idleTime = Random.Range(2f, 5f);
                }
                else
                {
                    idleTime -= Time.deltaTime; // Count down idle time
                }
            }
        }
        public override void followPlayer()
        {
            if (stats.Life.GetCurrent() <= 0)
            {
                SetState(EnemyState.DEAD);
            }
            else if (distanceBetweenPlayer > stats.TriggeredDistance.GetAppliedTotal())
            {
                SetState(EnemyState.IDLE);
            }
            else if (distanceBetweenPlayer <= attackDistance)
            {
                SetState(EnemyState.ATTACK);
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(MoveTo(player.transform.position));
            }
        }
        public void jumpAttack()
        {
            animator.SetTrigger("jumpTrigger");
            GetComponent<Rigidbody>().AddForce(transform.up * 5f, ForceMode.Impulse);
        }
        public void biteAttack()
        {
            animator.SetTrigger("biteTrigger");
        }
    }
}
