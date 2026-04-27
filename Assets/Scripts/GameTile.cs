using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material[] materials;

    public void Initialise(int material)
    {
        meshRenderer.material = materials[material];
    }
    
}
