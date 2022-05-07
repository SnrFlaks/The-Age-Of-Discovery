using UnityEngine;

public class Generator : MonoBehaviour
{
    void Start()
    {
    }
    
    void Update()
    {
        if (Buildings._objectInGround.GetTile(Vector3Int.FloorToInt(transform.position)) != null || Buildings._objectInGround.GetTile(Vector3Int.FloorToInt(transform.position)) != ItemList.buildings[6]) Destroy(gameObject);
        
    }
}
