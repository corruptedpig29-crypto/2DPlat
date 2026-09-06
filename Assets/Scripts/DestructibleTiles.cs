using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class DestructibleTiles : MonoBehaviour
{
    public Tilemap destructibletilemap;
    public Vector3Int cellPosition;
    public Vector3Int cellPosition2;
    public Vector3Int cellPosition3;
    public Vector3Int cellPosition4;
    public Vector3Int cellPosition5;
    public Vector3Int cellPosition6;
    public Vector3Int cellPosition7;
    public Vector3Int cellPosition8;
    public Vector3Int cellPosition9;

    CompositeCollider2D CompositeCollider2D;

    List<Vector3> points = new();    // Start is called before the first frame update
    void Start()
    {

        CompositeCollider2D = GetComponent<CompositeCollider2D>();
        destructibletilemap = GetComponent<Tilemap>();
    }
    private void OnDrawGizmos()
    {
        

    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {


        HashSet<Vector3Int> uniqueCells = new HashSet<Vector3Int>(); // To store unique cell positions
        if (collision.gameObject.CompareTag("CanBreakWall"))
        {

            foreach (ContactPoint2D hit in collision.contacts)
            {

                for(int i =-2; i <= 2; i++) { 
                    for(int j = -2; j <= 2; j++)
                    {
                        Vector2 offset = new Vector2(i - 1, j - 1); // Offsets to check the surrounding tiles
                        Vector2 checkPosition = hit.point + offset * destructibletilemap.cellSize; // Calculate the position to check
                        Vector3Int cellPositionToCheck = destructibletilemap.WorldToCell(checkPosition); // Convert to cell position


                        uniqueCells.Add(cellPositionToCheck); // Add the cell position to the HashSet

                    }
                }


            }
        }


        foreach (Vector3Int cellPositionToCheck in uniqueCells)
        {
            if (destructibletilemap.HasTile(cellPositionToCheck))
            {
                destructibletilemap.SetTile(cellPositionToCheck, null); // Destroy the tile
                destructibletilemap.RefreshTile(cellPositionToCheck); // Refresh the tilemap
            }
        }


    }


    //constantly destroy in front of it!!!
}