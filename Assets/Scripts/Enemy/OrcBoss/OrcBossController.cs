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
        bool isCircling;
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
            SetState(EnemyState.IDLE);
            base.Start();
            stats.TriggeredDistance.SetCurrent(100f);
            stats.Speed.SetFlat(runSpeed);
            stats.Life.SetFlat(1000f);
            speed = stats.Speed.GetCurrent();
        }
        protected override void Update()
        {
            distanceBetweenPlayer = Vector3.Distance(transform.position, player.transform.position);
            //Debug.Log("Moving: " + animator.GetBool("isMoving"));
            switch (enemyState)
            {
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

            if ((GetState() == EnemyState.TRIGGERED || GetState() == EnemyState.ATTACK)
                && distanceBetweenPlayer <= attackDistance
                && player.GetComponent<InputManager>().getPlayerInput().grounded.SwordAction.triggered
                && Random.value < 0.3
                && !animator.GetAnimatorTransitionInfo(0).IsName("Dodge")
                && GameObject.FindWithTag("WeaponSlot").transform.childCount > 0)
            {
                setSpeed(0f);
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Collider>().enabled = false;
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

                while (Vector3.Distance(transform.position, targetPosition) > stopDistance)
                {
                    animator.SetBool("isMoving", true);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * stats.Speed.GetCurrent());
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, stats.Speed.GetCurrent() * Time.deltaTime);
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
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * stats.Speed.GetCurrent());
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
            }
        }
        private void StartCircling()
        {
            isCircling = true;
            circlingTimer = 4f; // Reset circling duration
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
                GameObject.FindWithTag("EnemyWeapon").GetComponent<Renderer>().enabled = false;
                GameObject newWeapon = Instantiate(Resources.Load("Orc_Skull_Weapon"), transform) as GameObject;
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
            animator.SetTrigger("jumpTrigger");
        }
        public void onJump()
        {
            animator.SetTrigger("jumpIdleTrigger");
        }
        public void checkNearGround()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 4f)) 
            { 
                animator.SetTrigger("jumpEndTrigger");
                setSpeed(runSpeed);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Weapon") && (GetState() == EnemyState.TRIGGERED || GetState() == EnemyState.ATTACK))
            {
                setSpeed(0f);
                animator.SetTrigger("stunTrigger");
            }
        }
        public void endDodge()
        {
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
            resetSpeed();
        }
        public void resetSpeed()
        {
            setSpeed(stats.Speed.GetFlat());
        }
    }
}
