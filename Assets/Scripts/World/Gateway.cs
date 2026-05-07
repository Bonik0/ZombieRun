using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Gateway : MonoBehaviour
{
    public enum GateState { Open, Closed }
    public GateState initialState;
    public GameObject openVisual;
    public GameObject closedVisual;
    public List<GameObject> spawnPosList = new List<GameObject>();

    private void Start()
    {
        switch (initialState)
        {
            case GateState.Open:
                openVisual.SetActive(true);
                closedVisual.SetActive(false);
                break;
            case GateState.Closed:
                closedVisual.SetActive(true);
                openVisual.SetActive(false);
                break;
        }
    }

    public void OpenGate()
    {
        openVisual.SetActive(true);
        closedVisual.SetActive(false);
        for (int i = 0; i < spawnPosList.Count; i++)
        {
            GameObject spawnPoint = spawnPosList[i];
            spawnPoint.SetActive(true);
        }
        SessionDirector.Instance.PlayCue("door-open");
        HudRoot.Instance.SpawnHint("Door Alert");
    }

    public void CloseGate()
    {
        SessionDirector.Instance.PlayCue("door-close");
        closedVisual.SetActive(true);
        openVisual.SetActive(false);
    }
}
