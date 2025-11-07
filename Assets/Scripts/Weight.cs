using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Weight : MonoBehaviour
{
    private int currentWeight = 0;
    public int weightNeeded = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<SillyLittleGuys>() != null)
            {
                currentWeight++;
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
    }
}
