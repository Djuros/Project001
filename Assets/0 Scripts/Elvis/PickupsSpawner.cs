using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PickupsSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints, randomized_pool = new List<Transform>();
    public List<GameObject> pickups = new List<GameObject>();
    public List<string> pickup_prefab_names = new List<string>();
    int randomSpawnPoint;
    public static bool SpawnEnabled;

    private void Start()
    {
        Invoke("SpawnPickups", 4);
    }
   

    void SpawnPickups()
    {
        if (PhotonNetwork.IsMasterClient) {
            randomized_pool = new List<Transform>(spawnPoints);
            for (int i = 0; i < pickup_prefab_names.Count; i++)
            {
                GameObject _go = PhotonNetwork.Instantiate(pickup_prefab_names[i], Vector3.zero, Quaternion.identity);
                pickups.Add(_go);
                Transform tmp_sp = randomized_pool[Random.Range(0, randomized_pool.Count)];
                _go.transform.position = tmp_sp.position;
                randomized_pool.Remove(tmp_sp);
            }
        }
        
    }
}