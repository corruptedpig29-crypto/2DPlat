using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class spawnOnContact : MonoBehaviour
{
    [SerializeField] Tilemap destructibletilemap;
    [SerializeField] Tile tileToSpawn;
    BoxCollider2D coll;
    Vector3 min;
    Vector3 max;
    // Start is called before the first frame update
    Bounds bounds;
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();


    }

    // Update is called once per frame
    void Update()
    {
        bounds = coll.bounds;
        Vector3Int min = destructibletilemap.WorldToCell(bounds.min);
        Vector3Int max = destructibletilemap.WorldToCell(bounds.max);
        for (int i = (int)min.x; i <= max.x; i++)
        {
            for (int j = (int)min.y; j <= max.y; j++)
            {
                Vector3Int pose = new Vector3Int(i, j, 0);
                Vector2 center = destructibletilemap.GetCellCenterWorld(pose);

                if (coll.OverlapPoint(center))
                {
                    Vector3Int pos = new Vector3Int(i, j, 0);
                    if(!destructibletilemap.HasTile(pos))destructibletilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), tileToSpawn);
                }
            }
        }
    }
}
