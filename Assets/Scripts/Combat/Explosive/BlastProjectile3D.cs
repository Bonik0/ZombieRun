using UnityEngine;

public class BlastProjectile3D : BlastProjectile
{
    public GameObject secondBlastEffect;

    public override void Awake()
    {
        SessionDirector.Instance.PlayCue("glasses-floor");
    }

    public override void ResolveImpact(Collision collision)
    {
        if (impacted) return;
        impacted = true;
        body.isKinematic = true;
        transform.SetParent(collision.transform);
        body.useGravity = true;
        int separation = 1;
        Instantiate(blastEffect, transform.position + (-transform.right * separation), transform.rotation);
        Instantiate(secondBlastEffect, transform.position + (transform.right * separation), transform.rotation);
        Destroy(gameObject);
    }
}
