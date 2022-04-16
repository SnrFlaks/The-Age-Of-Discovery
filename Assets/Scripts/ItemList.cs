using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemList : MonoBehaviour
{
    [SerializeField] private TileBase[] buildingsIns;
    public static TileBase[] buildings;
    [SerializeField] private Sprite[] buildingsIconIns;
    public static Sprite[] buildingsIcon;
    private void Start() {
        buildings = buildingsIns;
        buildingsIcon = buildingsIconIns;
    }
}
