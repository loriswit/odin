using System;
using UnityEngine;

namespace Characters
{
    public class Weapon : MonoBehaviour
    {
        public Transform firePoint;
        public GameObject projectilePrefab;
        public float coolDown = 3f;

        void FixedUpdate()
        {
            Player player = FindObjectOfType<Player>();
            var playerDistance = Math.Abs(transform.position.x - player.transform.position.x);
            coolDown -= Time.fixedDeltaTime;
            if (coolDown > 0 || playerDistance > 20) return;

            Shoot();
            coolDown = 3f;
        }

        void Shoot()
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }
}
