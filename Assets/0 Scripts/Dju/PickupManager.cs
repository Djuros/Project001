using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public MyMPRef mymp;
    public Transform flag_anchor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("flag")) {
            if (flag_anchor.childCount > 0) { return; }
            other.transform.parent = flag_anchor;
            other.transform.localPosition = Vector3.zero;
            other.transform.localEulerAngles = new Vector3(0,0,0);
            other.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
    }
}
