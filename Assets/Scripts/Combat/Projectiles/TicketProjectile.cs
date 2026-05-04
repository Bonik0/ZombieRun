using UnityEngine;

public class TicketProjectile : ProjectileBehavior
{
    [SerializeField] private int penetrationCount;
    [SerializeField] private int penetrationLimit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Stalker>() != null)
        {
            Stalker stalker = other.gameObject.GetComponent<Stalker>();
            stalker.ApplyHit(damage);
            Instantiate(Resources.Load<GameObject>("Effects/Blood"), transform.position, Quaternion.identity);
            penetrationCount++;
            if (penetrationCount >= penetrationLimit) Destroy(gameObject);
        }
        else if (other.gameObject.GetComponent<Protagonist>() != null)
        {
            Debug.Log("Projectile hit player");
            impacted = false;
            return;
        }
        else if (other.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }

        transform.SetParent(other.transform);
    }
}
