using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private readonly SerialDisposable acceptingInputDisposable = new SerialDisposable();

    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private GameBoardSpawner board;
    private ReactiveProperty<Vector2Int?> currentHoldPos = new();
    [SerializeField] public int testingCurrentPlayerID = 0;
    //[SerializeField] private List<Vector2Int> positionSelections = new();
    public ReactiveProperty<List<Vector2Int>> positionSelectionsObservable = new(new());
    public readonly ReactiveProperty<HalfTurnType?> currentDragKind = new();
    public readonly Subject<(HalfTurnType kind, Vector2Int[] positions)> OnSelectionCompleted = new();
        
    public void Start()
    {
        Observable.EveryUpdate().Subscribe(AcceptingInputSelectionLoop).AssignTo(acceptingInputDisposable);


        currentHoldPos.Subscribe(OnHoldPositionChanged).AddTo(this);
    }
    
    private void AcceptingInputSelectionLoop()
    {
        if (currentDragKind.CurrentValue is null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) TryBeginDrag(HalfTurnType.Move);
            else if (Input.GetKeyDown(KeyCode.Mouse1)) TryBeginDrag(HalfTurnType.Attack);
            return;
        }

        var heldButton = currentDragKind.CurrentValue == HalfTurnType.Move ? KeyCode.Mouse0 : KeyCode.Mouse1;

        if (Input.GetKeyUp(heldButton))
        {
            currentHoldPos.Value = null;
            return;
        }

        if (Input.GetKey(heldButton))
        {
            var getPos = SelectPos();
            if (getPos.HasValue && getPos != currentHoldPos.CurrentValue)
            {
                currentHoldPos.Value = getPos;
                Debug.Log(getPos.Value);
            }
        }
    }

    private void TryBeginDrag(HalfTurnType kind)
    {
        var getPos = SelectPos();
        if (!getPos.HasValue) return;
        if (!IsAdjacentToPlayer(getPos.Value, testingCurrentPlayerID))
        {
            Debug.Log($"{getPos.Value} is not adjacent to player {testingCurrentPlayerID}");
            return;
        }
        currentDragKind.Value = kind;
        currentHoldPos.Value = getPos;
    }

    private void OnHoldPositionChanged(Vector2Int? newPos)
    {
        if (newPos.HasValue)
        {
            AddSelection(newPos.Value);
        }
        else if (positionSelectionsObservable.CurrentValue.Count > 0)
        {
            OnSelectionCompleted.OnNext((currentDragKind.CurrentValue!.Value,
                                         positionSelectionsObservable.CurrentValue.ToArray()));
            positionSelectionsObservable.Value = new List<Vector2Int>();
            currentDragKind.Value = null;
        }
    }

    private void AddSelection(Vector2Int newValue)
    {
        var currentList = positionSelectionsObservable.CurrentValue;
        currentList.Add(newValue);
        positionSelectionsObservable.Value = currentList;
    }
    
    private bool IsAdjacentToPlayer(Vector2Int selectedPos, int playerID)
    {
        var playerPos = board.GetPlayerPosition(playerID);
        if (!playerPos.HasValue) 
            return false;

        var isApprox = Mathf.Approximately((playerPos.Value - selectedPos).magnitude, 1);
        Debug.Log( $"{playerPos.Value} is adjacent to player {playerID}, {selectedPos}, {isApprox}");
        return isApprox;
    }

    private Vector2Int? SelectPos()
    {
        var ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, raycastLayerMask))
            return null;
        return GameBoardSpawner.CalcPosition(hit.collider.transform.position);
    }
}