using UnityEngine;

public class ArmamentBase : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Stats")]
    [SerializeField] private float fireInterval;
    private float cooldownTimer = 1000f;

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            if (cooldownTimer >= fireInterval)
            {
                FireProjectile();
                cooldownTimer = 0f;
            }
        }
    }

    private void FireProjectile()
    {
        Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
    }
}
