using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
[RequireComponent(typeof(Collider2D))]
public class PartCollector : MonoBehaviour
{
    public int partsToWin = 3;
    private int partCount = 0;
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GetComponent<SceneLoader>();
    }

    private void Update()
    {
        if (partCount >= partsToWin)
        {
            sceneLoader.LoadNextScene(3);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go != null)
        {
            if (go.GetComponent<CarryableObject>() != null)
            {
                partCount++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go != null)
        {
            if (go.GetComponent<CarryableObject>() != null)
            {
                partCount--;
                if (partCount < 0)
                {
                    partCount = 0;
                }
            }
        }
    }
}
