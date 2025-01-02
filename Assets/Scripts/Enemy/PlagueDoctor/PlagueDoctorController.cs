using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class PlagueDoctorController : AbstractEnemy
    {
        [SerializeField] float health;
        public bool attacking;
        public float shootCooldown;
        public float teleportCooldown;
        public float summonCooldown;
        public bool summoning;
        public int summonCount; 
        protected override void Awake()
        {
            base.Awake();
            xpDrop = 1000f;
            goldDrop = 1000;
            attackDistance = 15f;
            stats.Damage.SetFlat(45f);
            stats.TriggeredDistance.SetFlat(30f);
            shootCooldown = 1f;
            teleportCooldown = 5f;
            summonCooldown = 20f;
            summonCount = 0;
            attacking = false;
            summoning = false;
            SetState(EnemyState.IDLE);
        }
        protected override void Start()
        {
            baseSpeed = 4f;
            runSpeed = 4f;
            base.Start();
        }
        protected override void Update()
        {
            if ((GetState() != EnemyState.IDLE))
            {
                if (GameObject.Find("--Enemy") != null) { GameObject.Find("--Enemy").SetActive(false); }
                if (!summoning)
                {
                    distanceBetweenPlayer = Vector3.Distance(transform.position, player.transform.position);
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
                    if (shootCooldown <= 0)
                    {
                        SetState(EnemyState.ATTACK);
                        shootCooldown = 5f;
                    }
                    else { shootCooldown -= Time.deltaTime; }
                    if (teleportCooldown <= 0)
                    {
                        teleport();
                        teleportCooldown = 5f;
                    }
                    else if (teleportCooldown <= 1f) { if (GetComponent<ParticleSystem>() != null) GetComponent<ParticleSystem>().Play(); teleportCooldown -= Time.deltaTime; }
                    else { teleportCooldown -= Time.deltaTime; }

                    if (summonCooldown <= 0f)
                    {
                        summoning = true;
                        summonCount += 1;
                        summon();
                    }
                    else if (summonCooldown <= 1f)
                    {
                        if (GetComponent<ParticleSystem>() != null) GetComponent<ParticleSystem>().Play();
                        summonCooldown -= Time.deltaTime;
                        animator.SetTrigger("summonTrigger");
                    }
                    else { summonCooldown -= Time.deltaTime; }
                }
                else
                {
                    if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
                    {
                        summoning = false;
                        stats.Life.SetAdded(stats.Life.GetAdded() + stats.Life.GetFlat() / 4 * -1);
                        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
                    }
                }
            }
        }
        public override void idle() { }
        public override void followPlayer()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else
            {
                transform.LookAt(player.transform.position);
                animator.SetBool("isMoving", false);
            }
        }
        public override void attack()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else
            {
                StopAllCoroutines();
                shootAttack();
            }
        }
        public void shootAttack()
        {
            animator.SetTrigger("shootTrigger");
            attacking = true;
            shootCooldown = 1f;
        }
        public void shootBall()
        {
            GameObject newWeapon = Instantiate(Resources.Load("DeathBall"), transform) as GameObject;
            newWeapon.GetComponent<Renderer>().enabled = true;
            newWeapon.GetComponent<Rigidbody>().isKinematic = false;
            newWeapon.GetComponent<Rigidbody>().AddForce(((player.transform.position - transform.position).normalized) * 20f, ForceMode.Impulse);
        }
        public override void onAttackComplete()
        {
            base.onAttackComplete();
            attacking = false;
        }
        public void teleport()
        {
            float randomX; float randomZ;
            if (Random.value < 0.5) randomX = Random.Range(-10f, -5f);
            else randomX = Random.Range(5f, 10f);
            if (Random.value < 0.5) randomZ = Random.Range(-10f, -5f);
            else randomZ = Random.Range(5f, 10f);
            transform.position = new Vector3(player.transform.position.x + randomX, 1f, player.transform.position.z + randomZ);
            Debug.Log("Teleporting");
        }
        public void summonAnimEvent()
        {
            transform.position = new Vector3(transform.position.x, -100f, transform.position.z);
        }
        public void summon()
        {
            Debug.Log("Summoning");
            summonCooldown = 20f;
            float randomX; float randomZ;
            switch (summonCount)
            {
                case 1:
                    for (int i = 0; i <= 2; i++)
                    {
                        if (Random.value < 0.5) randomX = Random.Range(-10f, -5f);
                        else randomX = Random.Range(5f, 10f);
                        if (Random.value < 0.5) randomZ = Random.Range(-10f, -5f);
                        else randomZ = Random.Range(5f, 10f);
                        Instantiate(Resources.Load("SkeleMelee"), new Vector3(player.transform.position.x + randomX, 2f, player.transform.position.z + randomZ), transform.localRotation);
                    }
                    break;
                case 2:
                    for (int i = 0; i <= 2; i++)
                    {
                        if (Random.value < 0.5) randomX = Random.Range(-10f, -5f);
                        else randomX = Random.Range(5f, 10f);
                        if (Random.value < 0.5) randomZ = Random.Range(-10f, -5f);
                        else randomZ = Random.Range(5f, 10f);
                        Instantiate(Resources.Load("SkeleRanged"), new Vector3(player.transform.position.x + randomX, 2f, player.transform.position.z + randomZ), transform.localRotation);
                    }
                    break;
                case 3:
                    for (int i = 0; i <= 2; i++)
                    {
                        if (Random.value < 0.5) randomX = Random.Range(-10f, -5f);
                        else randomX = Random.Range(5f, 10f);
                        if (Random.value < 0.5) randomZ = Random.Range(-10f, -5f);
                        else randomZ = Random.Range(5f, 10f);
                        Instantiate(Resources.Load("SkeleMage"), new Vector3(player.transform.position.x + randomX, 2f, player.transform.position.z + randomZ), transform.localRotation);
                    }
                    break;
                case 4:
                    for (int i = 0; i <= 1; i++)
                    {
                        if (Random.value < 0.5) randomX = Random.Range(-10f, -5f);
                        else randomX = Random.Range(5f, 10f);
                        if (Random.value < 0.5) randomZ = Random.Range(-10f, -5f);
                        else randomZ = Random.Range(5f, 10f);
                        Instantiate(Resources.Load("SkeleMelee"), new Vector3(player.transform.position.x + randomX, 2f, player.transform.position.z + randomZ), transform.localRotation);
                    }
                    for (int i = 0; i <= 1; i++)
                    {
                        if (Random.value < 0.5) randomX = Random.Range(-10f, -5f);
                        else randomX = Random.Range(5f, 10f);
                        if (Random.value < 0.5) randomZ = Random.Range(-10f, -5f);
                        else randomZ = Random.Range(5f, 10f);
                        Instantiate(Resources.Load("SkeleRanged"), new Vector3(player.transform.position.x + randomX, 2f, player.transform.position.z + randomZ), transform.localRotation);
                    }
                    for (int i = 0; i <= 1; i++)
                    {
                        if (Random.value < 0.5) randomX = Random.Range(-10f, -5f);
                        else randomX = Random.Range(5f, 10f);
                        if (Random.value < 0.5) randomZ = Random.Range(-10f, -5f);
                        else randomZ = Random.Range(5f, 10f);
                        Instantiate(Resources.Load("SkeleMage"), new Vector3(player.transform.position.x + randomX, 2f, player.transform.position.z + randomZ), transform.localRotation);
                    }
                    break;
            }
        }
    }
}