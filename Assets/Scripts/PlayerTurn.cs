using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerTurn
{
    public PlayerCharacter playerCharacter;

    public HalfTurn halfTurn = new();
}


[Serializable]
public class HalfTurn
{
    public HalfTurnType MoveType = HalfTurnType.None;
    public List<Vector2Int> Positions = new();
}


[Serializable]
public enum HalfTurnType
{
    None = 0,
    Move = 1,
    Attack = 2,
    Build = 3,
}


