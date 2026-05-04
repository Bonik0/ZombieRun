using UnityEngine;
using UnityEngine.InputSystem;

public class LoadoutSwitcher : MonoBehaviour
{
    [SerializeField] internal ThrowableLauncher[] launchers;
    public bool[] slotUnlocked;
    private int previousSlotIndex;
    private InputAction previousSlotAction;
    private InputAction[] slotActions;

    private void Start()
    {
        previousSlotAction = InputSystem.actions.FindAction("Previous Weapon");
        slotActions = new InputAction[launchers.Length];
        for (int i = 0; i < launchers.Length; i++) slotActions[i] = InputSystem.actions.FindAction($"weapon{i + 1}");
        SelectInitialSlot();
    }

    private void Update()
    {
        if (previousSlotAction.WasPressedThisFrame() && SessionDirector.Instance.Config.canQuickSwap) SelectSlot(previousSlotIndex);
        CheckSlotInput();
    }

    private void SelectInitialSlot()
    {
        launchers[0].gameObject.SetActive(true);
        for (int i = 1; i < launchers.Length; i++) launchers[i].gameObject.SetActive(false);
    }

    private void CheckSlotInput()
    {
        for (int i = 0; i < slotActions.Length; i++)
            if (slotActions[i].WasCompletedThisFrame() && slotUnlocked[i]) SelectSlot(i);
    }

    private void SelectSlot(int slotIndex)
    {
        for (int i = 0; i < launchers.Length; i++)
        {
            if (launchers[i].gameObject.activeInHierarchy && previousSlotIndex != i) previousSlotIndex = i;
            launchers[i].gameObject.SetActive(i == slotIndex);
        }
    }
}
