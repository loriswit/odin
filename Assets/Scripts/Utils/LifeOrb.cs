using Characters;
using UnityEngine;

namespace Utils
{
    public class LifeOrb : MonoBehaviour
    {
        public float heal = 20f;

        void OnTriggerEnter2D(Collider2D hitInfo)
        {
            if (hitInfo.isTrigger) return;

            var player = hitInfo.GetComponent<Player>();
            if (hitInfo.GetComponent<Player>())
            {
                bool isHealed = player.GetComponentInParent<Character>().Heal(heal);
                if (isHealed)
                    Destroy(gameObject);
            }
        }
    }
}
