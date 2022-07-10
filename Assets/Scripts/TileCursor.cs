using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TileCursor : MonoBehaviour
{
    private Camera mainCam;
    private Tilemap _ground;
    [SerializeField] private GameObject hotBar;
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
        }
    }
}
