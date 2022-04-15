using System;
using UnityEngine;
using UnityEngine.UI;
public class HotBarButton : MonoBehaviour {
    public void HotButton(Sprite emptySprite)
    {
        Debug.Log(transform.GetChild(0).GetComponent<Image>().overrideSprite);
        for (var i = 0; i < HotBar.HotBarSelect.Length; i++) 
            HotBar.HotBarSelect[i] = transform.GetChild(0).GetComponent<Image>().overrideSprite != emptySprite && i == Convert.ToInt32(transform.name[transform.name.Length - 1].ToString())-1;
    }
}
