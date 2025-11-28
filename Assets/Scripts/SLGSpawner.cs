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
    public float spawnCooldown = 1f;

    private void Update()
    {

        if (playerNear)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnSLG();
            }
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
        float radius = Random.Range(minSpawnDistance, maxSpawnDistance);

        Vector3 spawnPosition = (transform.position + new Vector3(0, -1f, 0)) + (Vector3)(Random.insideUnitCircle * radius);
        GameObject slg = Instantiate(slgPrefab, spawnPosition, Quaternion.identity);
        Animator anim = slg.GetComponent<Animator>();
        anim.Play("Born", 0, 0f);
        slg.GetComponent<SpriteRenderer>().sortingLayerName = "AboveDefault";
        canSpawn = false;
    }

    private Vector3 ApplyRotationToVector(Vector3 v, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * v;
    }
}
