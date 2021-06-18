using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class LifeOrb : MonoBehaviour
    {
        public Rigidbody2D rb;

        public float heal = 20f;

        void onTriggerEnter2D(Collider2D hitInfo)
        {
            Debug.Log("test");
            if (hitInfo.isTrigger) return;

            var player = hitInfo.GetComponent<Player>();
            if (hitInfo.GetComponent<Player>())
            {
                player.GetComponentInParent<Character>().Heal(heal);
                Destroy(gameObject);
            }
        }
    }
}
