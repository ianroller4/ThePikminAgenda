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

    private float timer = 0f;
    public float spawnCooldown = 1f;

    private SLGManager slgManager;

    private void Start()
    {
        slgManager = FindObjectOfType<SLGManager>();
    }

    private void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.Space))
        {
            SpawnSLG();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerNear = false;
        }
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;
        if (timer > spawnCooldown)
        {
            timer = 0f;
        }
    }

    private void SpawnSLG()
    {
        if (!slgManager.CanAddSLG())
        {
            Debug.Log("Cannot spawn more SLG. already MAX CAPACITY YOU GREEDY!!");
            return;
        }

        float angle = Random.Range(0, 360);
        float radius = Random.Range(minSpawnDistance, maxSpawnDistance);

        Vector3 spawnPosition = (transform.position + new Vector3(0, -3f, 0)) + (Vector3)(Random.insideUnitCircle * radius);
        GameObject slg = Instantiate(slgPrefab, spawnPosition, Quaternion.identity);
        Animator anim = slg.GetComponent<Animator>();
        anim.Play("Born", 0, 0f);
        slg.GetComponent<SpriteRenderer>().sortingLayerName = "AboveDefault";
    }
}
