using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemaningRock : MonoBehaviour
{
    [SerializeField]
    private float fallHeight = 15f;

    [SerializeField]
    private float fallDuration = 1f;

    [SerializeField]
    private GameObject landingHitboxPrefab;

    private Boss boss;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool hasLanded = false;

    private Collider2D col;

    private GameObject warningToDestroy;

    public void SetWarning(GameObject warning)
    {
        warningToDestroy = warning;
    }

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        boss = FindObjectOfType<Boss>();
        startPos = transform.position;
        targetPos = transform.position - new Vector3(0f, fallHeight, 0f);

        col.isTrigger = true;

        StartCoroutine(FallRoutine());
    }

    // Update is called once per frame
    private void Update()
    {
        if (boss != null && boss.isRollingFailed)
        {
            Destroy(gameObject);
        }
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

        if (warningToDestroy != null)
        {
            Destroy(warningToDestroy);
        }

        SpawnLandingHitbox();

        col.isTrigger = false;
    }

    private void SpawnLandingHitbox()
    {
        GameObject hit = Instantiate(landingHitboxPrefab, transform.position, Quaternion.identity);

        Destroy(hit, 0.1f);
    }
}
