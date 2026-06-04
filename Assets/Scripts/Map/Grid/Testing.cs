using System;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Unit unit;
   
    private void Start()
    {
        
    }

    private void Update()
    {
        unit = UnitActionSystem.Instance.GetSelectedUnit();    
        
    }
}
