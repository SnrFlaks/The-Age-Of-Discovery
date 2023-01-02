using System;
using UnityEngine;
public class HotBarButton : MonoBehaviour
{
    // public void HotButton()
    // {
    //     for (var i = 0; i < HotBar.HotBarSelect.Length; i++) HotBar.HotBarSelect[i] = i == Convert.ToInt32(transform.name[transform.name.Length - 1].ToString())-1;
    // }
    [SerializeField] private HotBar hotBar;
    private void Update()
    {
        int lS = Convert.ToInt32(transform.name[^1].ToString()) - 1;
        KeyCode n = lS == 0 ? KeyCode.Alpha1 : lS == 1 ? KeyCode.Alpha2 : lS == 2 ? KeyCode.Alpha3 : lS == 3 ? KeyCode.Alpha4 : lS == 4 ? KeyCode.Alpha5 : lS == 5 ? KeyCode.Alpha6 : lS == 6 ? KeyCode.Alpha7 : lS == 7 ? KeyCode.Alpha8 : KeyCode.Alpha9;
        if (!Input.GetKeyDown(n)) return;
        for (var i = 0; i < hotBar.HotBarSelect.Length; i++)
        {
            hotBar.hotBarButtonSelect = i == lS ? lS : hotBar.hotBarButtonSelect;
            hotBar.HotBarSelect[i] = i == lS;
        }
    }
}
