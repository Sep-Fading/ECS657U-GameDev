using Enemy;
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

            attackDistance = 3f;
            attackCooldown = 1.5f;
            attackPattern.Add(weaponAttack);
            attackPattern.Add(punchAttack);
            //attackPattern.Add(jumpAttack);
            //attackPattern.Add(throwAttack);
        }
        protected override void Start()
        {
            baseSpeed = 2f;
            runSpeed = 7f;
            isCircling = false;
            circlingTimer = 5f;
            circlingCooldown = 10f;
            SetState(EnemyState.TRIGGERED);
            base.Start();
            stats.TriggeredDistance.SetCurrent(100f);
            stats.Speed.SetFlat(runSpeed);
            stats.Life.SetFlat(1000f);
            speed = stats.Speed.GetCurrent();
        }
        protected override void Update()
        {
            base.Update();

            if (animator.GetAnimatorTransitionInfo(0).IsName("Stun")) GetComponent<Rigidbody>().isKinematic = true;
            else GetComponent<Rigidbody>().isKinematic = false;

            if ((GetState() == EnemyState.TRIGGERED || GetState() == EnemyState.ATTACK)
                && distanceBetweenPlayer <= attackDistance
                && player.GetComponent<InputManager>().getPlayerInput().grounded.SwordAction.triggered
                && Random.value < 0.3
                && !animator.GetAnimatorTransitionInfo(0).IsName("Dodge")
                && GameObject.FindWithTag("WeaponSlot").transform.childCount > 0)
            {
                setSpeed(0f);
                animator.SetTrigger("dodgeTrigger");
                Debug.Log("Dodging");
            }

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
            else if (distanceBetweenPlayer <= attackDistance)
            {
                SetState(EnemyState.ATTACK);
            }
            else
            {
                // Normal chasing behavior
                StopAllCoroutines();
                attackDistance = 3f;
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
                StartCoroutine(MoveTo(player.transform.position));
                if (Random.value < 0.001 && distanceBetweenPlayer > attackDistance + 5f) throwAttack();
            }
        }
        private void StartCircling()
        {
            isCircling = true;
            circlingTimer = 10f; // Reset circling duration
            attackCooldown = 2.5f;
            //attackDistance = 7f; // Increase distance to allow circling
            StopAllCoroutines();
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
                newWeapon.transform.Rotate(0f, 80f, -5f, 0);
                newWeapon.GetComponent<Rigidbody>().AddForce(((player.transform.position - transform.position).normalized) * 30f, ForceMode.Impulse);
            }
        }
        public void enableWeapon()
        {
            if (isThrowing)
            {
                GameObject.FindWithTag("EnemyWeapon").GetComponent<Renderer>().enabled = true;
                //Destroy(GameObject.FindWithTag("EnemyWeaponThrowable"));
                GameObject newWeapon = Instantiate(Resources.Load("Orc_Skull_Weapon"), transform) as GameObject;
                isThrowing = false;
            }
        }
        public void jump() 
        {
            setSpeed(0f);
            Debug.Log((transform.position - player.transform.position).normalized);
            gameObject.GetComponent<Rigidbody>().AddForce((Vector3.up + (transform.position - player.transform.position).normalized) * 10f, ForceMode.Impulse);
        }
        public void jumpAttack()
        {
            transform.LookAt(player.transform);
            animator.SetTrigger("jumpTrigger");
        }
        public void onJump()
        {
            animator.SetTrigger("jumpIdleTrigger");
        }
        public void checkNearGround()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f)) 
            { 
                animator.SetTrigger("jumpEndTrigger");
                setSpeed(runSpeed);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Weapon") 
                //&& !(animator.GetAnimatorTransitionInfo(0).IsName("Punch") || animator.GetAnimatorTransitionInfo(0).IsName("Weapon")) 
                //&& GameObject.FindWithTag("WeaponHolder").GetComponent<Animator>().GetAnimatorTransitionInfo(0).IsName("TempSwordAnimation"))
                )
            {
                setSpeed(0f);
                animator.SetTrigger("stunTrigger");
            }
        }
        public void dodge()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Physics.IgnoreLayerCollision(6,7,true);
        }
        public void endDodge()
        {
            Physics.IgnoreLayerCollision(6, 7, false);
            GetComponent<Rigidbody>().isKinematic = false;
            SetState(EnemyState.TRIGGERED);
            resetSpeed();
        }
        public void resetSpeed()
        {
            setSpeed(stats.Speed.GetFlat());
        }
    }
}
