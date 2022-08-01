using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupsSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints, randomized_pool = new List<Transform>();
    public List<GameObject> pickups = new List<GameObject>();
    public GameObject[] pickup_prefab;
    int randomSpawnPoint;
    public int pickupsLimit = 4;
    public static bool SpawnEnabled;

    private void Start()
    {
        SpawnPickups();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RespawnPickups();
        }
    }

    void SpawnPickups()
    {
        randomized_pool = new List<Transform>(spawnPoints);
        for (int i = 0; i < pickupsLimit; i++)
        {
            GameObject _go = Instantiate(pickup_prefab[Random.Range(0, pickup_prefab.Length - 1)], Vector3.zero, Quaternion.identity);
            pickups.Add(_go);
            Transform tmp_sp = randomized_pool[Random.Range(0, randomized_pool.Count)];
            _go.transform.position = tmp_sp.position;
            randomized_pool.Remove(tmp_sp);
        }
    }
    public void RespawnPickups()
    {
        randomized_pool = new List<Transform>(spawnPoints);
        for (int i = 0; i < pickups.Count; i++)
        {
            pickups[i].SetActive(true);
            Transform tmp_sp = randomized_pool[Random.Range(0, randomized_pool.Count)];
            pickups[i].transform.position = tmp_sp.position;
            randomized_pool.Remove(tmp_sp);
        }
    }
}