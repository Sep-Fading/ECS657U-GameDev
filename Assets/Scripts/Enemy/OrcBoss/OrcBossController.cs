using Enemy;
using GameplayMechanics.Character;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

namespace enemy
{
    public class OrcBossController : AbstractEnemy
    {
        bool isThrowing;
        public bool isCircling;
        float circlingTimer;
        float circlingCooldown;
        [SerializeField] float speed;
        protected override void Awake()
        {
            base.Awake();

            xpDrop = 75f;
            goldDrop = 75;
            attackDistance = 4f;
            attackCooldown = 1.5f;
            attackPattern.Add(weaponAttack);
            attackPattern.Add(punchAttack);
            //attackPattern.Add(jumpAttack);
            //attackPattern.Add(throwAttack);
        }
        protected override void Start()
        {
            baseSpeed = 10f;
            runSpeed = 10f;
            isCircling = false;
            circlingTimer = 5f;
            circlingCooldown = 10f;
            SetState(EnemyState.IDLE);
            base.Start();
            stats.TriggeredDistance.SetCurrent(100f);
            stats.Speed.SetFlat(runSpeed);
            stats.Life.SetFlat(500f);
            stats.Damage.SetFlat(15f);
            speed = stats.Speed.GetCurrent();
        }
        protected override void Update()
        {
            base.Update();

            if (!isCircling) circlingCooldown -= Time.deltaTime;
            //Debug.Log("Speed: " + stats.Speed.GetCurrent());
        }
        public override IEnumerator MoveTo(Vector3 targetPosition)
        {
            // Normal chasing behavior if not circling
            if (!isCircling)
            {
                Vector3 direction = targetPosition - transform.position;

                if (direction.magnitude <= 0.001f)
                {
                    yield break; // Exit if the target position is too close
                }

                direction.Normalize();
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                float stopDistance = GetState() == EnemyState.TRIGGERED ? attackDistance : 0.1f;

                Vector3 lastPosition = transform.position;
                float stuckCheckInterval = 0.1f;
                float stuckThreshold = 0.0005f;
                float elapsedTime = 0f;

                while (Vector3.Distance(transform.position, targetPosition) > stopDistance)
                {
                    animator.SetBool("isMoving", true);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * stats.Speed.GetFlat());
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, stats.Speed.GetFlat() * Time.deltaTime);
                    // Increment elapsed time
                    elapsedTime += Time.deltaTime;
                    float distanceMoved = 0f;
                    // Check for stuck condition
                    if (elapsedTime >= stuckCheckInterval)
                    {
                        distanceMoved = Vector3.Distance(transform.position, lastPosition);
                        if (distanceMoved <= stuckThreshold)
                        {
                            if (audioSource.isPlaying) { audioSource.Pause(); }
                            animator.SetBool("isMoving", false);
                            yield break; // Stop moving
                        }

                        // Reset for the next interval
                        lastPosition = transform.position;
                        elapsedTime = 0f;
                    }
                    yield return null; // Wait for the next frame
                }
                if (audioSource.isPlaying) { audioSource.Pause(); }
                animator.SetBool("isMoving", false);
            }
            else
            {
                // Circling behavior
                while (circlingTimer > 0f)
                {
                    animator.SetBool("isMoving", true);

                    circlingTimer -= Time.deltaTime;

                    // Rotate around the player
                    transform.RotateAround(targetPosition, Vector3.up, stats.Speed.GetCurrent() * 2f * Time.deltaTime);

                    Quaternion targetRotation = Quaternion.LookRotation(Vector3.Cross(transform.position - targetPosition, Vector3.up).normalized * -1);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * stats.Speed.GetFlat());
                    // Randomly throw a weapon at the player
                    if (Random.value < 0.5f && Time.time - lastAttackTime >= attackCooldown)
                    {
                        throwAttack();
                        lastAttackTime = Time.time;
                    }
                    yield return null;
                }
                if (audioSource.isPlaying) { audioSource.Pause(); }
                // Exit circling
                isCircling = false;
                circlingCooldown = 10f;
                animator.SetBool("isMoving", false);
                setSpeed(runSpeed);
            }
        }
        public override void idle()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else if (distanceBetweenPlayer <= stats.TriggeredDistance.GetAppliedTotal())
            {
                SetState(EnemyState.TRIGGERED);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
        public override void followPlayer()
        {
            if (stats.Life.GetCurrent() <= 0)
            {
                SetState(EnemyState.DEAD);
            }
            else if (distanceBetweenPlayer <= attackDistance && !isCircling)
            {
                SetState(EnemyState.ATTACK);
            }
            else
            {
                // Normal chasing behavior
                StopAllCoroutines();
                attackDistance = 3f;
                if (!isThrowing)
                {
                    audioSource.spatialBlend = 1f;
                    audioSource.loop = true;
                    audioSource.clip = Resources.Load("Audio/Level1Run") as AudioClip;
                    if (!audioSource.isPlaying) { audioSource.Play(); }
                }
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
                StartCoroutine(MoveTo(player.transform.position));
                if (Random.value < 0.01 && distanceBetweenPlayer > attackDistance + 5f) throwAttack();
            }
        }
        private void StartCircling()
        {
            isCircling = true;
            circlingTimer = 10f; 
            attackCooldown = 2f;
            StopAllCoroutines();
            if (!isThrowing)
            {
                audioSource.spatialBlend = 1f;
                audioSource.loop = true;
                audioSource.clip = Resources.Load("Audio/Level1Run") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
            }
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
            StartCoroutine(MoveTo(player.transform.position));
        }
        public override void attack()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else
            {
                StopAllCoroutines();
                animator.SetBool("isMoving", false);

                if (Time.time - lastAttackTime >= attackCooldown && !isCircling) // Check cooldown
                {
                    bool runPattern = Random.value > 0.5f; // 50% chance to run attack pattern or a random attack
                    if (runPattern)
                    {
                        foreach (var attack in attackPattern)
                        {
                            attack.Invoke();
                            Debug.Log("Attack: " + attack.Method);
                        }
                    }
                    else
                    {
                        int attack = Random.Range(0, attackPattern.Count);
                        attackPattern[attack].Invoke();
                        Debug.Log("Attack: " + attackPattern[attack].Method);
                    }
                    lastAttackTime = Time.time; // Reset cooldown
                }
                if (Random.value < 0.4f && !isCircling && circlingCooldown <= 0f)
                { jumpAttack(); isCircling = true; }
                SetState(EnemyState.TRIGGERED);
            }
        }
        public void punchAttack() { animator.SetTrigger("punchTrigger"); }
        public void weaponAttack() { animator.SetTrigger("weaponTrigger"); }
        public void throwAttack() 
        {
            animator.SetTrigger("weaponTrigger");
            isThrowing = true;
        }
        public void checkThrowAttack()
        {
            if (isThrowing)
            {
                transform.LookAt(player.transform);
                GameObject.FindWithTag("EnemyWeapon").GetComponent<Renderer>().enabled = false;
                GameObject newWeapon = Instantiate(Resources.Load("OrcBossWeapon"), transform) as GameObject;
                newWeapon.transform.position += new Vector3(0f,1f,0f);
                newWeapon.GetComponent<Renderer>().enabled = true;
                newWeapon.GetComponent<Rigidbody>().isKinematic = false;
                newWeapon.GetComponent<Rigidbody>().AddForce(((player.transform.position - transform.position).normalized) * 25f, ForceMode.Impulse);
                audioSource.spatialBlend = 1f;
                audioSource.loop = false;
                audioSource.clip = Resources.Load("Audio/ClubThrow") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
            }
        }
        public void enableWeapon()
        {
            if (isThrowing)
            {
                GameObject.FindWithTag("EnemyWeapon").GetComponent<Renderer>().enabled = true;
                GameObject newWeapon = Instantiate(Resources.Load("OrcBossWeapon"), transform) as GameObject;
                isThrowing = false;
            }
        }
        public void jump() 
        {
            setSpeed(0f);
            transform.LookAt(player.transform);
            Debug.Log((transform.position - player.transform.position).normalized);
            //gameObject.GetComponent<Rigidbody>().AddForce((Vector3.up + (transform.position - player.transform.position).normalized) * 7f, ForceMode.Impulse);
            gameObject.GetComponent<Rigidbody>().AddForce((Vector3.up + Vector3.back) * 5f, ForceMode.Impulse);
        }
        public void jumpAttack()
        {
            transform.LookAt(player.transform);
            animator.SetTrigger("jumpTrigger");
        }
        public void onJump()
        {
            animator.SetTrigger("jumpIdleTrigger");
            transform.LookAt(player.transform);

        }
        public void checkNearGround()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f)) 
            { 
                animator.SetTrigger("jumpEndTrigger");
                setSpeed(runSpeed);
            }
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
            if (GameObject.Find("-- Enemy") != null && GameObject.Find("-- Enemy").GetComponent<AudioSource>() != null && GameObject.Find("BossGate").GetComponent<AudioSource>() != null)
            {
                GameObject.Find("-- Enemy").GetComponent<AudioSource>().Play();
                GameObject.Find("BossGate").GetComponent<AudioSource>().Pause();
            }
            base.destroySelf();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Weapon") && Random.value <= 0.5f)
            {
                PlayerStatManager.Instance.DoDamage(this);
                setSpeed(0f);
                animator.SetTrigger("stunTrigger");
                audioSource.spatialBlend = 1f;
                audioSource.loop = false;
                audioSource.clip = Resources.Load("Audio/EnemyHit") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
                if (Random.value < 0.5) { jumpAttack(); }
            }
        }
    }
}
