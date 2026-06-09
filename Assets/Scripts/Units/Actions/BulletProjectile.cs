using System;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField] private float projectileSpeed = 200f;
    
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
        
        if  (distanceBeforeMoving < distanceAfterMoving) Destroy(gameObject);

        
    }
}
