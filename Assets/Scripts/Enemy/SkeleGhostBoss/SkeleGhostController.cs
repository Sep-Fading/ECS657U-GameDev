using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class SkeleGhostController : AbstractEnemy
    {
        float moveCooldown;
        public bool attacking;
        public bool raining;
        public float rainTimer = 10f;
        public float rainSpawnTime = 1f;

        protected override void Awake()
        {
            base.Awake();

            attackDistance = 15f;
            attackCooldown = 20f;
            moveCooldown = 1.5f;
            attacking = false;
            attackPattern.Add(throwBall);
        }
        protected override void Start()
        {
            baseSpeed = 10f;
            runSpeed = 10f;
            SetState(EnemyState.TRIGGERED);
            base.Start();
        }
        protected override void Update()
        {
            distanceBetweenPlayer = Vector3.Distance(transform.position, player.transform.position);
            //Debug.Log("Moving: " + animator.GetBool("isMoving"));
            switch (enemyState)
            {
                case EnemyState.IDLE:
                    idle();
                    break;
                case EnemyState.TRIGGERED:
                    followPlayer();
                    break;
                case EnemyState.ATTACK:
                    attack();
                    break;
                case EnemyState.DEAD:
                    despawn();
                    break;
                default:
                    break;
            }
            transform.LookAt(player.transform);
            if (distanceBetweenPlayer <= 1.5f) { animator.SetTrigger("punchTrigger"); }
            if (attackCooldown <= 0 && !attacking) { SetState(EnemyState.ATTACK); }
            else { attackCooldown -= Time.deltaTime; SetState(EnemyState.TRIGGERED); }
            if (raining)
            {
                if (rainTimer > 0f && rainSpawnTime <= 0f)
                {
                    GameObject ghostBall = Instantiate(Resources.Load("GhostBall") as GameObject);
                    ghostBall.transform.position = new Vector3(Random.Range(-10f, 10f), 11f, Random.Range(-30f, -2f));
                    ghostBall.GetComponent<Renderer>().enabled = true;
                    ghostBall.GetComponent<Rigidbody>().isKinematic = false;
                    ghostBall.GetComponent<Rigidbody>().AddForce(Vector3.down * 10f, ForceMode.Impulse);
                    rainSpawnTime = 1f;
                }
                if (rainTimer <= 0f)
                {
                    attackCooldown = 20f;
                    rainTimer = 5f;
                    rainSpawnTime = 1f;
                    attacking = false;
                    raining = false;
                    GetComponentInChildren<Renderer>().enabled = true;
                    GetComponent<Collider>().enabled = true;
                    onAttackComplete();
                }
                else
                {
                    rainSpawnTime -= Time.deltaTime;
                    rainTimer -= Time.deltaTime;
                }
            }
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

            Vector3 lastPosition = transform.position;
            float stuckCheckInterval = 0.5f;
            float stuckThreshold = 0.05f;
            float elapsedTime = 0f;

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                animator.SetBool("isMoving", true);
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    stats.Speed.GetCurrent() * Time.deltaTime
                );

                // Increment elapsed time
                elapsedTime += Time.deltaTime;

                // Check for stuck condition
                if (elapsedTime >= stuckCheckInterval)
                {
                    float distanceMoved = Vector3.Distance(transform.position, lastPosition);
                    if (distanceMoved <= stuckThreshold)
                    {
                        animator.SetBool("isMoving", false);
                        yield break; // Stop moving
                    }

                    // Reset for the next interval
                    lastPosition = transform.position;
                    elapsedTime = 0f;
                }

                yield return null; // Wait for the next frame
            }

            animator.SetBool("isMoving", false);
        }
        public override void followPlayer()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            //else if (attackCooldown <= 0f) SetState(EnemyState.ATTACK);
            else
            {
                if (moveCooldown <= 0)
                {
                    moveCooldown = 1.5f;
                    setSpeed(runSpeed);
                    StopAllCoroutines();
                    animator.SetBool("isMoving", true);
                    StartCoroutine(MoveTo(player.transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(0f, 1f), Random.Range(-5f, 5f))));
                }
                else moveCooldown -= Time.deltaTime;
            }
        }
        public override void attack()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else
            {
                StopAllCoroutines();
                attacking = true;
                animator.SetBool("isMoving", false);
                int attack = Random.Range(0, attackPattern.Count);
                attackPattern[attack].Invoke();
            }
        }
        public void shootAttack()
        {
            animator.SetTrigger("shootTrigger");
        }
        public void throwBall()
        {
            GameObject ball = Instantiate(Resources.Load("GhostBall") as GameObject, transform);
            ball.GetComponent<Renderer>().enabled = true;
            ball.GetComponent<Rigidbody>().isKinematic = false;
            ball.GetComponent<Rigidbody>().AddForce(((player.transform.position - transform.position).normalized) * 10f, ForceMode.Impulse);
            attacking = false;
            attackCooldown = 5f;
        }
        public void rainAttack()
        {
            animator.SetTrigger("rainTrigger");
        }
        public void beginRain()
        {
            GetComponentInChildren<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            raining = true;
        }
        public override void onAttackComplete()
        {
            base.onAttackComplete();
        }
    }
}
