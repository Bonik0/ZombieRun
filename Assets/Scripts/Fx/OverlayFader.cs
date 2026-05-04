using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OverlayFader : MonoBehaviour
{
    [SerializeField, Header("DO NOT FORGET TO ASSIGN TARGETS")] private Image[] targets;
    [SerializeField] private Color finalColor;
    [SerializeField] private bool useAlpha = true;
    [SerializeField] private bool fadeOnStart = true;
    [SerializeField] private bool destroyOnComplete;
    [SerializeField] private bool ignoreTimeScale;
    [SerializeField] private float duration = 1.5f;

    private void Start()
    {
        if (fadeOnStart) RunFade();
    }

    public void RunFade()
    {
        foreach (Image image in targets)
        {
            image.CrossFadeColor(finalColor, duration, ignoreTimeScale, false);
            if (useAlpha) image.CrossFadeAlpha(finalColor.a, duration, ignoreTimeScale);
            if (destroyOnComplete) StartCoroutine(CheckDestroy());
        }
    }

    private IEnumerator CheckDestroy()
    {
        while (true)
        {
            foreach (Image image in targets) if (image.color == finalColor) Destroy(image.gameObject, duration);
            yield return new WaitForEndOfFrame();
        }
    }
}
