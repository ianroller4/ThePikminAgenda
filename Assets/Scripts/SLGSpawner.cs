using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SLGSpawner : MonoBehaviour
{
    public GameObject slgPrefab;
    public float minSpawnDistance = 1f;
    public float maxSpawnDistance = 3f;

    private bool playerNear = false;
    private bool canSpawn = false;

    private float timer = 0f;
    public float spawnCooldown = 2f;

    private void Update()
    {
        if (canSpawn)
        {
            if (playerNear)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SpawnSLG();
                }
            }
        }
        else
        {
            UpdateTimer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go != null)
        {
            playerNear = true;
            canSpawn = true;
        }
        else
        {
            Debug.LogWarning("No game object with collider..... somehow");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go != null)
        {
            playerNear = false;
            canSpawn = false;
        }
        else
        {
            Debug.LogWarning("No game object with collider..... somehow");
        }
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;
        if (timer > spawnCooldown)
        {
            timer = 0f;
            canSpawn = true;
        }
    }

    private void SpawnSLG()
    {
        float angle = Random.Range(0, 360);
        Vector3 dir = ApplyRotationToVector(Vector3.right, angle);
        Vector3 spawnPosition = transform.position + dir * Random.Range(minSpawnDistance, maxSpawnDistance);
        GameObject slg = Instantiate(slgPrefab);
        slg.transform.position = spawnPosition;
        canSpawn = false;
    }

    private Vector3 ApplyRotationToVector(Vector3 v, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * v;
    }
}
