using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            BulletPool.ins.Fire();
        }
    }
}
