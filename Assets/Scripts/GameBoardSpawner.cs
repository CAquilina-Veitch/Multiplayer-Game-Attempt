using System;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardSpawner : MonoBehaviour
{
    [SerializeField] private GameTile gameTile;
    [SerializeField] private List<GameTile> gameTileList = new();
    
    private readonly Dictionary<Vector2Int, GameTile> tiles = new();

    /*private void OnValidate()
    {
        gameTileList = new();
        foreach (Transform child in transform) 
            gameTileList.Add(child.GetComponent<GameTile>());
    }*/

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        for (int x = 0; x < 7; x++)
        for (int z = 0; z < 7; z++)
        {
            var spawnedObject = Instantiate(gameTile, new Vector3(x-3, 0, z-3), Quaternion.identity, transform);
            spawnedObject.Initialise((x + z) % 2);
            spawnedObject.gameObject.name = $"Tile {x},{z}";
        }
    }

    private void Awake()
    {
        foreach (var tile in gameTileList) 
            tiles.Add(CalcPosition(tile.transform.position), tile);
    }

    private void OnDrawGizmosSelected()
    {
        for (int x = 0; x < 7; x++)
        for (int z = 0; z < 7; z++)
        {
            Gizmos.color = (x + z) % 2 == 0 ? Color.white : Color.black;
            Gizmos.DrawWireCube(new Vector3(x - 3f, 0, z - 3f), Vector3.one);
        }
    }

    public static Vector2Int CalcPosition(Vector3 vector3) => 
        new(Mathf.RoundToInt(vector3.x) + 3, Mathf.RoundToInt(vector3.z) + 3);
}
