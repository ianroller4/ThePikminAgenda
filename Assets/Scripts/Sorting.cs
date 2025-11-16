using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Sorting class handles Y-based sprite layering for top-down objects.
 * It updates the sprite's sorting order based on its Y-position
 * It also uses a small threshold to avoid unnecessary updates.
 */
[RequireComponent(typeof(SpriteRenderer))]
public class Sorting : MonoBehaviour
{
    // --- References ---
    private SpriteRenderer sorted;

    // --- Settings ---
    [SerializeField]
    private bool sortingActive = true; // To deactivate this on certain objects.
    [SerializeField]
    private float yOffset = 0f; // Adjusts the sorting pivot when the sprite's pivot is not aligned to the feet
    [SerializeField]
    private float minDistance = 0.2f; // Minimum distance before the sorting value updates.

    private int lastSortOrder = 0;

    // Start is called before the first frame update
    void Start()
    {
        sorted = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!sorted || !sortingActive)
        {
            return;
        }

        float sortY = transform.position.y + yOffset;
        int newSortOrder = (int)(-sortY / minDistance);
        if (lastSortOrder != newSortOrder)
        {
            lastSortOrder = sorted.sortingOrder;
            sorted.sortingOrder = newSortOrder;
        }
    }
}