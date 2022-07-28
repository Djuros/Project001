using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletPoolItem {
    public GameObject go { get; set; }
    public MeshRenderer meshRenderer { get; set; }
    public Collider collider { get; set; }
}

public class BulletsPooling : MonoBehaviour {
    // Inspector assigned
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _poolSize;
    [SerializeField] private bool _autoResize;
    
    // Private variables
    private List<BulletPoolItem> _bullets = new();
    
    private void Start() {
        // instantiate bullets
        for (var i = 0; i < _poolSize; i++) {
            AddNewBullet();
        }
    }

    private BulletPoolItem AddNewBullet() {
        // instantiate bullet
        var bullet = Instantiate(_bulletPrefab, parent: transform);
        // create bullet pool item
        var bulletPoolItem = new BulletPoolItem {
            go = bullet,
            meshRenderer = bullet.GetComponent<MeshRenderer>(),
            collider = bullet.GetComponent<Collider>(),
        };
        // setup
        bulletPoolItem.go.SetActive(false);
        bulletPoolItem.meshRenderer.enabled = false;
        bulletPoolItem.collider.enabled = false;
        // add to list
        _bullets.Add(bulletPoolItem);
        
        return bulletPoolItem;
    }
    
    private IEnumerator ReturnBulletInternal(long id, float delay) {
        yield return new WaitForSeconds(delay);
        
        // disable bullet
        var bpi = _bullets.Find(bullet => id == bullet.collider.GetInstanceID());
        if (bpi == null) throw new Exception("Bullet not found");

        // disable components
        bpi.go.SetActive(false);
        bpi.meshRenderer.enabled = false;
        bpi.collider.enabled = false;
    }

    // public API
    public void SpawnBullet(Transform transform) {
        // find bullet pool item
        var bulletPoolItem = _bullets.Find(x => !x.go.activeSelf);
        
        // if not found
        if (bulletPoolItem == null) {
            if (!_autoResize) throw new Exception("No more bullets available");

            bulletPoolItem = AddNewBullet();
        }
        
        // setup
        bulletPoolItem.go.transform.position = transform.position;
        bulletPoolItem.go.transform.rotation = transform.rotation;
        bulletPoolItem.go.SetActive(true);
        bulletPoolItem.meshRenderer.enabled = true;
        bulletPoolItem.collider.enabled = true;
    }

    public void ReturnBullet(long id, float delay = 0f) {
        StartCoroutine(ReturnBulletInternal(id, delay));
    }    
}