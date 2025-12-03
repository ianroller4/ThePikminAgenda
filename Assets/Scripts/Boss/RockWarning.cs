using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWarning : MonoBehaviour
{
    [SerializeField]
    private GameObject rockPrefab;

    [SerializeField]
    private float delay = 1.0f;

    [SerializeField]
    private float fallHeight = 15f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FallRoutine());
    }

    private IEnumerator FallRoutine()
    {
        Vector3 rockSpawnPos = transform.position + new Vector3(0f, fallHeight, 0f);

        yield return new WaitForSeconds(delay);

        Instantiate(rockPrefab, rockSpawnPos, Quaternion.identity);

        Destroy(gameObject,1f);
    }
}
