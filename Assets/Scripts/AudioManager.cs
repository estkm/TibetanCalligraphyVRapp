using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioSource uiAudioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        uiAudioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayUISound(AudioClip clip)
    {
        if (clip != null)
        {
            uiAudioSource.PlayOneShot(clip);
        }
    }
}
