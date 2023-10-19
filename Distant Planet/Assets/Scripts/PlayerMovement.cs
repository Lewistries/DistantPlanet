using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using static MeterControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour {
    // This makes the player gradually slow down as they hit a wall instead of coming to a complete stop.
    // Helps prevent the player getting stuck inside a wall.
    public float elasticity;
    
    // The acceleration of gravity.
    public float gravityAccel;

    // The maximum movement speed with just standard movement.
    public float moveSpeed;
    // The acceleration of standard movement.
    public float moveAccel;

    // The speed of a dash.
    public float dashSpeed;

    // The amount by which you slow down on the ground (0 = frictionless, 1 = instant stop)
    public float groundFriction;
    // The amount by which you slow down on the ground when holding forward
    public float reducedFriction;
    // The amount by which you slow down in the air
    public float airFriction;

    // The speed of a jump.
    public float jumpSpeed;
    // The speed of a double jump.
    public float doubleJumpSpeed;
    // The length of time you can hold the button to retain the maximum speed of a jump.
    public float jumpDuration;

    // The amount of bonus time you get to jump after leaving the ground.
    // Used to give slightly more leniency on jumping at the edge of a platform.
    public float coyoteDuration;
    // The amount of bonus time you get to wall jump after leaving a wall.
    // Used to give more leniency when holding away from a wall before you press the jump button.
    public float wallCoyoteDuration;

    // How high they player will go when they land on a bounce pad.
    public int bounceBoost;

    // The hitbox of the player.
    private new BoxCollider2D collider;
    
    // The player's velocity.
    private Vector2 velocity = Vector2.zero;

    // The speed of the current jump.
    private float jumpStrength = 0;
    // The horizontal speed of the current jump, used for wall jumps.
    private float wallJumpStrength = 0;
    // The amount of time the player has held the current jump.
    private float jumpElapsed = 0;

    // Whether the player is on the ground.
    private bool grounded = false;
    // The direction of the wall the player is sliding against, or 0 if there is no wall.
    private float walldir = 0;

    // The direction the player is facing (that is, the last direction they pressed)
    private float facing = 1;

    // The amount of coyote time elapsed.
    private float coyoteElapsed = 999;
    private float wallCoyoteElapsed = 999;

    //Controls player respawn point
    private Vector3 respawnPoint;
    public Rigidbody2D playerRigidbody;

    // Start is called before the first frame update
    void Start() {
        collider = GetComponent<BoxCollider2D>();
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update() {
        float direction = Input.GetAxis("Horizontal");
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = respawnPoint;
            currentMeter = meterStat;
            velocity = Vector2.zero;
        }
        if (-moveSpeed < velocity.x && velocity.x < moveSpeed) { // not at top speed
            velocity.x += moveAccel * direction * Time.deltaTime;
            velocity.x = Mathf.Clamp(velocity.x, -moveSpeed, moveSpeed);
        }
        if (velocity.x != 0 && direction / velocity.x <= 0) { // not holding into movement
            if (grounded) {
                velocity.x *= Mathf.Pow(1.0f - groundFriction, Time.deltaTime);
            } else {
                velocity.x *= Mathf.Pow(1.0f - airFriction, Time.deltaTime);
            }
        } else if (grounded) { // ground friction when holding into movement
            velocity.x *= Mathf.Pow(1.0f - reducedFriction, Time.deltaTime);
        }

        if (direction != 0) {
            facing = direction;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentMeter > 0) {
            if (facing < 0) {
                velocity.x = -dashSpeed;
            } else {
                velocity.x = dashSpeed;
            }
            velocity.y = 0;
            currentMeter--;
        }

        velocity.y -= gravityAccel * Time.deltaTime;

        if (Input.GetButtonDown("Jump")) {
            jumpStrength = 0;
            wallJumpStrength = 0;
            jumpElapsed = 0;
            if (grounded) {
                jumpStrength = jumpSpeed;
            } else if (walldir != 0) {
                jumpStrength = jumpSpeed;
                wallJumpStrength = jumpSpeed * -walldir;
            } else if (currentMeter > 0) {
                jumpStrength = doubleJumpSpeed;
                currentMeter--;
            }
        }
        if (Input.GetButton("Jump") && jumpElapsed < jumpDuration) {
            if (jumpStrength != 0) {
                velocity.y = jumpStrength;
            }
            if (wallJumpStrength != 0) {
                velocity.x = wallJumpStrength;
            }
        }
        jumpElapsed += Time.deltaTime;
        coyoteElapsed += Time.deltaTime;

        if (coyoteElapsed > coyoteDuration) {
            grounded = false;
        }
        if (MoveWithCollision(Vector2.zero)) { // Player is stuck inside a floor
            print("stuck");
            if (grounded) {
                transform.Translate(Vector2.up * 10);
            } else if (walldir != 0) {
                transform.Translate(Vector2.right * 10 * walldir);
            } else {
                transform.Translate(Vector2.down * 10);
            }
        }

        if (MoveWithCollision(Vector2.up * velocity.y * Time.deltaTime)) {
            if (velocity.y < 0) {
                grounded = true;
                coyoteElapsed = 0;
            }
            velocity.y *= (1 - 1 / elasticity);
        }

        if (velocity.x != 0) {
            wallCoyoteElapsed += Time.deltaTime;
            if (wallCoyoteElapsed > wallCoyoteDuration) {
                walldir = 0;
            }
            if (MoveWithCollision(Vector2.right * velocity.x * Time.deltaTime)) {
                if (velocity.x > 0) {
                    walldir = 1;
                    wallCoyoteElapsed = 0;
                } else {
                    walldir = -1;
                    wallCoyoteElapsed = 0;
                }
                velocity.x *= (1 - 1 / elasticity);
            }
        }
    }

    bool MoveWithCollision(Vector2 movement) {
        Vector2 colliderSize = collider.size * transform.localScale;
        float distance = movement.magnitude * elasticity;
        RaycastHit2D[] results = Physics2D.BoxCastAll(transform.position, colliderSize, 0, movement, distance);
        foreach (RaycastHit2D result in results) {
            if (result.collider != collider && !result.collider.isTrigger) {
                return true;
            }
        }
        transform.Translate(movement);
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint")) {
            respawnPoint = transform.position;
            currentMeter = meterStat;
        } else if (collision.gameObject.CompareTag("Fall")) {
            transform.position = respawnPoint;
            currentMeter = meterStat;
            velocity = Vector2.zero;
        } else if (collision.gameObject.CompareTag("BoostUp")) {
            velocity.y = bounceBoost;
            if (MoveWithCollision(Vector2.up * velocity.y * Time.deltaTime)) {
                grounded = true;
                velocity.y *= (1 - 1 / elasticity);
            }
        } else if (collision.gameObject.CompareTag("Moving")) {
            collider.transform.SetParent(collision.transform);
        } else if (collision.gameObject.CompareTag("EndLevel")) {
            SceneManager.LoadScene(collision.gameObject.name, LoadSceneMode.Single);

        }
    }



    private void OnCollisionEnter2D(Collision2D collide) 
    {
        if (collide.gameObject.CompareTag("Ice"))
        {
            groundFriction = .25F;
        }
    }

    private void OnCollisionExit2D(Collision2D collide) 
    {
        if (collide.gameObject.CompareTag("Ice"))
        {
            groundFriction = .75F;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Moving")) {
            collider.transform.SetParent(null);
        }
    }

}

    
