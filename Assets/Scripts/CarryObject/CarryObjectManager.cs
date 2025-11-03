using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryObjectManager : MonoBehaviour
{
    public List<CarryableObject> carryObjects;

    private void Awake()
    {
        carryObjects = new List<CarryableObject>();
    }

    public void AddObject(CarryableObject co)
    {
        if (!carryObjects.Contains(co))
        {
            carryObjects.Add(co);
        }
    }

    public void RemoveObject(CarryableObject co)
    {
        if (carryObjects.Contains(co))
        {
            carryObjects.Remove(co);
        }
    }
}
