using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private List<Unit> unitList;
    private List<Unit> enemyUnitList;
    private List<Unit> playerUnitList;

    public static UnitManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of UnitManager!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        unitList = new List<Unit>();
        enemyUnitList  = new List<Unit>();
        playerUnitList = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDied += Unit_OnAnyUnitDied;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Add(unit);
        
        if (unit.IsEnemy()) enemyUnitList.Add(unit);
        else playerUnitList.Add(unit);
    }

    private void Unit_OnAnyUnitDied(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Remove(unit);
        
        if (unit.IsEnemy()) enemyUnitList.Remove(unit);
        else playerUnitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    
    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }
    
    public List<Unit> GetPlayerUnitList()
    {
        return playerUnitList;
    }
}
