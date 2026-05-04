using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HintBubble : MonoBehaviour
{
    [Header("Behaviour")]
    public bool destroyAfterTime;
    public float destroyAfterSeconds = 5;
    public bool destroyAfterAction;
    [SerializeField] private InputActionReference action;

    [Header("Visuals")]
    public bool cycleInputs = true;
    public float timePerInputCycleSeconds = 0.8f;
    [SerializeField] private Sprite[] inputSprites;
    [SerializeField] private Image inputReadout;

    private void Start()
    {
        if (cycleInputs) StartCoroutine(CycleInputs(timePerInputCycleSeconds));
        if (destroyAfterTime) Destroy(gameObject, destroyAfterSeconds);
    }

    private void Update()
    {
        if (destroyAfterAction && action.action.WasPressedThisFrame()) Destroy(gameObject);
    }

    private IEnumerator CycleInputs(float duration)
    {
        int index = 0;
        while (true)
        {
            inputReadout.sprite = inputSprites[index];
            index++;
            if (index >= inputSprites.Length) index = 0;

            yield return new WaitForSeconds(duration);
        }
    }
}
