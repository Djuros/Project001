using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 torgueAmount;
    public Transform matchRef;
    private void Awake()
    {
        transform.parent = null;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = matchRef.position;
        RotateCam();
    }
    private void RotateCam()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            rb.AddTorque(-torgueAmount);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rb.AddTorque(torgueAmount);
        }
    }
}
