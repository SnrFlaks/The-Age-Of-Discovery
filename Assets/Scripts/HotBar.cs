using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class HotBar : MonoBehaviour
{
    public static bool CreateLock;
    private TileBase[] hotBarTile;
    private Sprite[] hotBarSprite;
    [SerializeField] private Sprite emptySprite;
    public static readonly bool[] HotBarSelect = new bool[9];
    private void OnMouseEnter() => CreateLock = true;
    private void OnMouseExit() => CreateLock = false;
    private void Start() {
        hotBarSprite = ItemList.buildingsIcon;
    }
    private void Update() {
        for (var i = 0; i < HotBarSelect.Length; i++) {
            transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite = (hotBarSprite[i] == null) ? emptySprite : hotBarSprite[i];
            transform.GetChild(i).localScale = HotBarSelect[i] ? new Vector2(1.1f, 1.1f) : new Vector2(1, 1);
        }
    }
}
