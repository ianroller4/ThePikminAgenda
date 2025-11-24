using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SacrificeFire : MonoBehaviour
{
    [SerializeField] private GameObject altarOne;
    [SerializeField] private GameObject altarTwo;
    [SerializeField] private GameObject altarThree;
    [SerializeField] private GameObject altarFour;

    private int killed = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject slg = collision.gameObject;
        if (slg != null)
        {
            if (slg.GetComponent<SillyLittleGuys>() != null)
            {
                // Kill them
                killed++;
                UpdateAltars();
            }
        }
    }

    private void UpdateAltars()
    {
        // Turn on altar fire animations based on killed
    }
}
