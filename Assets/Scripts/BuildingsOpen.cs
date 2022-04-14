using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BuildingsOpen : MonoBehaviour
{
    [SerializeField] private Tilemap _buildings;
    [SerializeField] private TileBase[] buildings;
    [SerializeField] private GameObject buildMenu;
    private Text _buildName;

    private void Start() {
        _buildName.text = buildMenu.transform.GetChild(0).GetComponent<Text>().text;
    }

    void Update()
    {
        var point = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellP = _buildings.WorldToCell(point);
        Debug.Log(_buildings.GetTile(cellP) + "\t" + buildings[0]);
        if (Input.GetMouseButtonDown(0) && _buildings.GetTile(cellP) == buildings[0])
        {
            _buildName.text = "Бур";
            buildMenu.SetActive(true);
                
        }
    }
    public void Back() => buildMenu.SetActive(false);
    
}
