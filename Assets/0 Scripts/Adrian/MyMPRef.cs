using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Globalization;
public class MyMPRef : MonoBehaviour
{
    public int my_id, capturedFlags;
    public float my_healt;
    public PhotonView pv;
    public Transform gun;
    public bool playAgain = false;

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
        HUD.ins.fill_bar.fillAmount = my_healt / 100;
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
