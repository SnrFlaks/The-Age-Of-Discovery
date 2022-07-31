using UnityEngine;
using UnityEngine.UI;

public class HotBar : MonoBehaviour
{
    public static bool CreateLock;
    private Sprite[] hotBarSprite;
    public static Sprite[] spriteNN;
    [SerializeField] private Sprite[] spriteN;
    [SerializeField] private Sprite emptySprite;
    public static readonly bool[] HotBarSelect = new bool[9];
    private void OnMouseEnter() => CreateLock = true;
    private void OnMouseExit() => CreateLock = false;
    private void Start() {
        spriteNN = spriteN;
        hotBarSprite = ItemList.buildingsIcon;
    }
    private void Update() {
        for (var i = 0; i < HotBarSelect.Length; i++) {
            transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite = spriteN[i] == null ? emptySprite : spriteN[i];
            transform.GetChild(i).localScale = HotBarSelect[i] ? new Vector2(1.1f, 1.1f) : new Vector2(1, 1);
        }
    }
}
