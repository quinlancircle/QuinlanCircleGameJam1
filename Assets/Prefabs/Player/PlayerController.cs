using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float collisionOffset = 0.05f; // So you don't get stuck inside anything.
    public ContactFilter2D movementFilter;

    private Vector2 moveInput;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        // Try to move player in input direction, followed by left right and up down input if failed.
        bool success = MovePlayer(moveInput);

        if (!success) {
            // Try left/right.
            success = MovePlayer(new Vector2(moveInput.x, 0));

            if (!success) {
                success = MovePlayer(new Vector2(0, moveInput.y));
            }
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>().normalized;
    }

    // Tries to move the player in a direction by casting in that direction by
    // amount moved plus an offset. If no collisions are found, it moves the
    // player. Returns true or false depending on whether a move was executed.
    // https://www.youtube.com/watch?v=05eWA0TP3AA
    public bool MovePlayer(Vector2 direction)
    {
        // Check for potential collisions.
        int count = rb.Cast(
            direction, // X and Y values between -1 and 1 that represent the direction from the body to look for c...
            movementFilter, // The settings that determine where a collision can occur on such layers to collid...
            castCollisions, // List of collisions to store the found collisions into after the Cast is finished.
            moveSpeed * Time.fixedDeltaTime + collisionOffset // The amount to cast equal to the movement plus a...
        );

        if (count == 0) {
            Vector2 moveVector = direction * moveSpeed * Time.fixedDeltaTime;

            // No collisions.
            rb.MovePosition(rb.position + moveVector);
            return true;
        } else {
            /*
            // Print collisions.
            foreach (RaycastHit2D hit in castCollisions) {
                print(hit.ToString());
            }
            */

            return false;
        }
    }
}
