using System;
using UnityEngine;

namespace Characters
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private bool isStatic;

        private Player player;
        private Character character;

        private bool facingLeft = true;

        private void Awake()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<Player>();
        }

        private void FixedUpdate()
        {
            if (character.ReceivingDamage) return;

            if (!isStatic)
            {
                var playerDistance = Math.Abs(transform.position.x - player.transform.position.x);
                if (playerDistance > 10 || playerDistance < 4)
                    character.StopMoving();
                else if (player.transform.position.x < transform.position.x)
                    character.MoveLeft();
                else
                    character.MoveRight();
            }

            if ((player.transform.position.x < transform.position.x && facingLeft == false) ||
                (player.transform.position.x > transform.position.x && facingLeft == true)) {
                	transform.Rotate(0f, 180f, 0f);
					facingLeft = !facingLeft;	
				}
        }

        private void Update()
        {
            // if enemy killed
            if (character.Health <= 0)
                Destroy(gameObject, 0.5f);
        }
    }
}
