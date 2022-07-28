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
    public int my_id, capturedFlags;
    public float my_healt;
    public PhotonView pv;
    public Animator anim;
    private readonly int _deathParameterHash = Animator.StringToHash("Death");
    public AudioSource _as;
    public AudioClip body_hit, death_clip;
    public bool dead;
    public TextMeshPro name_label;
    public CharacterController ch;
    public PlayerController pc;
    private void Start()
    {
        my_healt = 100;
        if (pv.IsMine)
        {
            this.gameObject.name = PhotonNetwork.NickName;
            my_id = pv.ViewID;
            if (HUD.ins != null) HUD.ins.fill_bar.fillAmount = 1;
        }
        else
        {
            this.gameObject.name = pv.Owner.NickName;
            my_id = pv.ViewID;
        }
    }

    public void Take_Damage(float _amount)
    {
        print("PV " + gameObject.name);

        if (!pv.IsMine) { return; }
        _as.volume = 0.6f;
        my_healt -= _amount;
        HUD.ins.fill_bar.fillAmount = my_healt / 100;
        _as.PlayOneShot(body_hit);
        if (my_healt <= 0 && !dead) { Death(); }
    }
    void Death() {
        _as.volume = 0.5f;
        dead = true;
        _as.PlayOneShot(death_clip);
        anim.SetTrigger(_deathParameterHash);
    }
    [PunRPC]
    public void Pos(string _x, string _y, string _z)
    {
        float x = 0; float.TryParse(_x, NumberStyles.Any, CultureInfo.InvariantCulture, out x);
        float y = 0; float.TryParse(_y, NumberStyles.Any, CultureInfo.InvariantCulture, out y);
        float z = 0; float.TryParse(_z, NumberStyles.Any, CultureInfo.InvariantCulture, out z);
        transform.position = new Vector3(x,y,z);
    }
}
