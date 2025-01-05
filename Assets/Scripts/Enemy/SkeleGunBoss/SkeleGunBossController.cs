using GameplayMechanics.Character;
using InventoryScripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;

namespace Enemy
{
    public class SkeleGunBossController : AbstractEnemy
    {
        public float shootCooldown;
        public float teleportCooldown;
        public float afterImageCooldown;
        public float afterImageCountdown;
        public bool afterImage;
        public bool attacking;
        public Vector3[] teleportPoints;
        public float health;
        protected override void Awake()
        {
            base.Awake();
            xpDrop = 100f;
            goldDrop = 100;
            attackDistance = 5f;
            attackCooldown = 5f;
            teleportCooldown = 10f;
            afterImageCooldown = 20f;
            afterImageCountdown = 20f;
            afterImage = false;
            attacking = false;
            stats.Life.SetFlat(750f);
            stats.Damage.SetFlat(15f);
            SetState(EnemyState.IDLE);
            attackPattern.Add(shootAttack);
        }
        protected override void Start()
        {
            baseSpeed = 1f;
            runSpeed = 1f;
            base.Start();
        }
        protected override void Update()
        {
            health = stats.Life.GetCurrent();
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            if (stats.Life.GetCurrent() <= 0)
            {
                SetState(EnemyState.DEAD);
            }
            if (afterImage)
            {
                transform.LookAt(player.transform);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Draw")
                {
                    animator.SetTrigger("drawTrigger");
                }
                if (afterImageCountdown <= 1.5f)
                {
                    { if (GetComponent<ParticleSystem>()) GetComponent<ParticleSystem>().Play(); }
                }
                if (afterImageCountdown <= 0f)
                {
                    Destroy(gameObject);
                }

                afterImageCountdown -= Time.deltaTime;
            }
            else
            {
                if (animator.GetAnimatorTransitionInfo(0).IsName("Shoot")
                    || animator.GetAnimatorTransitionInfo(0).IsName("Ready")
                    || animator.GetAnimatorTransitionInfo(0).IsName("Draw")
                    || animator.GetAnimatorTransitionInfo(0).IsName("Stun"))
                setSpeed(0f);
                if (afterImageCooldown > 0)
                {
                    base.Update();
                    if (!(GetState() == EnemyState.IDLE))
                    {
                        if (attackCooldown <= 0) { attackCooldown = 5f; SetState(EnemyState.ATTACK); }
                        else { SetState(EnemyState.TRIGGERED); attackCooldown -= Time.deltaTime; }
                        float dodgeChance = Random.value;
                        if (teleportCooldown <= 2f || afterImageCooldown <= 2f || (distanceBetweenPlayer <= attackDistance + 5f && dodgeChance <= 0.2f))
                        { if (GetComponent<ParticleSystem>()) GetComponent<ParticleSystem>().Play(); }
                        if ((teleportCooldown <= 0 || (distanceBetweenPlayer <= attackDistance - 0.2f && dodgeChance < 0.2f)) && GetComponent<ParticleSystem>().isPlaying)
                        {
                            audioSource.spatialBlend = 1f;
                            audioSource.loop = false;
                            audioSource.clip = Resources.Load("Audio/Cast") as AudioClip;
                            if (!audioSource.isPlaying) { audioSource.Play(); }
                            teleportCooldown = 20f;
                            float randomX; float randomZ;
                            if (Random.value < 0.5) randomX = Random.Range(-15f, -10f);
                            else randomX = Random.Range(10f, 15f);
                            if (Random.value < 0.5) randomZ = Random.Range(-15f, -10f);
                            else randomZ = Random.Range(10f, 15f);
                            transform.position = new Vector3(player.transform.position.x + randomX, 0f, player.transform.position.z + randomZ);
                            Debug.Log("Teleporting");
                        }
                        else { teleportCooldown -= Time.deltaTime; }
                        afterImageCooldown -= Time.deltaTime;
                    }
                }
                else
                {
                    if (!attacking)
                    {
                        if (stats.Life.GetCurrent() <= stats.Life.GetFlat() / 2) afterImageAttack(8);
                        else afterImageAttack(4);
                    }
                    else
                    {
                        if (GameObject.FindGameObjectsWithTag("Boss").Length == 1)
                        {
                            animator.SetBool("isMoving", true);
                        }
                        if (afterImageCountdown <= 1.5f)
                        {
                            foreach (GameObject boss in GameObject.FindGameObjectsWithTag("Boss"))
                            {
                                gameObject.GetComponent<ParticleSystem>().Play();
                            }
                        }
                        if (afterImageCountdown <= 0f)
                        {
                            foreach (GameObject boss in GameObject.FindGameObjectsWithTag("Boss"))
                            {
                                if (boss.GetComponent<SkeleGunBossController>().afterImage) Destroy(boss);
                            }
                            afterImageCooldown = Random.Range(10f, 20f);
                            afterImageCountdown = 7f;
                            attacking = false;
                            animator.SetTrigger("shootTrigger");
                            animator.SetBool("isMoving", true);
                            playerStats.TakeDamage(stats.Damage.GetCurrent() * GameObject.FindGameObjectsWithTag("Boss").Length);
                        }
                        else
                        {
                            afterImageCountdown -= Time.deltaTime;
                        }
                    }
                }
            }
        }
        public override void idle() { if (GetComponent<ParticleSystem>() != null) { GetComponent<ParticleSystem>().Play(); } }
        public override void followPlayer()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else
            {
                transform.LookAt(player.transform);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                setSpeed(runSpeed);
                StopAllCoroutines();
                animator.SetBool("isMoving", true);
                StartCoroutine(MoveTo(player.transform.position));
            }
        }
        public void walkAudio()
        {
            audioSource.spatialBlend = 0f;
            audioSource.loop = false;
            audioSource.clip = Resources.Load("Audio/Level2BossWalk") as AudioClip;
            if (!audioSource.isPlaying) { audioSource.Play(); }
        }
        public override void attack()
        {
            if (stats.Life.GetCurrent() <= 0) SetState(EnemyState.DEAD);
            else
            {
                StopAllCoroutines();
                animator.SetBool("isMoving", true);
                int attack = Random.Range(0, attackPattern.Count);
                attackPattern[attack].Invoke();
            }
        }
        public void shootAttack()
        {
            animator.SetTrigger("drawTrigger");
        }
        public void checkAfterImages()
        {
            if (!attacking) 
            {
                animator.SetTrigger("shootTrigger");
            }
        }
        public void shootPlayer()
        {
            Debug.Log(Vector3.Angle(transform.forward, transform.position - player.transform.position));
            if (Vector3.Angle(transform.forward, transform.position - player.transform.position) >= 177.25f
                && Vector3.Angle(transform.forward, transform.position - player.transform.position) <= 182.75f)
            {
                playerStats.TakeDamage(stats.Damage.GetCurrent());
            }
            audioSource.spatialBlend = 0f;
            audioSource.loop = false;
            audioSource.clip = Resources.Load("Audio/ShootGun") as AudioClip;
            audioSource.Play();
        }
        public void afterImageAttack(float afterimages)
        {
            attacking = true;
            afterImage = false;
            StopAllCoroutines();
            animator.SetBool("isMoving", false);
            animator.SetTrigger("drawTrigger");
            float randomX;
            float randomZ;
            if (Random.value < 0.5) randomX = Random.Range(-15f, -10f);
            else randomX = Random.Range(10f, 15f);
            if (Random.value < 0.5) randomZ = Random.Range(-15f, -10f);
            else randomZ = Random.Range(10f, 15f);
            transform.position = new Vector3(player.transform.position.x + randomX, 0f, player.transform.position.z + randomZ);
            { if (GetComponent<ParticleSystem>()) GetComponent<ParticleSystem>().Play(); }
            setSpeed(0f);
            for (int i = 0; i <= afterimages; i++)
            {
                if (Random.value < 0.5) randomX = Random.Range(-15f, -10f);
                else randomX = Random.Range(10f, 15f);
                if (Random.value < 0.5) randomZ = Random.Range(-15f, -10f);
                else randomZ = Random.Range(10f, 15f);
                GameObject clone = Instantiate(Resources.Load("SkeleGunBoss"), new Vector3(player.transform.position.x + randomX, 0f, player.transform.position.z + randomZ), transform.rotation, transform.parent) as GameObject;
                clone.GetComponent<SkeleGunBossController>().afterImage = true;
                clone.GetComponent<SkeleGunBossController>().attacking = true;
                clone.GetComponent<SkeleGunBossController>().StopAllCoroutines();
                clone.GetComponent<SkeleGunBossController>().stats.Life.SetFlat(1f);
                clone.GetComponent<SkeleGunBossController>().afterImageCooldown = -10f;
                clone.GetComponent<Animator>().SetTrigger("readyTrigger");
            }
            audioSource.spatialBlend = 1f;
            audioSource.loop = false;
            audioSource.clip = Resources.Load("Audio/Cast") as AudioClip;
            if (!audioSource.isPlaying) { audioSource.Play(); }
        }
        public override void deathAudio()
        {
            if (!afterImage)
            { base.deathAudio(); }
            else
            {
                audioSource.spatialBlend = 1f;
                audioSource.loop = false;
                audioSource.clip = Resources.Load("Audio/Cast") as AudioClip;
                if (!audioSource.isPlaying) { audioSource.Play(); }
            }
        }
        public override void destroySelf()
        {
            if (GameObject.Find("Portal") != null && !afterImage)
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
            if (!afterImage)
            {
                if (XpManager.Instance != null) { XpManager.GiveXp(xpDrop); }
                if (Inventory.Instance != null) { Inventory.GiveGold(goldDrop); }
                if (GameObject.Find("-- Enemy") != null && GameObject.Find("-- Enemy").GetComponent<AudioSource>() != null && GameObject.Find("BossGate").GetComponent<AudioSource>() != null)
                {
                    GameObject.Find("-- Enemy").GetComponent<AudioSource>().Play();
                    GameObject.Find("BossGate").GetComponent<AudioSource>().Pause();
                }
            }
            Destroy(gameObject);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Weapon"))
            {
                animator.SetTrigger("stunTrigger");
                animator.SetBool("isMoving", true);
                SetState(EnemyState.TRIGGERED);
                audioSource.spatialBlend = 0f;
                audioSource.loop = false;
                audioSource.clip = Resources.Load("Audio/Cast") as AudioClip;
                audioSource.Play();
                if (afterImage)
                {
                    animator.SetTrigger("deathTrigger");
                }
                else
                {
                    Debug.Log("Main Boss Attacked");
                    playerStats.DoDamage(this);
                    if (stats.Life.GetCurrent() <= 0)
                    {
                        animator.SetTrigger("deathTrigger");
                    }
                    else
                    {
                        setSpeed(0f);
                        gameObject.GetComponent<Rigidbody>().AddForce((Vector3.back) * 2f, ForceMode.Impulse);
                        if (attacking)
                        {
                            afterImageCooldown = Random.Range(10f, 20f);
                            afterImageCountdown = 7f;
                            attacking = false;
                        }
                    }
                    foreach (GameObject boss in GameObject.FindGameObjectsWithTag("Boss"))
                    {
                        if (boss.GetComponent<SkeleGunBossController>().afterImage)
                        { boss.GetComponent<Animator>().SetTrigger("deathTrigger"); }
                    }
                }
            }
        }
    }
}
