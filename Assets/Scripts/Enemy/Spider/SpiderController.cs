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

            attackDistance = 3f;
            attackCooldown = 1f;
            attackPattern.Add(biteAttack);
        }
        protected override void Start()
        {
            baseSpeed = 4f;
            runSpeed = 4f;
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
                //PlayerStatManager.Instance.DoDamage(this);
                //PlayerStatManager.Instance.DoDamage(enemy);
                setSpeed(0f);
                gameObject.GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.up) * 2f, ForceMode.Impulse);
                animator.SetTrigger("stunTrigger");
            }
        }
    }
}
