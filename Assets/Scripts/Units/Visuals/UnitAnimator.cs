using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform bulletEjectPoint;
    private AudioSource audioSource;
    
    private void Awake()
    {
        if (TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnUnitStartMoving += MoveAction_OnUnitStartMoving;
            moveAction.OnUnitStopMoving += MoveAction_OnUnitStopMoving;
        }

        if (TryGetComponent(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
        
        audioSource = GetComponent<AudioSource>();
    }

    private void MoveAction_OnUnitStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }
    
    private void MoveAction_OnUnitStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }
    
    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs args)
    {
        animator.SetTrigger("Shoot");
        audioSource.Play();

        Transform bullet = Instantiate(bulletProjectilePrefab, bulletEjectPoint.position, Quaternion.identity);
        BulletProjectile projectileInstance = bullet.GetComponent<BulletProjectile>();
        Vector3 targetFixedPosition = args.targetUnit.transform.position;
        targetFixedPosition.y = bulletEjectPoint.transform.position.y;
        projectileInstance.Setup(targetFixedPosition);
    }
}
