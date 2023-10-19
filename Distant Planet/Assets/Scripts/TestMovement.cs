using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using static MeterControl;
using UnityEngine;

// Simplified movement system to easily test other scripts. Will be removed from full game
public class TestMovement : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;

    public float moveSpeed = 1;
    public float moveAccel = 1;
    public float airFriction = .5F;

    //Controls the meter
    public bool meterActive = false;

    // Controls how high the character will jump
    [SerializeField] float jumpHeight = 2;
    bool isOnGround = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool meterUsed = false;

        if (Input.GetKey(KeyCode.LeftShift) && !meterActive && currentMeter > 0)
        {
            meterActive = true;
        }
        else if (currentMeter <= 0)
        {
            meterActive = false;
        } 

        // Whenever spacebar is pressed, the character will jump up as long as they are on ground
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround) {
            playerRigidbody.velocity += Vector2.up * jumpHeight;
        }
        // Player can jump additional times in the air as long as they have meter
        else if(Input.GetKeyDown(KeyCode.Space) && !isOnGround && currentMeter > 0)
        {
            playerRigidbody.velocity += Vector2.up * jumpHeight;
            currentMeter--;
        }

        if(Input.GetKeyDown(KeyCode.RightArrow) && meterActive)
        {
            playerRigidbody.velocity += Vector2.right * moveAccel * 10;
            meterUsed = true;
            meterActive = false;
          
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && meterActive)
        {
            playerRigidbody.velocity += Vector2.left * moveAccel * 10;
            meterUsed = true;
            meterActive = false;
          
        }
        if (!meterUsed)
        {
            bool left = Input.GetKey(KeyCode.LeftArrow);
            bool right = Input.GetKey(KeyCode.RightArrow);

            float x = playerRigidbody.velocity.x;
            float y = playerRigidbody.velocity.y;

            if (x < 0 && !left)
            { // moving left and not holding left, apply drag
                x *= (1 - airFriction);
            }
            if (x > 0 && !right)
            { // moving right and not holding right, apply drag
                x *= (1 - airFriction);
            }

            if (left && !right && x > -moveSpeed)
            { // holding left
                x = Mathf.Clamp(x - moveAccel, -moveSpeed, moveSpeed);
            }
            if (right && !left && x < moveSpeed)
            { // holding right
                x = Mathf.Clamp(x + moveAccel, -moveSpeed, moveSpeed);
            }

            playerRigidbody.velocity = new Vector2(x, y);
        }
        else
        {
            meterActive = false;
            currentMeter--;
        }
    }

    // Determines what happens to the player if they collide into something
    void OnCollisionEnter2D(Collision2D collision) {
        // Player jumps are refreshed
        if (collision.gameObject.CompareTag("Ground")) {
            isOnGround = true;
        }
    }

    // Determines what happens to the player if they stop colliding with something
    void OnCollisionExit2D(Collision2D collision) {
        // Player is unable to jump once they are in the air
        if (collision.gameObject.CompareTag("Ground")) {
            isOnGround = false;
        }
    }
}