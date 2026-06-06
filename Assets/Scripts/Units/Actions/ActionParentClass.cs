using System;
using UnityEngine;

public abstract class ActionParentClass : MonoBehaviour
{
    protected Unit parentUnit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        parentUnit = GetComponent<Unit>();
    }

    public abstract string GetActionName();
}
