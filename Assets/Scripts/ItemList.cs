using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemList : MonoBehaviour
{
    [SerializeField] private TileBase[] buildingsIns;
    public static TileBase[] buildings;
    [SerializeField] private Sprite[] buildingsIconIns;
    public static Sprite[] buildingsIcon;
    [SerializeField] private string[] buildingsNameIns;
    public static string[] buildingsName;
    [Header("Buildings Level")]
    [SerializeField] private SubList[] upgradeCost = new SubList[6];
    public static SubList[] upgradeCostStat = new SubList[6];
    [Serializable]
    public class SubList {
        public int[] level2Costs = new int[9];
        public int[] level3Costs = new int[9];
        public int[] level4Costs = new int[9];
        public int[] level5Costs = new int[9];
        public int[] level6Costs = new int[9];
        public int[] level7Costs = new int[9];
        public Sprite[] levelSprite = new Sprite[6];
    }
    private void Awake() {
        buildings = buildingsIns;
        buildingsIcon = buildingsIconIns;
        buildingsName = buildingsNameIns;
        upgradeCostStat = upgradeCost;
        for (var i = 0; i < buildingsNameIns.Length; i++) {
            buildingsNameIns[i] = buildingsIns[i].name;
        }
    }
}
