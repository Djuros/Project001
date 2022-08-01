using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class BulletPool : MonoBehaviour
{
    public static BulletPool ins;
    public GameObject bullet_prefab;
    public List<Bullet> bullets = new List<Bullet>();
    public Transform muzzle;
    public int initial_pool_size;
    public bool multiplayer;

    private void Awake()
    {
        ins = this;
      
    }
    public void Spawn_Bullets() {
        multiplayer = PhotonNetwork.IsConnected;
        for (int i =0; i < initial_pool_size; i++) {
            GameObject _bullet = null;
            //   if (multiplayer) { _bullet = PhotonNetwork.Instantiate("Bullet", Vector3.zero, Quaternion.identity); }
            // else { _bullet = Instantiate(bullet_prefab, Vector3.zero, Quaternion.identity); }
            _bullet = PhotonNetwork.Instantiate("Bullet", Vector3.zero, Quaternion.identity);
            bullets.Add(_bullet.GetComponent<Bullet>());
            _bullet.transform.position = new Vector3(0,10000,0);
        }
    }
    public void Fire()
    {
        Bullet _bullet = null;
        for (int i = 0; i < bullets.Count; i++) {
            if (bullets[i].transform.position.y == 10000) { _bullet = bullets[i]; break; }
        }
        if (_bullet == null) {
            if (multiplayer) { _bullet = PhotonNetwork.Instantiate("Bullet", Vector3.zero, Quaternion.identity).GetComponent<Bullet>(); }
            else { _bullet = Instantiate(bullet_prefab, Vector3.zero, Quaternion.identity).GetComponent<Bullet>(); }
            bullets.Add(_bullet);
        }
        _bullet.Eject(muzzle.position, muzzle.forward);
    }
}
