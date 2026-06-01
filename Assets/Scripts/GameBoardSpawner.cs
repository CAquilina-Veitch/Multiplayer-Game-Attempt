using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBoardSpawner : MonoBehaviour
{
    [SerializeField] private GameTile gameTile;
    [SerializeField] private PlayerCharacter playerCharacterPrefab;
    [SerializeField] private List<GameTile> gameTileList = new();
    
    [SerializeField] public List<PlayerCharacter> players = new();
    
    private readonly Dictionary<Vector2Int, GameTile> tiles = new();
    
    [ContextMenu("Spawn")]
    public void SpawnBoard()
    {
        for (int x = 0; x < 7; x++)
        for (int z = 0; z < 7; z++)
        {
            var spawnedObject = Instantiate(gameTile, new Vector3(x-3, 0, z-3), Quaternion.identity, transform);
            spawnedObject.Initialise((x + z) % 2);
            spawnedObject.gameObject.name = $"Tile {x},{z}";
        }
        gameTileList = new();
        foreach (Transform child in transform) 
            gameTileList.Add(child.GetComponent<GameTile>());
    }


    public void SpawnPlayer(int id, Vector2Int position)
    {
        var newPlayer = Instantiate(playerCharacterPrefab, BoardPosition(position), Quaternion.identity, transform);
        newPlayer.ID = id;
        newPlayer.gameObject.name = $"Player {id}";
        newPlayer.Position = position;
        players.Add(newPlayer);
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
    
    public static Vector3 BoardPosition(Vector2Int position) =>
        new(position.x - 3, 1, position.y - 3);

    public Vector2Int? GetPlayerPosition(int id)
    {
        var player = players.FirstOrDefault(pc => pc.ID == id);
        if (player == null)
        {
            Debug.LogWarning("player null");
            return null;
        }
        return player.Position;
    }
}
