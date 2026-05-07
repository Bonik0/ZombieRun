using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WaveOrchestrator : MonoBehaviour
{
    [Header("SessionConfig")]
    [SerializeField] private float waveDuration;
    private float timer;
    private bool timerActive;
    public int currentWaveIndex;
    [SerializeField] private float waveTransitionTime;

    [Header("Waves")]
    [SerializeField] private Wave[] waves;
    [SerializeField] private List<GameObject> spawnPosList = new();
    private List<float> localCounters = new();

    private void Start()
    {
        BeginNextWave();
        HudRoot.Instance.SetWaveLabel(currentWaveIndex);
    }

    private void Update()
    {
        if (!timerActive) return;
        if (timer < waveDuration)
        {
            ProcessCurrentWave();
            string timerString = ((int)(waveDuration - timer)).ToString();
        }
        else
        {
            BeginWaveTransition();
        }
    }

    private void BeginWave(int waveIndex)
    {
        SessionDirector.Instance.Config.clearedWave = waveIndex + 1;
        localCounters.Clear();
        foreach (WaveSegment segment in waves[waveIndex].segments) localCounters.Add(1);
        timer = 0;
        timerActive = true;
        Debug.Log("Starting wave " + currentWaveIndex);
    }

    private void BeginNextWave()
    {
        BeginWave(currentWaveIndex);
    }

    private void BeginWaveTransition()
    {
        timerActive = false;
        waves[currentWaveIndex].onComplete.Invoke();
        currentWaveIndex++;
        if (currentWaveIndex >= waves.Length) return;
        StartCoroutine(DelayNextWave());
    }

    public IEnumerator DelayNextWave()
    {
        HudRoot.Instance.SetWaveLabel(currentWaveIndex);
        yield return new WaitForSeconds(waveTransitionTime);
        BeginNextWave();
    }

    private void ProcessCurrentWave()
    {
        Wave currentWave = waves[currentWaveIndex];
        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment segment = currentWave.segments[i];
            float startTime = segment.timeWindow.x / 100 * waveDuration;
            float endTime = segment.timeWindow.y / 100 * waveDuration;
            if (timer < startTime || timer > endTime) continue;
            float timeSinceSegmentStart = timer - startTime;
            if (timeSinceSegmentStart / (1f / segment.spawnFrequency) > localCounters[i])
            {
                if (SessionDirector.Instance.ReserveStalkerSlot()) Instantiate(segment.prefab, PickSpawnPoint().position, Quaternion.identity, transform);
                localCounters[i]++;
            }
        }
        timer += Time.deltaTime;
    }

    private Transform PickSpawnPoint()
    {
        GameObject spawnPoint = spawnPosList[Random.Range(0, spawnPosList.Count)];
        if (!spawnPoint.activeSelf)
        {
            while (!spawnPoint.activeSelf) spawnPoint = spawnPosList[Random.Range(0, spawnPosList.Count)];
        }

        return spawnPoint.transform;
    }
}
