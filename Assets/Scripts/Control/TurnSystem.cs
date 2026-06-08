using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int turnNumber = 1;
    public event EventHandler OnTurnChanged;
    private bool isPlayerTurn = true;

    
    public static TurnSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one instance of TurnSystem!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
