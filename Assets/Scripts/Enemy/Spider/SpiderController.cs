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
            xpDrop = 5f;
            goldDrop = 12;
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
            }
        }
    }
}
