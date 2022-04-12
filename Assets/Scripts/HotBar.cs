using System;
using UnityEngine;

public class HotBar : MonoBehaviour
{
    public static bool CreateLock;
    public static readonly bool[] HotBarSelect = new bool[9];
    private void OnMouseEnter() => CreateLock = true;
    private void OnMouseExit() => CreateLock = false;
    private void Update() {
        for (var i = 0; i < HotBarSelect.Length; i++) {
            transform.GetChild(i).localScale = HotBarSelect[i] ? new Vector2(1.1f, 1.1f) : new Vector2(1, 1);
        }
    }
}
