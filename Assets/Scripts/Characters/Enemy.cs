using System;
using UnityEngine;

namespace Characters
{
    public class Enemy : MonoBehaviour
    {
        private Player player;
        private Character character;

        private void Awake()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<Player>();
        }

        private void FixedUpdate()
        {
            var playerDistance = Math.Abs(transform.position.x - player.transform.position.x);
            if (playerDistance > 20 || playerDistance < 1.5)
                character.StopMoving();

            else if (player.transform.position.x < transform.position.x)
                character.MoveLeft();

            else
                character.MoveRight();
        }

        private void Update()
        {
            // if enemy killed
            if (character.Health <= 0)
                Destroy(gameObject);
        }
    }
}
