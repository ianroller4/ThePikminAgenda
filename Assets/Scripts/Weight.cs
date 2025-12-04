using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Weight : MonoBehaviour
{
    private int currentWeight = 0;
    public int weightNeeded = 5;

    [SerializeField] private Fraction fraction;

    private Animator animator;

    private AudioSource audioSource;

    private void Start()
    {
        InitFraction();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void InitFraction()
    {
        fraction.transform.position = transform.position + Vector3.up;
        fraction.SetDenominator(weightNeeded.ToString());
        fraction.SetNumerator("0");
        fraction.gameObject.SetActive(false);
    }

    private void UpdateFraction()
    {
        fraction.transform.position = transform.position + Vector3.up * 2.5f;
        if (currentWeight > 0)
        {
            fraction.gameObject.SetActive(true);
            fraction.SetNumerator(currentWeight.ToString());
        }
        else
        {
            fraction.SetNumerator("0");
            fraction.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<SillyLittleGuys>() != null)
            {
                currentWeight++;
                UpdateFraction();
                if (currentWeight >= weightNeeded)
                {
                    animator.SetBool("break", true);
                    ClearObject();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<SillyLittleGuys>() != null)
            {
                currentWeight--;
                UpdateFraction();
                if (currentWeight < 0)
                {
                    currentWeight = 0;
                }
            }
        }
    }

    private void ClearObject()
    {
        audioSource.Play();
        Destroy(gameObject, 1.5f);
    }
}
