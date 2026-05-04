using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
    public UnityEvent onComplete;
}

[System.Serializable]
public struct WaveSegment
{
    [MinMaxSlider(0, 100)] public Vector2 timeWindow;
    public float spawnFrequency;
    public GameObject prefab;
}
