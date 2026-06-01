using UnityEngine;

public class PlayerStatBar : MonoBehaviour
{
    [SerializeField] private UIFillIcon[] hearts;
    [SerializeField] private UIFillIcon[] energys;

    private void Awake()
    {
        //subscribe to health observable
        
    }

    private void SetMaxHealth(int maxHealth)
    {
        if (maxHealth > hearts.Length) 
            Debug.LogError($"Max Health: {maxHealth} but only {hearts.Length} hearts.");
        for (int i = 0; i < hearts.Length; i++) 
            hearts[i].gameObject.SetActive(i < maxHealth);
    }

    private void SetHealth(int health)
    {
        if (health > hearts.Length) 
            Debug.LogError($"Max Health: {health} but only {hearts.Length} hearts.");
        for (int i = 0; i < hearts.Length; i++) 
            hearts[i].SetFilled(i < health);
    }

    private void SetMaxEnergy(int maxEnergy)
    {
        if (maxEnergy > hearts.Length) 
            Debug.LogError($"Max Health: {maxEnergy} but only {hearts.Length} hearts.");
        for (int i = 0; i < hearts.Length; i++) 
            hearts[i].gameObject.SetActive(i < maxEnergy);
    }
    
    private void SetEnergy(int energy)
    {
        if (energy > energys.Length) 
            Debug.LogError($"Max Health: {energy} but only {energys.Length} hearts.");
        for (int i = 0; i < energys.Length; i++) 
            energys[i].SetFilled(i < energy);
    }
    
}