using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MyMPRef : MonoBehaviour
{
    public int my_id;
    public float my_healt;
    public PhotonView pv;
    public Transform gun;

    private void Start()
    {
        my_healt = 100;

        if (pv.IsMine)
        {
            this.gameObject.name = PhotonNetwork.NickName;
            my_id = pv.ViewID;
           HUD.ins.fill_bar.fillAmount = 1;
        }
        else
        {
            this.gameObject.name = pv.Owner.NickName;
            my_id = pv.ViewID;
        }
    }

    public void Take_Damage(float _amount)
    {
        my_healt -= _amount;
        HUD.ins.fill_bar.fillAmount = my_healt/100;

    }
}
