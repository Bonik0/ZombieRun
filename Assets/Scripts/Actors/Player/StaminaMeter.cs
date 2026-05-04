using UnityEngine;
using UnityEngine.UI;

public class StaminaMeter : MonoBehaviour
{
    public float Endurance
    {
        get => endurance;
        set { endurance = Mathf.Clamp01(value); }
    }
    private float endurance;
    public bool canSprint;
    private float recoveryRate = 0.25f;
    private LocomotionDriver locomotionDriver;
    private Slider gauge;
    private Image gaugeImage;

    private void Start()
    {
        Endurance = 1;
        locomotionDriver = GetComponent<LocomotionDriver>();
        gauge = HudRoot.Instance.enduranceGauge;
        gaugeImage = gauge.fillRect.GetComponent<Image>();
    }

    private void Update()
    {
        CheckEndurance();
        RegenerateEndurance();
        gauge.value = Endurance;
        gaugeImage.color = canSprint ? Color.white : Color.red;
    }

    private void CheckEndurance()
    {
        if (Endurance <= 0)
        {
            Endurance = 0;
            canSprint = false;
        }
        else if (!canSprint && Endurance == 1)
        {
            canSprint = true;
        }
    }

    private void RegenerateEndurance()
    {
        if (Endurance < 1 && !locomotionDriver.sprinting) Endurance += recoveryRate * Time.deltaTime;
    }
}
