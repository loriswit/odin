using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        [Header("Character Stats")]
        [SerializeField]
        private float jumpSpeed = 20;

        [SerializeField]
        private float moveSpeed = 10;

        [SerializeField]
        private float health = 100;

        [Header("Ground Detection")]
        [Range(0f, 90f)]
        [SerializeField]
        private float maxGroundAngle = 45;

        [Header("Physics materials")]
        [SerializeField]
        private PhysicsMaterial2D lowFriction;

        [SerializeField]
        private PhysicsMaterial2D highFriction;

        [Header("Graphics")]
        [SerializeField]
        private GameObject sprite;

        private Rigidbody2D body;

        private float gravityScale;
        private Vector2 velocity;
        private Direction direction;
        private float jumpCooldown;
        private float hurtCooldown;

        private GameObject ground;
        private Vector2 groundNormal;

        private const float MoveSmoothing = 0.05f;
        private const float JumpDelay = 0.1f;
        private const float HurtDelay = 0.5f;

        private Animator animator;
        private static readonly int GroundedId = Animator.StringToHash("grounded");
        private static readonly int JumpId = Animator.StringToHash("jump");
        private static readonly int RunningId = Animator.StringToHash("running");
        private static readonly int LandId = Animator.StringToHash("land");
        private static readonly int HurtId = Animator.StringToHash("hurt");
        private static readonly int DieId = Animator.StringToHash("die");

        /**
         * True whenever the player is on the ground.
         */
        private bool Grounded => ground != null;

        /**
         * True whenever the player is in the air.
         * Also true during cooldown to prevents ground-related behaviours right after jumping.
         */
        private bool MidAir => !Grounded || jumpCooldown > 0;

        /**
         * Return the current health points.
         */
        public float Health => health;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();

            // disable default gravity
            gravityScale = body.gravityScale;
            body.gravityScale = 0;

            body.sharedMaterial = lowFriction;

            animator = GetComponentInChildren<Animator>();
        }

        public void Jump()
        {
            if (MidAir) return;

            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            jumpCooldown = JumpDelay;

            animator?.ResetTrigger(LandId);
            animator?.SetTrigger(JumpId);
        }

        public void MoveRight()
        {
            direction = Direction.Right;

            // remove friction when moving
            body.sharedMaterial = lowFriction;

            animator?.SetBool(RunningId, true);
            if (sprite)
                sprite.transform.localScale = new Vector2(1, 1);
        }

        public void MoveLeft()
        {
            direction = Direction.Left;

            // remove friction when moving
            body.sharedMaterial = lowFriction;

            animator?.SetBool(RunningId, true);
            if (sprite)
                sprite.transform.localScale = new Vector2(-1, 1);
        }

        public void StopMoving()
        {
            direction = Direction.Idle;
            animator?.SetBool(RunningId, false);
        }

        public void Hurt(GameObject source, float damage)
        {
            if (hurtCooldown > 0 || health <= 0) return;

            health -= damage;
            hurtCooldown = HurtDelay;

            // make the character bump
            var hitDirection = (transform.position - source.transform.position).normalized;
            body.velocity = hitDirection * 20 + new Vector3(0, Grounded ? 10 : 2, 0);

            animator?.SetTrigger(health > 0 ? HurtId : DieId);
        }

        private void FixedUpdate()
        {
            // decrease cooldowns
            jumpCooldown -= Time.fixedDeltaTime;
            hurtCooldown -= Time.fixedDeltaTime;

            Vector2 targetVelocity;
            Vector2 gravity;

            // prevent moving when receiving damage
            var speed = hurtCooldown > 0 ? 0 : (float) direction * moveSpeed;

            // when mid-air, move horizontally and use default gravity
            if (MidAir)
            {
                targetVelocity = new Vector2(speed, body.velocity.y);
                gravity = Physics.gravity;
            }

            // when on ground, define velocity and gravity according to slope angle
            // this makes walking on slopes more smooth, as if the ground was always horizontal
            else
            {
                targetVelocity = Vector2.Perpendicular(groundNormal) * speed;
                gravity = groundNormal * Physics.gravity.magnitude;
            }

            body.AddForce(gravity * (gravityScale * body.mass));
            body.velocity = Vector2.SmoothDamp(body.velocity, targetVelocity, ref velocity, MoveSmoothing);

            if (sprite)
                sprite.transform.localRotation = Quaternion.FromToRotation(Vector2.down, groundNormal);

            // draw gravity and velocity vectors
            Debug.DrawRay(body.position, gravity, Color.white);
            Debug.DrawRay(body.position, targetVelocity, Color.grey);
            Debug.DrawRay(body.position, body.velocity, Color.black);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var wasGrounded = Grounded;

            // check if colliding with ground
            CheckGround(collision);

            // if landing
            if (!wasGrounded && Grounded)
            {
                animator?.ResetTrigger(JumpId);
                animator?.SetTrigger(LandId);

                // increase friction when landing without moving
                // prevents sliding on slope when landing
                if (direction == Direction.Idle)
                    body.sharedMaterial = highFriction;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            // check if still colliding with ground
            // contact points and angles may have changed
            CheckGround(collision);

            // remove friction if not grounded anymore
            if (!Grounded)
                body.sharedMaterial = lowFriction;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            // leave current ground
            if (Grounded && collision.gameObject == ground)
            {
                ground = null;
                groundNormal = Vector2.zero;
                animator?.SetBool(GroundedId, false);
            }
        }

        private void CheckGround(Collision2D collision)
        {
            // reset if current ground
            if (collision.gameObject == ground)
            {
                ground = null;
                groundNormal = Vector2.zero;
            }

            foreach (var contact in collision.contacts)
            {
                // check if slope isn't too steep
                if (Vector2.Angle(Vector2.up, contact.normal) <= maxGroundAngle)
                {
                    if (!Grounded)
                    {
                        ground = collision.gameObject;
                        groundNormal = -contact.normal;
                    }

                    Debug.DrawRay(contact.point, contact.normal,
                        body.sharedMaterial == highFriction ? Color.blue : Color.green);
                }
                else
                    Debug.DrawRay(contact.point, contact.normal, Color.red);
            }

            animator?.SetBool(GroundedId, Grounded);
        }
    }
}
