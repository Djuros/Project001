using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon;
public class RangeAttack : MonoBehaviour
{
    public Transform muzzle;
    public LayerMask hit_layer;
    public PhotonView current_pv_target;
    private void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            RaycastHit hit;

            if (Physics.Raycast(muzzle.position, muzzle.forward, out hit,10000, hit_layer))
            {
                if (current_pv_target == null) {
                    current_pv_target = hit.transform.parent.gameObject.GetPhotonView();
                }
                else {
                    if (hit.transform != current_pv_target.transform.GetChild(0).transform)
                    { // target doesnt match with previous
                        current_pv_target = hit.transform.parent.gameObject.GetPhotonView();
                    }
                    else {
                        print("TARGET IS THE SAME");
                    }
                }
                print(current_pv_target.ViewID);
            }
        }
    }
}
