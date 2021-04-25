using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float jumpSpeed = 20;

    [SerializeField]
    private float moveSpeed = 10;

    [Header("Ground Detection")]
    [SerializeField]
    private Transform groundLevel;

    [SerializeField]
    private LayerMask groundLayers;

    [Header("Collider Material")]
    [SerializeField]
    private PhysicsMaterial2D groundedMaterial;

    [SerializeField]
    private PhysicsMaterial2D midairMaterial;

    private Rigidbody2D body;
    private new CapsuleCollider2D collider;

    private Vector2 velocity;
    private float direction;
    private bool grounded;

    private const float GroundDetectionRadius = 0.2f;
    private const float MoveSmoothing = 0.05f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        // set speed according to current direction
        var current = body.velocity;
        var target = new Vector2(direction * moveSpeed, current.y);
        body.velocity = Vector2.SmoothDamp(current, target, ref velocity, MoveSmoothing);

        // check whether the player touches the ground
        grounded = Physics2D.OverlapCircle(groundLevel.position, GroundDetectionRadius, groundLayers);
        collider.sharedMaterial = grounded ? groundedMaterial : midairMaterial;
    }

    private void OnJump()
    {
        if (grounded)
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
    }

    private void OnMove(InputValue value)
    {
        // update move direction (-1 = left, 1 = right, 0 = idle)
        direction = value.Get<float>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundLevel.position, GroundDetectionRadius);
    }
}
