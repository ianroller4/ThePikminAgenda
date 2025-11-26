using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WeightTrigger : MonoBehaviour
{
    private int currentWeight = 0;
    private int weightNeeded = 5;

    [SerializeField] private GameObject enableObject;
    [SerializeField] private bool activateObjectOnRequirementMet = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<SillyLittleGuys>() != null)
            {
                currentWeight++;
                if (currentWeight >= weightNeeded)
                {
                    if (activateObjectOnRequirementMet)
                    {
                        enableObject.SetActive(true);
                    }
                    else
                    {
                        enableObject.SetActive(false);
                    }
                    
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
                    if (activateObjectOnRequirementMet)
                    {
                        enableObject.SetActive(false);
                    }
                    else
                    {
                        enableObject.SetActive(true);
                    }
                }
            }
        }
    }
}
