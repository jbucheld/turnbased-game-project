using System;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{

    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootBone;
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnUnitDeath += HealthSystem_OnUnitDeath;
    }

    private void HealthSystem_OnUnitDeath(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);
    }
}
