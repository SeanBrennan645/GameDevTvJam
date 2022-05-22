using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float timeBetweenJumps = 0.5f;
    [SerializeField] int extraJumps = 1;
    [SerializeField] Collider2D feet;

    public bool isActive = true;

    private Vector2 moveDirection;
    private Vector2 rawInput;
    private int jumpCount;
    private bool isJumping;
    private Rigidbody2D rb;


    const string platformLayer = "Platform";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = 0;
    }

    void FixedUpdate()
    {
        //Move the player
        rb.velocity = new Vector2(rawInput.x * moveSpeed, rb.velocity.y);

        //Make the player jump
        if (isJumping)
        {
            isJumping = false;
        }

        if(jumpCount > 0)
        {
            if (feet.IsTouchingLayers(LayerMask.GetMask(platformLayer)))
            {
                jumpCount = 0;
            }
        }
    }

    //Used by the input system 
    void OnMove(InputValue value)
    {
        if (!isActive) { return; }
        rawInput = value.Get<Vector2>();
    }

    //Used by the input system
    IEnumerator OnJump(InputValue value)
    {
        if (!isActive) { yield break; }
        if(!isJumping && jumpCount < extraJumps)
        {
            isJumping = true;
            jumpCount++;

            rb.velocity = new Vector2(0.0f, 0.0f);
            rb.velocity += new Vector2(0f, jumpForce);

            yield return new WaitForSeconds(timeBetweenJumps);
        }
    }
}