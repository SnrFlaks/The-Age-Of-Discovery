using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemList : MonoBehaviour
{
    [SerializeField] private TileBase[] buildingsIns;
    public static TileBase[] buildings;
    [SerializeField] private Sprite[] buildingsIconIns;
    public static Sprite[] buildingsIcon;
    [Header("Buildings Level")]
    [SerializeField] private SubList[] upgradeCost = new SubList[6];
    public static SubList[] upgradeCostStat = new SubList[6];
    [Serializable]
    public class SubList {
        public int[] level1Costs = new int[8];
        public int[] level2Costs = new int[8];
        public int[] level3Costs = new int[8];
        public int[] level4Costs = new int[8];
        public int[] level5Costs = new int[8];
        public int[] level6Costs = new int[8];
        public int[] level7Costs = new int[8];
        public Sprite[] levelSprite = new Sprite[7];
    }
    private void Awake() {
        buildings = buildingsIns;
        buildingsIcon = buildingsIconIns;
        upgradeCostStat = upgradeCost;
    }
}
