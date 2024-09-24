using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy_prefab;


    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }


    IEnumerator SpawnEnemies()
    {
        while (true) {
            GameObject enemy = Instantiate(enemy_prefab);
            enemy.transform.position = transform.position;
            enemy.GetComponent<SpriteRenderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            yield return new WaitForSeconds(1.0f);
        }
    }
}
