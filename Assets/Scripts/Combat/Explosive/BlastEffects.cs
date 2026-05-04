using System.Collections;
using UnityEngine;

public class BlastEffects : MonoBehaviour
{
    private int damage;
    private BlastProjectile sourceProjectile;
    [SerializeField] private float lifespan;

    private void Start()
    {
        sourceProjectile = FindAnyObjectByType<BlastProjectile>();
        damage = sourceProjectile.damage;
        Destroy(sourceProjectile);
        StartCoroutine(Expire());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Combatant>() != null)
        {
            if (other.gameObject.GetComponent<Stalker>() != null)
            {
                Stalker stalker = other.gameObject.GetComponent<Stalker>();
                stalker.ApplyHit(damage);
            }
            else if (other.gameObject.GetComponent<Combatant>() != null)
            {
                Combatant combatant = other.gameObject.GetComponent<Combatant>();
                combatant.ApplyHit(damage / 10);
            }

            StartCoroutine(Expire());
        }
    }

    private IEnumerator Expire()
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        HudRoot.Instance.pickupBanner.SetActive(false);
    }
}
