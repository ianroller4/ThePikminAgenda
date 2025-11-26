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

    private void Start()
    {
        InitFraction();
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
        fraction.transform.position = transform.position + Vector3.up * 2;
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
        Destroy(gameObject);
        Destroy(fraction.gameObject);
    }
}
