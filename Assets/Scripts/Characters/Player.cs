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

        private void Awake()
        {
            character = GetComponent<Character>();
        }

        private void OnJump()
        {
            character.Jump();
        }

        private void OnAttack()
        {
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

            // flip hitbox
            if (input != 0)
                hitbox.transform.localScale = new Vector2(input, 1);
        }
    }
}