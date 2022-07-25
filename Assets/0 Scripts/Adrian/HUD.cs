using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    public static HUD ins;
    public Image fill_bar;
    private void Awake()
    {
        ins = this;
    }
}
