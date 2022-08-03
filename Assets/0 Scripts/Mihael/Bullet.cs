using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class Bullet : MonoBehaviour { 
    // inspector assigned
    [SerializeField] private int _speed = 5;
    [SerializeField] private int remove_at_time = 10;
    [SerializeField] private Collider col;
    [SerializeField] private Rigidbody rb;
    public ParticleSystem hit_effect, blood_hit_effect;
    public AudioSource _as;
    public bool fired;
    public float damage;
    private void Start() {
        Invoke("Back_To_Pool", remove_at_time);
    }
    public void Eject(Vector3 _from, Vector3 _towards) {
        col.enabled = true;
        gameObject.SetActive(true);
        transform.position = BulletPool.ins.muzzle.position;
        transform.rotation = BulletPool.ins.muzzle.rotation;
        rb.velocity = BulletPool.ins.muzzle.transform.forward * _speed;
        ResetEffect();
    }
    private void Update()
    {
        if (transform.position.y < 1000 && !fired) {
            fired = true;
            Invoke("ResetBullet", 10);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            blood_hit_effect.transform.parent = null;
            blood_hit_effect.Play();
            other.GetComponent<MyMPRef>().Take_Damage(damage);
            Back_To_Pool();
          
        }
        else {
            AudioManager.ins.PlaySound();
            hit_effect.transform.parent = null;
            hit_effect.Play();
            Back_To_Pool();
        }
    }
    private void ResetBullet()
    {
       // fired = false;
        Back_To_Pool();
        ResetEffect();
    }
    public void Back_To_Pool() {
        transform.position = new Vector3(0,10000,0);
        transform.rotation = Quaternion.identity;
       // col.enabled = false;
       // gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    void ResetEffect() {
        hit_effect.transform.SetParent(transform);
        hit_effect.transform.localPosition = new Vector3(0,0,-5f);
        blood_hit_effect.transform.SetParent(transform);
        blood_hit_effect.transform.localPosition = new Vector3(0, 0, -5f);
    }
}