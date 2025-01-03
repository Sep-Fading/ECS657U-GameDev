using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class SkeleGhostController : AbstractEnemy
    {
        public float health;
        float moveCooldown;
        public bool attacking;
        public bool raining;
        public float rainTimer = 10f;
        public float rainSpawnTime = 1f;

        protected override void Awake()
        {
            base.Awake();
            xpDrop = 125f;
            goldDrop = 150;
            attackDistance = 15f;
            attackCooldown = 10f;
            stats.Life.SetFlat(1000f);
            stats.Damage.SetFlat(30f);
            moveCooldown = 4f;
            attacking = false;
            attackPattern.Add(shootAttack);
        }
        protected override void Start()
        {
            baseSpeed = 10f;
            runSpeed = 10f;
            SetState(EnemyState.IDLE);
            base.Start();
        }
        protected override void Update()
        {
            if (!(GetState() == EnemyState.IDLE))
            {
                if (GameObject.Find("master") != null) { GameObject.Find("master").SetActive(false); }
                health = stats.Life.GetCurrent();
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
                if (stats.Life.GetCurrent() <= stats.Life.GetFlat() / 2f)
                {
                    attackPattern = new List<System.Action>() { rainAttack, shootAttack };
                }
                transform.LookAt(player.transform);
                if (distanceBetweenPlayer <= 1.5f) { animator.SetTrigger("punchTrigger"); }
                if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
                else if (attackCooldown <= 0 && !attacking) { SetState(EnemyState.ATTACK); }
                else { attackCooldown -= Time.deltaTime; SetState(EnemyState.TRIGGERED); }
                if (raining)
                {
                    if (rainTimer > 0f && rainSpawnTime <= 0f)
                    {
                        GameObject ghostBall = Instantiate(Resources.Load("GhostBall") as GameObject);
                        ghostBall.transform.position = new Vector3(Random.Range(275f, 297f), 11f, Random.Range(402f, 432f));
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
                    audioSource.spatialBlend = 1f;
                    audioSource.loop = false;
                    audioSource.clip = Resources.Load("Audio/ClubThrow") as AudioClip;
                    if (!audioSource.isPlaying) { audioSource.Play(); }
                    moveCooldown = 3f;
                    setSpeed(runSpeed);
                    StopAllCoroutines();
                    animator.SetBool("isMoving", true);
                    float randomX;
                    float randomZ;
                    if (Random.value < 0.5) randomX = Random.Range(-5f, -3f);
                    else randomX = Random.Range(3f, 5f);
                    if (Random.value < 0.5) randomZ = Random.Range(-5f, -3f);
                    else randomZ = Random.Range(3f, 5f);
                    StartCoroutine(MoveTo(player.transform.position + new Vector3(randomX, Random.Range(0f, 1f), randomZ)));
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
            attacking = false;
        }
        public void throwBall()
        {
            audioSource.spatialBlend = 1f;
            audioSource.loop = false;
            audioSource.clip = Resources.Load("Audio/Cast") as AudioClip;
            if (!audioSource.isPlaying) { audioSource.Play(); }
            GameObject ball = Instantiate(Resources.Load("GhostBall") as GameObject, transform);
            ball.GetComponent<Renderer>().enabled = true;
            ball.GetComponent<Rigidbody>().isKinematic = false;
            ball.GetComponent<Rigidbody>().AddForce(((player.transform.position - transform.position).normalized) * 10f, ForceMode.Impulse);
            attackCooldown = 5f;
        }
        public void rainAttack()
        {
            animator.SetTrigger("rainTrigger");
            attacking = false;
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
        public override void destroySelf()
        {
            if (GameObject.Find("Portal") != null)
            {
                if (GameObject.Find("Portal").GetComponent<AudioSource>() != null)
                {
                    GameObject.Find("Portal").GetComponent<AudioSource>().Play();
                }
                GameObject.Find("Portal").GetComponent<Collider>().enabled = true;
                if (GameObject.Find("PortalHole") != null)
                {
                    GameObject.Find("PortalHole").GetComponent<Renderer>().enabled = true;
                }
            }
            base.destroySelf();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Weapon"))
            {
                Debug.Log("Enemy Attacked");
                animator.SetTrigger("stunTrigger");
                playerStats.DoDamage(this);
                attacking = false;
                audioSource.spatialBlend = 1f;
                audioSource.loop = false;
                audioSource.clip = Resources.Load("Audio/EnemyHit") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }

            }
            if (other.gameObject.CompareTag("EnemyWeaponThrowable"))
            {
                animator.SetTrigger("stunTrigger");
                playerStats.DoDamage(this);
                audioSource.spatialBlend = 1f;
                audioSource.loop = false;
                audioSource.clip = Resources.Load("Audio/EnemyHit") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
            }
        }
    }
}
