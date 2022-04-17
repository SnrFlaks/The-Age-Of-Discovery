using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BuildingsOpen : MonoBehaviour
{
    [SerializeField] private Tilemap buildingsInMap;
    private TileBase[] _buildings;
    [SerializeField] private GameObject buildMenu;
    private Text _buildName;

    private void Start()
    {
        _buildings = ItemList.buildings;
        _buildName = buildMenu.transform.GetChild(0).GetComponent<Text>();
    }
    
    void Update() {
        var point = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellP = buildingsInMap.WorldToCell(point);
        if (!Input.GetMouseButtonDown(0) || buildingsInMap.GetTile(cellP) != _buildings[0]) return;
        buildMenu.SetActive(true);
        _buildName.text = "Бур:";
    }
    public void Back() => buildMenu.SetActive(false);
}