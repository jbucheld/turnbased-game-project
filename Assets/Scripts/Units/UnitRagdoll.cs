using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransforms(originalRootBone,  ragdollRootBone);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                
                MatchAllChildTransforms(child, cloneChild);
                ApplyExplosionToRagdoll(ragdollRootBone, 30f, transform.position, 2f);
            }
        }
    }

    private void ApplyExplosionToRagdoll(
        Transform root, 
        float explosionForce, 
        Vector3 explosionPosition, 
        float explosionRange)
    {
        foreach (Transform child in root)
        {
            
            if (child.TryGetComponent(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce,explosionPosition,explosionRange);
            }
            
            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
