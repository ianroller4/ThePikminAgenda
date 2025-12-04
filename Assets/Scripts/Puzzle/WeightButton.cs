using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WeightButton : MonoBehaviour
{
    private bool pressed = false;

    private Animator animator;

    [SerializeField] private OrderChecker orderChecker;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pressed)
        {
            animator.SetBool("pressed", false);
        }
        else
        {
            animator.SetBool("pressed", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<SillyLittleGuys>() != null)
            {
                pressed = true;
                orderChecker.AddWeightButton(this);
                audioSource.Stop();
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<SillyLittleGuys>() != null)
            {
                pressed = false;
                orderChecker.RemoveWeightButton(this);
                audioSource.Stop();
                audioSource.Play();
            }
        }
    }
}
