using System;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField] private float projectileSpeed = 200f;
    [SerializeField] private TrailRenderer trailRenderer; 
    [SerializeField] private Transform hitVFXPrefab;
    
    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - this.transform.position).normalized;
        
        float distanceBeforeMoving = Vector3.Distance(this.transform.position, targetPosition);
        transform.position += moveDirection * (projectileSpeed * Time.deltaTime);
        float distanceAfterMoving = Vector3.Distance(this.transform.position, targetPosition);
        
        if  (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;
            trailRenderer.transform.parent = null;
            Instantiate(hitVFXPrefab, targetPosition, Quaternion.identity);
            Destroy(gameObject);
        };
        
    }
}
