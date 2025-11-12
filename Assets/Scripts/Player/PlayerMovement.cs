using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* PlayerMovement
 * 
 * Controls the player movement
 * 
 */
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // --- Component References ---
    private Rigidbody2D rb;

    // --- Variables ---
    public float speed = 5f;
    private Vector2 input;

    // --- Animator ---
    private Animator animator;
    private Vector2 lastDir = Vector2.zero;

    /* Start
     * 
     * Called once before the first frame of update
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    /* Update
     * 
     * Called once per frame
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    void Update()
    {
        ListenForInput();
        Move();
    }

    /* ListenForInput
     * 
     * Listens for keyboard input from the player and based on input animates the character
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void ListenForInput()
    {
        // Get input
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
        // Player moving so move animation
        if (input != Vector2.zero)
        {
            animator.SetBool("idle", false);
            lastDir = input;
            animator.SetFloat("x", input.x);
            animator.SetFloat("y", input.y);
        }
        else // Player not moving so idle animation
        {
            animator.SetBool("idle", true);
            animator.SetFloat("x", lastDir.x);
            animator.SetFloat("y", lastDir.y);
        }
    }

    /* Move
     * 
     * Moves the player by changing the velocity of the rigidbody based on input and speed
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void Move()
    {
        rb.velocity = input * speed;
    }
}
