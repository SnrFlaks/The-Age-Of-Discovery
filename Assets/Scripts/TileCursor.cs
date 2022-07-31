using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TileCursor : MonoBehaviour
{
    private Camera mainCam;
    private Tilemap _ground;
    [SerializeField] private GameObject hotBar;
    [SerializeField] private Sprite cannon1;
    [SerializeField] private Sprite empty;

    private void Start()
    {
        mainCam = Camera.main;
        _ground = transform.parent.GetChild(0).GetComponent<Tilemap>();
    }

    private void Update()
    {
        var cellPosition = _ground.WorldToCell(mainCam.ScreenToWorldPoint(Input.mousePosition));
        gameObject.transform.position = new Vector3(cellPosition.x + 0.5f, cellPosition.y + 0.5f, 0);
        for (var i = 0; i < 9; i++)
        {
            if (!HotBar.HotBarSelect[i]) continue;
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = hotBar.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite;
            transform.GetChild(1).GetComponent<SpriteRenderer>().color = HotBar.spriteNN[i] == cannon1 ? new Color(1,1,1,0.2f) : new Color(1f, 1f, 1f, 0f);
        }
    }
}
