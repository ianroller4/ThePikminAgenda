using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderChecker : MonoBehaviour
{
    [SerializeField] private List<WeightButton> correctOrder;

    private List<WeightButton> pressedOrder;

    [SerializeField] private GameObject doorToOpen;

    private AudioSource audioSource;

    private bool wasJustOpen = false;

    private void Start()
    {
        pressedOrder = new List<WeightButton>();
        audioSource = doorToOpen.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (doorToOpen != null)
        {
            if (pressedOrder.Count == correctOrder.Count)
            {
                if (CheckOrder())
                {
                    if (!wasJustOpen)
                    {
                        OpenDoor();
                    }
                }
                else
                {
                    ResetWeightButtons();
                }
            }
            else
            {
                if (wasJustOpen)
                {
                    CloseDoor();
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

    private void CloseDoor()
    {
        Debug.Log("Door Rebuild");
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        wasJustOpen = false;
        doorToOpen.GetComponent<Animator>().SetBool("crumble", false);
    }

    private void OpenDoor()
    {
        // open the door
        Debug.Log("Door Crumble");
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        wasJustOpen = true;
        doorToOpen.GetComponent<Animator>().SetBool("crumble", true);
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
