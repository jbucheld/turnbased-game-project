using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitslist;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitslist = new List<Unit>();
    }

    public override string ToString()
    {
        // return gridPosition.ToString() + "\n" + unit.ToString();
        string unitString = "";
        foreach (Unit unit in unitslist)
        {
            unitString += unit + "\n";
        }
        return $"{gridPosition.ToString()}\nUnits: {unitString}";
    }

    public void AddUnit(Unit unit)
    {
        unitslist.Add(unit);
    }

    public List<Unit> GetUnitslist()
    {
        return unitslist;
    }

    public void RemoveUnit(Unit unit)
    {
        unitslist.Remove(unit);
    }
    
}
