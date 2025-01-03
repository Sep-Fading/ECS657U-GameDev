using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class SpiderController : AbstractEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            xpDrop = 7f;
            goldDrop = 25;
            attackDistance = 3f;
            attackCooldown = 1f;
            attackPattern.Add(biteAttack);
        }
        protected override void Start()
        {
            baseSpeed = 4f;
            runSpeed = 6f;
            stats.Life.SetFlat(100f);
            stats.Damage.SetFlat(10f);
            base.Start();
        }
        protected override void Update()
        {
            base.Update();
            if (animator.GetAnimatorTransitionInfo(0).IsName("Attack")
                || animator.GetAnimatorTransitionInfo(0).IsName("Stun")) 
                setSpeed(0f);
        }
        public override void idle()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else if (distanceBetweenPlayer <= stats.TriggeredDistance.GetAppliedTotal()) SetState(EnemyState.TRIGGERED);

            else
            {
                stats.Speed.SetCurrent(baseSpeed);
                animator.SetBool("isMoving", false);
                if (idleTime <= 0)
                {
                    // Randomly decide between moving or staying still
                    bool willMove = Random.value > 0.5f; // 50% chance to move or stay idle
                    if (willMove)
                    {
                        audioSource.spatialBlend = 1f;
                        audioSource.loop = true;
                        audioSource.clip = Resources.Load("Audio/SpiderWalk") as AudioClip;
                        if (!audioSource.isPlaying) { audioSource.Play(); }
                        animator.SetBool("isMoving", true);
                        Vector3 randomDirection = Random.insideUnitSphere * stats.IdleRadius.GetAppliedTotal();
                        randomDirection += transform.position; // Offset by current position
                        randomDirection.y = transform.position.y; // Maintain current Y position

                        StopAllCoroutines();
                        StartCoroutine(MoveTo(randomDirection));
                    }
                    else { audioSource.Pause(); }
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
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else if (distanceBetweenPlayer > stats.TriggeredDistance.GetAppliedTotal()) SetState(EnemyState.IDLE);
            else if (distanceBetweenPlayer <= attackDistance) SetState(EnemyState.ATTACK);
            else
            {
                StopAllCoroutines();
                animator.SetBool("isMoving", true);
                setSpeed(runSpeed);
                audioSource.spatialBlend = 1f;
                audioSource.loop = true;
                audioSource.clip = Resources.Load("Audio/SpiderRun") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
                StartCoroutine(MoveTo(player.transform.position));
            }
        }
        public void biteAttack()
        {
            animator.SetTrigger("attackTrigger");
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Weapon"))
            {
                Debug.Log("Enemy Attacked");
                playerStats.DoDamage(this);
                setSpeed(0f);
                gameObject.GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.up) * 2f, ForceMode.Impulse);
                animator.SetTrigger("stunTrigger");
                audioSource.spatialBlend = 1f;
                audioSource.loop = false;
                audioSource.clip = Resources.Load("Audio/EnemyHit") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
            }
        }
    }
}
