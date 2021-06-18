using System;
using Characters;
using UnityEngine;

namespace Utils
{
    public class Weapon : MonoBehaviour
    {
        public Transform firePoint;
        public GameObject projectilePrefab;
        public float shootRate = 3;

        private float coolDown;
        private Character character;

        private void Awake()
        {
            coolDown = shootRate / 3;
            character = GetComponent<Character>();
        }

        void FixedUpdate()
        {
            var player = FindObjectOfType<Player>();
            var playerDistance = Math.Abs(transform.position.x - player.transform.position.x);
            coolDown -= Time.fixedDeltaTime;

            if (coolDown > 0 || playerDistance > 7 || character.ReceivingDamage || character.Health <= 0)
                return;

            Shoot();
            coolDown = shootRate;
        }

        void Shoot()
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }
}
