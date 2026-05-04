using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public int damage;
    public Rigidbody body;
    public bool impacted;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ResolveImpact(collision);
    }

    public virtual void ResolveImpact(Collision collision)
    {
        if (impacted) return;
        impacted = true;

        if (collision.gameObject.GetComponent<Stalker>() != null)
        {
            Stalker stalker = collision.gameObject.GetComponent<Stalker>();
            stalker.ApplyHit(damage);
            Instantiate(Resources.Load<GameObject>("Effects/Blood"), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (collision.gameObject.GetComponent<Protagonist>() != null)
        {
            Debug.Log("Projectile hit player");
            impacted = false;
            return;
        }
        else if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }

        body.isKinematic = true;
        transform.SetParent(collision.transform);
    }
}
