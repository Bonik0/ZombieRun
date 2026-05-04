using UnityEngine;

public class EnvironmentSensor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EffectZone")) HudRoot.Instance.pickupBanner.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        HudRoot.Instance.pickupBanner.SetActive(false);
    }
}
