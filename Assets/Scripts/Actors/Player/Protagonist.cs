using System.Collections;
using UnityEngine;

public class Protagonist : Combatant
{
    private float recoveryTimer;
    private SessionConfig config;

    protected override void OnStart()
    {
        base.OnStart();
        config = SessionDirector.Instance.Config;
    }

    public override void Perish()
    {
        if (invulnerable) print("Player died");
        else SessionDirector.Instance.ShowFailureScene();
    }

    private void Update()
    {
        recoveryTimer = Mathf.Clamp(recoveryTimer + Time.deltaTime, 0, config.recoveryDelay);
        if (recoveryTimer >= config.recoveryDelay) Mend(config.recoveryRate * Time.deltaTime);
    }

    protected override void OnReset()
    {
        gameObject.tag = "Player";
        archetype = "Player";
    }

    public override void ApplyHit(float amount)
    {
        base.ApplyHit(amount);
        recoveryTimer = 0;
        StartCoroutine(RunHitFlash());
    }

    public IEnumerator RunHitFlash()
    {
        HudRoot.Instance.hitVignette.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        HudRoot.Instance.hitVignette.SetActive(false);
    }
}
