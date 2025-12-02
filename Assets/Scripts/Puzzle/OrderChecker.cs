using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderChecker : MonoBehaviour
{
    [SerializeField] private List<WeightButton> correctOrder;

    private List<WeightButton> pressedOrder;

    [SerializeField] private GameObject doorToOpen;

    private void Update()
    {
        if (doorToOpen != null)
        {
            if (CheckOrder())
            {
                OpenDoor();
            }
            else
            {
                ResetWeightButtons();
            }
        }
    }

    private bool CheckOrder()
    {
        for (int i = 0; i < pressedOrder.Count; i++)
        {
            // Loop through and compare
            // return false
        }

        return true;
    }

    private void OpenDoor()
    {
        // open the door
    }

    private void ResetWeightButtons()
    {
        for (int i = 0; i < pressedOrder.Count; i++)
        {
            // Reset each button
        }
        // Clear list
    }

    public void AddWeightButton(WeightButton wb)
    {
        if (!pressedOrder.Contains(wb))
        {
            pressedOrder.Add(wb);
        }
    }

    public void RemoveWeightButton(WeightButton wb)
    {
        if (pressedOrder.Contains(wb))
        {
            pressedOrder.Remove(wb);
        }
    }
}
