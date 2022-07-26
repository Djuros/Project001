using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour { 
    // inspector assigned
    [SerializeField] private int _speed = 5;
    [SerializeField] private int remove_at_time = 10;
    [SerializeField] private Collider col;
    [SerializeField] private Rigidbody rb;

    private void Start() {
        Invoke("Back_To_Pool", remove_at_time);
    }
    public void Eject(Vector3 _from, Vector3 _towards) {
        col.enabled = true;
        gameObject.SetActive(true);
        transform.position = BulletPool.ins.muzzle.position;
        transform.rotation = BulletPool.ins.muzzle.rotation;
        rb.velocity = BulletPool.ins.muzzle.transform.forward * _speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            other.GetComponent<MyMPRef>().Take_Damage(10);
            Back_To_Pool();
        }
    }
    public void Back_To_Pool() {
        transform.position = new Vector3(0,10000,0);
        transform.rotation = Quaternion.identity;
        col.enabled = false;
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}