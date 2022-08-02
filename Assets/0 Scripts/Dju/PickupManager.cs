using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PickupManager : MonoBehaviour
{
    public MyMPRef mymp;
    public Transform flag_anchor, flag;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("flag")) {
            if (flag_anchor.childCount > 0) { return; }
            flag = other.transform;
            flag.parent = flag_anchor;
            flag.localPosition = Vector3.zero;
            flag.localEulerAngles = new Vector3(0,0,0);
            flag.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
        else if (other.CompareTag("FlagDelivery"))
        {
            if (flag_anchor.childCount == 0) { return; }
            flag.parent = other.transform;
            flag.localPosition = Vector3.zero;
            flag.localEulerAngles = new Vector3(0, 0, 0);
            flag.localScale = new Vector3(1f, 1f, 1f);
            MessagingSystem.ins.me.score++;
        }
        else if (other.CompareTag("Damage"))
        {
            // instantiate stronger bullets at bullet pool
            BulletPool.ins.Switch_To_Stronger_Bullets();
            PhotonNetwork.Destroy(other.transform.parent.GetComponent<PhotonView>());
        }
        else if (other.CompareTag("Health"))
        {
            MessagingSystem.ins.me.current_health += 50;
            MessagingSystem.ins.me.max_health += 50;
            PhotonNetwork.Destroy(other.transform.parent.GetComponent<PhotonView>());
        }
        else if (other.CompareTag("FireRate"))
        {
            MessagingSystem.ins.me.pc._fireRate += 5;
            PhotonNetwork.Destroy(other.transform.parent.GetComponent<PhotonView>());
        }
        else if (other.CompareTag("Speed"))
        {
            MessagingSystem.ins.me.pc._walkSpeed += 5;
            MessagingSystem.ins.me.pc._runSpeed += 5;
            PhotonNetwork.Destroy(other.transform.parent.GetComponent<PhotonView>());
        }
    }
}
