using UnityEngine;


namespace Weapons
{
    public class AxeReturn : MonoBehaviour
    {
        private Collider weaponCollider;
        private GameObject player;
        public bool returning = false;

        private void Awake()
        {
            player = GameObject.Find("PlayerObject");
            weaponCollider = GetComponent<Collider>();
        }

        private void FixedUpdate()
        {
            if (returning)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 70f * Time.deltaTime);
                if(Vector3.Distance(transform.position, player.transform.position) < 2f)
                {
                    ToggleReturn();
                    player.GetComponent<ThrowableAxe>().DestroyAxe();
                    
                }
            }
        }

        public void ToggleReturn()
        {
            returning = !returning;
        }
    }
}