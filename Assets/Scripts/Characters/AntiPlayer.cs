using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class AntiPlayer : MonoBehaviour
    {
        [Header("Weapon")]
        [SerializeField]
        private Collider2D hitbox;

        [SerializeField]
        private float damage = 30;

        private Player player;
        private Character character;
        private Animator animator;

        private static readonly int Attack = Animator.StringToHash("attack");

        private float attackCooldown;
        private const float AttackDelay = 1.5f;

        private float jumpCooldown;
        private const float JumpDelay = 2;

        private bool dead;

        private void Awake()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<Player>();
            animator = GetComponentInChildren<Animator>();
        }

        private void FixedUpdate()
        {
            // check if dead
            if (character.Health <= 0)
            {
                if (!dead)
                {
                    dead = true;
                    character.StopMoving();
                    Destroy(gameObject, 1.5f);
                }

                return;
            }

            attackCooldown -= Time.fixedDeltaTime;
            jumpCooldown -= Time.fixedDeltaTime;

            // movements
            var playerDistanceX = Math.Abs(transform.position.x - player.transform.position.x);
            if (playerDistanceX > 15 || playerDistanceX < 2)
            {
                character.StopMoving();
                jumpCooldown = JumpDelay / 2;
            }
            else if (player.transform.position.x < transform.position.x)
            {
                character.MoveLeft();
                hitbox.transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                character.MoveRight();
                hitbox.transform.localScale = new Vector2(1, 1);
            }

            // jumps
            var playerDistanceY = player.transform.position.y - transform.position.y;
            if (jumpCooldown <= 0 && playerDistanceY > 1.5)
            {
                jumpCooldown = JumpDelay;
                character.Jump();
            }

            // attack
            var playerDistance = Vector2.Distance(player.transform.position, transform.position);
            if (playerDistance <= 3 && attackCooldown <= 0)
            {
                attackCooldown = AttackDelay;
                animator.SetTrigger(Attack);

                var filter = new ContactFilter2D
                {
                    layerMask = LayerMask.GetMask("Character"),
                    useLayerMask = true,
                };
                var targets = new List<Collider2D>();

                if (hitbox.OverlapCollider(filter, targets) > 0)
                    foreach (var target in targets)
                        if (target.name == "Player")
                        {
                            target.GetComponent<Character>().Hurt(gameObject, damage);
                            break;
                        }
            }
        }
    }
}
