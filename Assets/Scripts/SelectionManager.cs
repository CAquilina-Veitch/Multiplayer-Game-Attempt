using System;
using R3;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    
    private readonly SerialDisposable acceptingInputDisposable = new SerialDisposable();

    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask raycastLayerMask;
    
    
    public void Start()
    {
        Observable.EveryUpdate().Subscribe(AcceptingInputSelectionLoop).AssignTo(acceptingInputDisposable);
    }
    
    
    private void AcceptingInputSelectionLoop()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0)) 
            return;

        var ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, raycastLayerMask))
            Debug.Log("Nothing Hit");
        else
        {
            Debug.Log($"Selected {hit.collider.transform.position}");

            var tappedPos = GameBoardSpawner.CalcPosition(hit.collider.transform.position);

            Debug.Log($"{tappedPos} tapped.");

        }
    }




    
    
    
}