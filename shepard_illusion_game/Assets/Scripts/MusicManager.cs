using UnityEngine;

// Attach this to a persistent GameObject (e.g. GameManager)
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void StopMusic() => audioSource.Stop();
    // public void PlayMusic() => audioSource.Play();
    public void PauseMusic() => audioSource.Pause();
    // public void FadeOut(float duration) => StartCoroutine(FadeCoroutine(0f, duration));
    // public void FadeIn(float duration) => StartCoroutine(FadeCoroutine(1f, duration));

    public void PlayMusic()
    {
        StopAllCoroutines();  // 停掉任何進行中的 fade
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void FadeOut(float duration)
    {
        StopAllCoroutines();  // 同上
        StartCoroutine(FadeCoroutine(0f, duration));
    }

    public void FadeIn(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeCoroutine(1f, duration));
    }
    private System.Collections.IEnumerator FadeCoroutine(float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
        if (targetVolume == 0f) audioSource.Stop();
    }
}