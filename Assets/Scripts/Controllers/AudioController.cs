using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Audio Clips")]
    public AudioClip successClip;
    public AudioClip failClip;

    private void Awake()
    {
        //Patron singleton basico
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySuccess()
    {
        sfxSource.PlayOneShot(successClip);
    }

    public void PlayFail()
    {
        sfxSource.PlayOneShot(failClip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}
