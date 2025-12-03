using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingRock : MonoBehaviour
{
    [SerializeField]
    private float fallHeight = 15f;

    [SerializeField]
    private float fallDuration = 1f;

    [SerializeField]
    private GameObject landingHitboxPrefab;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool hasLanded = false;

    private void Start()
    {
        startPos = transform.position;
        targetPos = transform.position - new Vector3(0f, fallHeight, 0f);

        StartCoroutine(FallRoutine());
    }

    private IEnumerator FallRoutine()
    {
        float t = 0f;

        while (t < fallDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / fallDuration);
            float easeIn = n * n;
            transform.position = Vector3.Lerp(startPos, targetPos, easeIn);

            yield return null;
        }

        Land();
    }

    private void Land()
    {
        if (hasLanded)
        {
            return;
        }

        hasLanded = true;
        transform.position = targetPos;

        SpawnLandingHitbox();

        Destroy(gameObject);
    }

    private void SpawnLandingHitbox()
    {
        GameObject hit = Instantiate(landingHitboxPrefab, transform.position, Quaternion.identity);
        Destroy(hit, 0.1f);
    }
}
