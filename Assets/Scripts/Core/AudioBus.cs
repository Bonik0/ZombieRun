using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioBus : MonoBehaviour
{
    private AudioSource emitter;

    private void Awake()
    {
        emitter = GetComponent<AudioSource>();
    }

    public void PlayCue(string fileName)
    {
        emitter.PlayOneShot(Resources.Load<AudioClip>("Sounds/" + fileName));
    }

    public void PlayCue(AudioClip clip)
    {
        emitter.PlayOneShot(clip);
    }
}
