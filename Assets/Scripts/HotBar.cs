using UnityEngine;
using UnityEngine.UI;

public class HotBar : MonoBehaviour
{
    public bool CreateLock;
    private Sprite[] hotBarSprite;
    public Sprite[] spriteNN;
    [SerializeField] private Sprite[] spriteN;
    [SerializeField] private Sprite emptySprite;
    public bool[] HotBarSelect = new bool[9];
    private void OnMouseEnter() => CreateLock = true;
    private void OnMouseExit() => CreateLock = false;
    public int hotBarButtonSelect;
    private void Start()
    {
        spriteNN = spriteN;
        hotBarSprite = ItemList.buildingsIcon;
    }
    private void Update()
    {
        for (var i = 0; i < HotBarSelect.Length; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite = spriteN[i] == null ? emptySprite : spriteN[i];
            transform.GetChild(i).localScale = HotBarSelect[i] ? new Vector2(1.1f, 1.1f) : new Vector2(1, 1);
        }
    }
}
