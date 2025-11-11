using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ListenForInput();
        Move();
    }

    private void ListenForInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
        if (input != Vector2.zero)
        {
            animator.SetBool("idle", false);
            lastDir = input;
            animator.SetFloat("x", input.x);
            animator.SetFloat("y", input.y);
        }
        else
        {
            animator.SetBool("idle", true);
            animator.SetFloat("x", lastDir.x);
            animator.SetFloat("y", lastDir.y);
        }
    }

    private void Move()
    {
        rb.velocity = input * speed;
    }
}
