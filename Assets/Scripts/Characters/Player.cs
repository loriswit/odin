using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    public class Player : MonoBehaviour
    {
        [Header("Weapon")]
        [SerializeField]
        private Collider2D hitbox;

        [SerializeField]
        private float damage = 40;

        private Character character;

        private Animator animator;
        private static readonly int Attack = Animator.StringToHash("attack");

        private void Awake()
        {
            character = GetComponent<Character>();
            animator = GetComponentInChildren<Animator>();
        }

        private void OnJump()
        {
            character.Jump();
        }

        private void OnAttack()
        {
            animator.SetTrigger(Attack);

            var filter = new ContactFilter2D
            {
                layerMask = LayerMask.GetMask("Hurtbox"),
                useLayerMask = true,
                useTriggers = true
            };
            var targets = new List<Collider2D>();

            if (hitbox.OverlapCollider(filter, targets) < 1) return;

            foreach (var target in targets)
                target.GetComponentInParent<Character>().Hurt(gameObject, damage);
        }

        private void OnMove(InputValue value)
        {
            var input = value.Get<float>();

            if (input > 0)
                character.MoveRight();

            else if (input < 0)
                character.MoveLeft();

            else
                character.StopMoving();

            // if moving
            if (input != 0)
                hitbox.transform.localScale = new Vector2(input, 1);
        }
    }
}
