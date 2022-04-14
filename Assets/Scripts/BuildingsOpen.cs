using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BuildingsOpen : MonoBehaviour
{
    [SerializeField] private Tilemap buildingsInMap;
    [SerializeField] private TileBase[] buildings;
    [SerializeField] private GameObject buildMenu;
    private Text _buildName;

    private void Start() {
        _buildName = buildMenu.transform.GetChild(0).GetComponent<Text>();
    }

    void Update() {
        var point = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellP = buildingsInMap.WorldToCell(point);
        //Debug.Log(buildingsInMap.GetTile(cellP) + "\t" + buildings[0]);
        if (Input.GetMouseButtonDown(0) && buildingsInMap.GetTile(cellP) == buildings[0])
        {
            buildMenu.SetActive(true);
            _buildName.text = "Бур";
        }
    }
    public void Back() => buildMenu.SetActive(false);
}