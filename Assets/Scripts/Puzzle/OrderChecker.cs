using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderChecker : MonoBehaviour
{
    [SerializeField] private List<WeightButton> correctOrder;

    private List<WeightButton> pressedOrder;

    [SerializeField] private GameObject doorToOpen;

    private void Start()
    {
        pressedOrder = new List<WeightButton>();
    }

    private void Update()
    {
        if (doorToOpen != null)
        {
            if (pressedOrder.Count == correctOrder.Count)
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
    }

    private bool CheckOrder()
    {
        int sameCount = 0;
        for (int i = 0; i < pressedOrder.Count; i++)
        {
            if (pressedOrder[i].Equals(correctOrder[i]))
            {
                sameCount++;
            }
        }

        return sameCount == correctOrder.Count;
    }

    private void OpenDoor()
    {
        // open the door
        doorToOpen.SetActive(false);
    }

    private void ResetWeightButtons()
    {
        for (int i = 0; i < pressedOrder.Count; i++)
        {
            // Reset each button
            pressedOrder[i].GetComponent<Animator>().SetBool("pressed", false);
        }
        pressedOrder.Clear();
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
