
using UnityEngine;

public class Generator : MonoBehaviour
{
    LineRenderer line;
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }
    
    void Update()
    {
        if (Buildings._objectInGround.GetTile(Vector3Int.FloorToInt(gameObject.transform.position)) != null && Buildings._objectInGround.GetTile(Vector3Int.FloorToInt(gameObject.transform.position)) != ItemList.buildings[6]);
        Debug.Log(Vector3Int.FloorToInt(new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y + 1)));
        for (float x = transform.position.x - 5; x < transform.position.x + 5; x++)
        {
            for (float y = transform.position.x - 5; y < transform.position.y + 5; y++)
            {
                if (Buildings._objectInGround.GetTile(Vector3Int.FloorToInt(new Vector2(x, y))) != null && Buildings._objectInGround.GetTile(Vector3Int.FloorToInt(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y))) != ItemList.buildings[6])
                {
                    line.SetPosition(0, gameObject.transform.position);
                    line.SetPosition(1, new Vector2(x - 1, y - 1));
                }
            }
        }
    }
}
