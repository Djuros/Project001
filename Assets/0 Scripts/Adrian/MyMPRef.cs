using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Globalization;
using TMPro;
public class MyMPRef : MonoBehaviour
{
    public int my_id, score;
    public float current_health, max_health;
    public PhotonView pv;
    public bool playAgain = false;

    public Animator anim;
    private readonly int _deathParameterHash = Animator.StringToHash("Death");
    public AudioSource _as;
    public AudioClip body_hit, death_clip;
    public bool dead;
    public TextMeshPro name_label;
    public CharacterController ch;
    public PlayerController pc;
    public PickupManager pum;
    public Rigidbody rb;
    bool in_jump;
    public Camera cam;
    public RotateCamera rc;
    public PickupManager pm;
    Vector3 prev_location;
    float cnt;
    public Vector3 spawn_pos;
    private void Start()
    {
        current_health = max_health;
        if (pv.IsMine)
        {
            this.gameObject.name = PhotonNetwork.NickName;
            my_id = pv.ViewID;
            if (HUD.ins != null) HUD.ins.fill_bar.fillAmount = 1;
            cam.enabled = true;
            cam.gameObject.SetActive(true);
            rc.enabled = true;
            pm.enabled = true;
        }
        else
        {
            this.gameObject.name = pv.Owner.NickName;
            my_id = pv.ViewID;
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.freezeRotation = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }
 

    private void Update()
    {
        OthersPlayersAnimationCallback();
        if (Input.GetKeyUp(KeyCode.O)) {
            Take_Damage(500);
        }
    }
    void OthersPlayersAnimationCallback() {
        if (!pv.IsMine)
        {
            if (!in_jump)
            {
                if (!dead)
                {

                    if (cnt < 1)
                    {
                        cnt += 1;
                        prev_location = transform.position;
                    }
                    else
                    {
                        cnt = 0;
                        if (prev_location != transform.position)
                        {
                            pc._anim.SetFloat(pc._runParameterHash, 1.5f);
                        }
                        else
                        {
                            pc._anim.SetFloat(pc._runParameterHash, 0);
                        }
                    }
                }
            }
            else
            {
                if (transform.position.y < 0.1f)
                {
                    in_jump = false;
                }
            }
        }
    }
    public void Take_Damage(float _amount)
    {
        if (!pv.IsMine) { return; }
        _as.volume = 0.6f;
        current_health -= _amount;
        HUD.ins.fill_bar.fillAmount = current_health / max_health;
        _as.PlayOneShot(body_hit);
        if (current_health <= 0 && !dead) { Die(); }
    }
    void Die() {
        Death();
        MessagingSystem.ins.I_Died(my_id);
    }
    public void Death() {
        _as.volume = 0.3f;
        dead = true;
        _as.PlayOneShot(death_clip);
        anim.SetTrigger(_deathParameterHash);
        if (pum.flag_anchor.childCount > 0) {
            Transform _flag = pum.flag_anchor.GetChild(0);
            _flag.parent = null;
            _flag.localScale = new Vector3(1,1,1);
        }
        Invoke("Respawn", 2);
        ch.enabled = false;
        pc.enabled = false;
    }
    void Respawn() {
        print("Respawn");
        current_health = 100;
        max_health = 100;
        dead = false;
        anim.Play("Idle",0);
        transform.position = spawn_pos;
        if (pv.IsMine) {
            pc.enabled = true;
            ch.enabled = true;
            MessagingSystem.ins.I_Respawned(my_id);
            HUD.ins.fill_bar.fillAmount = current_health / max_health;
            pc._fireRate = 5;
            pc._walkSpeed = 6;
            pc._runSpeed = 10;
            BulletPool.ins.Switch_The_Bullets("Bullet_10");
        }
    }
    [PunRPC]
    public void Pos(string _x, string _y, string _z)
    {
        if (pv.IsMine) {
            float x = 0; float.TryParse(_x, NumberStyles.Any, CultureInfo.InvariantCulture, out x);
            float y = 0; float.TryParse(_y, NumberStyles.Any, CultureInfo.InvariantCulture, out y);
            float z = 0; float.TryParse(_z, NumberStyles.Any, CultureInfo.InvariantCulture, out z);
            transform.position = new Vector3(x, y, z);
            spawn_pos = transform.position;
            pc.enabled = true;
            ch.enabled = true;
        }
    }
    public void Fire_Animation_MP(int _fireRate) {
        if (dead) { return; }
        anim.Play("ShootMp",1);
    }
    public void Jump_Mp() {
        if (dead) { return; }
        anim.Play("Jumping", 0);
        in_jump = true;
    }

}
