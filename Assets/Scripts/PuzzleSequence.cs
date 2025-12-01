using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSequence : MonoBehaviour
{
    [SerializeField] private List<GameObject> orderForActivation;
    private List<GameObject> orderOfActivations;

    [SerializeField] private GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        orderOfActivations = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool CompareOrders()
    {
        if (orderForActivation.Count == orderOfActivations.Count)
        {
            for (int i = 0; i < orderForActivation.Count; i++)
            {
                if (!orderOfActivations[i].Equals(orderForActivation[i]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void AddObject(GameObject go)
    {
        if (!orderOfActivations.Contains(go))
        {
            orderOfActivations.Add(go);
        }
    }

    public void RemoveObject(GameObject go)
    {
        if (orderOfActivations.Contains(go))
        {
            orderOfActivations.Remove(go);
        }
    }
}
