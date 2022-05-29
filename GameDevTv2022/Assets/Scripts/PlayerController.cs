using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float wallJumpForce = 5f;
    [SerializeField] float timeBetweenJumps = 0.5f;
    [SerializeField] int extraJumps = 1;
    [Header("Colliders")]
    [SerializeField] Collider2D feet;
    [SerializeField] Collider2D side;
    [Header("Other")]
    [SerializeField] Vector3 StartPosition;
    [SerializeField] ObjectPool Pool;
    //Audios
    [Header("SFX")]
    [SerializeField] AudioClip coinAudio;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] AudioClip jumpAudio;

    public bool isActive = true;

    private Vector2 moveDirection;
    private Vector2 rawInput;
    private int jumpCount;
    private bool isJumping;
    private bool inAir;
    private Rigidbody2D rb;
    private AudioSource audioSource;


    const string platformLayer = "Platform";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = 0;
        inAir = false;
        audioSource = GetComponent<AudioSource>();
        StartPosition = transform.position;
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
                inAir = false;
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
            inAir = true;

            //if (side.IsTouchingLayers(LayerMask.GetMask(platformLayer)))
            //{
            //    Debug.Log("We Here?");
            //    rb.velocity = new Vector2(0.0f, 0.0f);
            //    Vector2 momentum = new Vector2(wallJumpForce, jumpForce);
            //    Debug.Log(momentum);
            //    rb.velocity += new Vector2(wallJumpForce, jumpForce);
            //    audioSource.PlayOneShot(jumpAudio);
            //    yield break;
            //}
            //else
            //{
            //    Debug.Log("Or Here?");
            //    jumpCount++;

            //    rb.velocity = new Vector2(0.0f, 0.0f);
            //    rb.velocity += new Vector2(0f, jumpForce);

            //    audioSource.PlayOneShot(jumpAudio);

            //    yield return new WaitForSeconds(timeBetweenJumps);
            //}
            jumpCount++;

            rb.velocity = new Vector2(0.0f, 0.0f);
            rb.velocity += new Vector2(0f, jumpForce);

            audioSource.PlayOneShot(jumpAudio);

            yield return new WaitForSeconds(timeBetweenJumps);
        }
    }

    void OnDie(InputValue value)
    {
        if (!isActive) { return; }
        RefreshPlayer();
    }

    void OnReset(InputValue value)
    {
        if (!isActive) { return; }
        LevelManager.Instance.RefreshLevel();
    }

    public void RefreshPlayer()
    {
        SpawnPlayerTile();
        audioSource.PlayOneShot(deathAudio);
        transform.position = StartPosition;
    }

    public void SpawnPlayerTile()
    {
        Pool.EnableObjectInPool(transform.position);
    }

    public void PlayCoinSFX()
    {
        audioSource.PlayOneShot(coinAudio);
    }
}
