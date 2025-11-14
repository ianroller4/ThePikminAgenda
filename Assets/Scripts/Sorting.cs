using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Sortable : MonoBehaviour
{
    public float yOffset = 0f;

    SpriteRenderer sorted;
    public bool sortingActive = true; // Allows us to deactivate this on certain objects.
    public const float MIN_DISTANCE = 0.2f; // Minimum distance before the sorting value updates.
    int lastSortOrder = 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        sorted = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        if (!sorted || !sortingActive) return;

        float sortY = transform.position.y + yOffset;
        int newSortOrder = (int)(-sortY / MIN_DISTANCE);
        if (lastSortOrder != newSortOrder)
        {
            lastSortOrder = sorted.sortingOrder;
            sorted.sortingOrder = newSortOrder;
        }
    }
}