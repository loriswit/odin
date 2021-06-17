using UnityEngine;

namespace Characters
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 20f;
        public Rigidbody2D rb;
        public float damage = 20f;

        void Start()
        {
            rb.velocity = transform.right * speed;
        }

        void OnTriggerEnter2D(Collider2D hitInfo)
        {
            if (hitInfo.isTrigger) return;

            var player = hitInfo.GetComponent<Player>();
            if (hitInfo.GetComponent<Player>())
            {
                player.GetComponentInParent<Character>().Hurt(gameObject, damage);
                Destroy(gameObject);
            }

            // go through other characters
            else if (!hitInfo.GetComponent<Character>())
                Destroy(gameObject);
        }
    }
}
