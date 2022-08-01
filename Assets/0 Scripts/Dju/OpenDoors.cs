using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    public List<GameObject> doors = new List<GameObject>();
    private void Start()
    {
        Invoke("OpenTheDoors", 3);
    }
    void OpenTheDoors() {
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].SetActive(false);
        }
    }
}
