using UnityEngine;


namespace Weapons
{
    public class AxeReturn : MonoBehaviour
    {
        private Collider weaponCollider;
        private GameObject player;
        private bool returning = false;

        private void Awake()
        {
            player = GameObject.Find("PlayerObject");
            weaponCollider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (returning)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 70f * Time.deltaTime);
                if(Vector3.Distance(transform.position, player.transform.position) < 2f)
                {
                    player.GetComponent<ThrowableAxe>().DestroyAxe();
                    ToggleReturn();
                }
            }
        }

        public void ToggleReturn()
        {
            returning = !returning;
        }
    }
}