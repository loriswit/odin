using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float jumpSpeed = 20;

    [SerializeField]
    private float moveSpeed = 10;

    [Header("Ground Detection")]
    [Range(0f, 90f)]
    [SerializeField]
    private float maxGroundAngle = 45;

    private Rigidbody2D body;

    private float gravityScale;
    private Vector2 velocity;
    private float direction;
    private float jumpCooldown;

    private GameObject ground;
    private Vector2 groundNormal;

    private const float MoveSmoothing = 0.05f;
    private const float JumpDelay = 0.1f;

    /**
     * True whenever the player is on the ground.
     */
    private bool Grounded => ground != null;

    /**
     * True whenever the player is in the air.
     * Also true during cooldown to prevents ground-related behaviours right after jumping.
     */
    private bool MidAir => !Grounded || jumpCooldown > 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

        // disable default gravity
        gravityScale = body.gravityScale;
        body.gravityScale = 0;
    }

    /**
     * Physics-related updates.
     */
    private void FixedUpdate()
    {
        // decrease cooldown
        jumpCooldown -= Time.fixedDeltaTime;

        Vector2 targetVelocity;
        Vector2 gravity;

        // when mid-air, move horizontally and use default gravity
        if (MidAir)
        {
            targetVelocity = new Vector2(direction * moveSpeed, body.velocity.y);
            gravity = Physics.gravity;
        }

        // when on ground, define velocity and gravity according to slope angle
        // this makes walking on slopes more smooth, as if the ground was always horizontal
        else
        {
            targetVelocity = Vector2.Perpendicular(groundNormal) * (direction * moveSpeed);
            gravity = groundNormal * Physics.gravity.magnitude;
        }

        body.AddForce(gravity * (gravityScale * body.mass));
        body.velocity = Vector2.SmoothDamp(body.velocity, targetVelocity, ref velocity, MoveSmoothing);

        // draw gravity and velocity vectors
        Debug.DrawRay(body.position, gravity, Color.white);
        Debug.DrawRay(body.position, targetVelocity, Color.grey);
        Debug.DrawRay(body.position, body.velocity, Color.black);
    }

    private void OnJump()
    {
        if (MidAir) return;

        body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        jumpCooldown = JumpDelay;
    }

    private void OnMove(InputValue value)
    {
        // update move direction (-1 = left, 1 = right, 0 = idle)
        direction = value.Get<float>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // check if colliding with ground
        if (!Grounded || Grounded && collision.gameObject == ground)
        {
            ground = null;
            groundNormal = Vector2.zero;

            foreach (var contact in collision.contacts)
            {
                var angle = Vector2.Angle(Vector2.up, contact.normal);
                if (angle > maxGroundAngle) continue;

                ground = collision.gameObject;
                groundNormal = -contact.normal;
                break;
            }
        }

        // draw contact points
        foreach (var contact in collision.contacts)
            Debug.DrawRay(contact.point, contact.normal,
                Color.HSVToRGB(Math.Abs(collision.collider.GetInstanceID()) * 0.37f % 1f, 1, 1));
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // leave ground
        if (Grounded && collision.gameObject == ground)
            ground = null;
    }
}
