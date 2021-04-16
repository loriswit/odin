using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float jumpSpeed = 20;

    [SerializeField]
    private float moveSpeed = 10;

    [SerializeField]
    private Transform groundLevel;

    [SerializeField]
    private LayerMask groundLayers;

    private Rigidbody2D body;
    private float direction;
    private bool grounded;
    
    private const float GroundDetectionRadius = 0.2f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // set speed according to current direction
        body.velocity = new Vector2(direction * moveSpeed, body.velocity.y);

        // check whether the player touches the ground
        grounded = Physics2D.OverlapCircle(groundLevel.position, GroundDetectionRadius, groundLayers);
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
