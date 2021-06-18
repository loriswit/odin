using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class LifeOrb : MonoBehaviour
    {
        public Rigidbody2D rb;

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
