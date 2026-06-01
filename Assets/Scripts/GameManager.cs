using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameTurn> gameHistory =  new();
    public GameTurn currentGameTurn = new();
    public GameBoardSpawner GameBoardSpawner;
    public SelectionManager SelectionManager;
    
    public int NumberOfPlayersInGame = 2;

    private int testingNumberOfPlayers = 2;
    private readonly Vector2Int[] startingPositions = { new Vector2Int(3,0), new Vector2Int(3,6) };
    
    
    public Observable<GameTurn> OnGameTurnSubmitted => onGameTurnSubmitted;
    private readonly Subject<GameTurn> onGameTurnSubmitted = new();

    private void Start()
    {

        for(int i = 0; i < NumberOfPlayersInGame; i++)
        {
            GameBoardSpawner.SpawnPlayer(i, startingPositions[i]);
        }
        
        StartNewTurn();
        
    }
    

    public void StartNewTurn()
    {
        currentGameTurn = new GameTurn();


        SelectionManager.OnSelectionCompleted.Subscribe(SelectionCompleted).AddTo(this);
    }

    private void SelectionCompleted((HalfTurnType kind, Vector2Int[] positions) sel)
    {
        var turn = new PlayerTurn();
        turn.playerCharacter = GameBoardSpawner.players.First(p=>p.ID == SelectionManager.testingCurrentPlayerID);
        turn.halfTurn.MoveType = sel.kind;
        turn.halfTurn.Positions = sel.positions.ToList();
        SubmitPlayerTurn(turn);
    }

    public void SubmitPlayerTurn(PlayerTurn turn)
    {
        if (!turn.playerCharacter.ID.HasValue)
        {
            Debug.LogWarning("turn submitted with no playerid?");
            return;
        }

        if (currentGameTurn.HasPlayerSubmitted(turn.playerCharacter.ID.Value))
        {
            Debug.LogWarning($"player {turn.playerCharacter.ID} already submitted");
            return;
        }
        else
        {
            currentGameTurn.AddPlayerTurn(turn);
            if (currentGameTurn.IsTurnComplete(NumberOfPlayersInGame))
            {
                Debug.Log($"turn finished! all players submitted");
                onGameTurnSubmitted.OnNext(currentGameTurn);
            }
        }
    }

    private void OnCurrentTurnComplete()
    {
        MarkCurrentTurnAsComplete();
    }

    private void MarkCurrentTurnAsComplete()
    {
        gameHistory.Add(currentGameTurn);
        currentGameTurn = new();
    }
}


[Serializable]
public class GameTurn
{
    public List<PlayerTurn> playersTurns = new();

    public void AddPlayerTurn(PlayerTurn turn)
    {
        if (playersTurns.Any(pt => pt.playerCharacter == turn.playerCharacter))
            Debug.LogWarning($"player {turn.playerCharacter.name} has already submitted turn");
        playersTurns.Add(turn);
    }

    public bool HasPlayerSubmitted(int playerID) => playersTurns.Any(pt => pt.playerCharacter.ID == playerID);

    public void RemovePlayerTurn(int playerID) => playersTurns.RemoveAll(pt => pt.playerCharacter.ID == playerID);

    public bool IsTurnComplete(int expectedSubmittedPlayerCount) => playersTurns.Count == expectedSubmittedPlayerCount;
}
