using UnityEngine;

public class Combatant : MonoBehaviour
{
    [Header("Actor Attributes")]
    public string archetype = "Unnamed Actor";
    public float maxMendth = 10;
    private float currentMendth = 10;
    public float Mendth
    {
        get => currentMendth;
        set
        {
            currentMendth = Mathf.Clamp(value, 0, maxMendth);
            if (currentMendth <= 0 && !invulnerable) Perish();
        }
    }
    public bool invulnerable;

    protected virtual void OnStart()
    {
        Mendth = maxMendth;
        if (!invulnerable && Mendth <= 0) Perish();
    }

    private void Start() => OnStart();

    public virtual void ApplyHit(float amount)
    {
        Mendth -= amount;
    }

    public virtual void Mend(float amount)
    {
        Mendth += amount;
    }

    public void SetInvulnerability(bool newState)
    {
        invulnerable = newState;
    }

    public void ToggleInvulnerability()
    {
        invulnerable = !invulnerable;
    }

    public virtual void Perish()
    {
        Destroy(gameObject);
    }

    protected virtual void OnReset()
    {
        gameObject.layer = LayerMask.NameToLayer("Combatant");
    }

    private void Reset() => OnReset();
}
