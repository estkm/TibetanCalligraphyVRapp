using UnityEngine;

public class PlayAudioOnEnable : MonoBehaviour
{
    [SerializeField] private AudioClip clip;              
    [SerializeField] private AudioSource audioManager; 

    private void OnEnable()
    {
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager no asignado en " + gameObject.name);
            return;
        }

        if (clip == null)
        {
            Debug.LogWarning("AudioClip no asignado en " + gameObject.name);
            return;
        }

        audioManager.clip = clip;
        audioManager.Play();
    }
}
