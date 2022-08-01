using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints, randomized_pool = new List<Transform>();
    public List<GameObject> flags = new List<GameObject>();
    public GameObject flag_prefab;
    int randomSpawnPoint;
    public int flagLimit = 1;
    public static bool SpawnEnabled;

    private void Start()
    {
        SpawnFlags();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ReSpawnFlags();
        }
    }
   
    void SpawnFlags()
    {
        randomized_pool = new List<Transform>(spawnPoints);
        for (int i = 0; i < flagLimit; i++) 
        {
            GameObject _go = Instantiate(flag_prefab, Vector3.zero, Quaternion.identity);
            flags.Add(_go);
            Transform tmp_sp = randomized_pool[Random.Range(0, randomized_pool.Count)];
            _go.transform.position = tmp_sp.position;
            randomized_pool.Remove(tmp_sp);
        }
    }
    public void ReSpawnFlags()
    {
        randomized_pool = new List<Transform>(spawnPoints);
        for (int i = 0; i < flags.Count; i++)
        {
            flags[i].SetActive(true);
            Transform tmp_sp = randomized_pool[Random.Range(0, randomized_pool.Count)];
            flags[i].transform.position = tmp_sp.position;
            randomized_pool.Remove(tmp_sp);
        }
    }


    /*void Update()
    {
        if (flagCount < flagLimit)
        {
            SpawnEnabled = true;
            SpawnFlag();
        } else if (flagCount >= flagLimit)
        {
            SpawnEnabled = false;
        }

        //------- TESTING RESPAWN -------
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(flag);
            flagCount--;
        }
        //-------------------------------


    }

    void SpawnFlag()
    {
        if (SpawnEnabled)
        {
            randomSpawnPoint = Random.Range(0, spawnPoints.Length);

            flag = Instantiate(flag_prefab, spawnPoints[randomSpawnPoint].position, Quaternion.identity);
            flagCount++;
        }
    }*/
}
