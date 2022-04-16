using System;
using UnityEngine;
public class HotBarButton : MonoBehaviour {
    public void HotButton()
    {
        for (var i = 0; i < HotBar.HotBarSelect.Length; i++) HotBar.HotBarSelect[i] = i == Convert.ToInt32(transform.name[transform.name.Length - 1].ToString())-1;
    }
}
