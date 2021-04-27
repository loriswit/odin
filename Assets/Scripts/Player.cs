using System;
using System.Linq;
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

    [Header("Collider Material")]
    [SerializeField]
    private PhysicsMaterial2D groundedMaterial;

    [SerializeField]
    private PhysicsMaterial2D midairMaterial;

    private Rigidbody2D body;
    private new CapsuleCollider2D collider;

    private Vector2 velocity;
    private float direction;

    private GameObject ground;
    private bool Grounded => ground != null;

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

        // set appropriate physics material
        collider.sharedMaterial = Grounded ? groundedMaterial : midairMaterial;
    }

    private void OnJump()
    {
        if (Grounded)
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
    }

    private void OnMove(InputValue value)
    {
        // update move direction (-1 = left, 1 = right, 0 = idle)
        direction = value.Get<float>();
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        // remove reference if not ground anymore
        if (Grounded && collision.gameObject == ground && !IsGround(collision))
            ground = null;

        // check for new ground
        else if (!Grounded && IsGround(collision))
            ground = collision.gameObject;

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

    private bool IsGround(Collision2D collision)
    {
        return collision.contacts
            .Select(contact => Vector2.Angle(Vector2.up, contact.normal))
            .Any(angle => angle < maxGroundAngle);
    }
}
