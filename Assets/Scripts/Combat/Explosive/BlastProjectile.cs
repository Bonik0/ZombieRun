using UnityEngine;

public class BlastProjectile : ProjectileBehavior
{
    public GameObject blastEffect;

    public virtual void Awake()
    {
    }

    public override void ResolveImpact(Collision collision)
    {
        if (impacted) return;
        impacted = true;
        body.isKinematic = true;
        transform.SetParent(collision.transform);
        body.useGravity = true;
        Instantiate(blastEffect, transform.position, Quaternion.identity);
        SessionDirector.Instance.PlayCue("cola-floor");
        Destroy(gameObject);
    }
}
