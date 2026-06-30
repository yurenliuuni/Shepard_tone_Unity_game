using UnityEngine;

// Attach this to the trigger object (e.g. your Pipe or any collectible)
public class MusicTrigger : MonoBehaviour
{
    public enum TriggerAction { Stop, Pause, FadeOut, Play, FadeIn}

    public TriggerAction action = TriggerAction.FadeOut;
    public float fadeDuration = 1.5f;

    private bool triggered = false;

    void OnTriggerExit2D(Collider2D other) {
       if (!other.CompareTag("Player")) return;
        triggered = false;  // ← 離開就重置
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player")) return;

        triggered = true;

        switch (action)
        {
            case TriggerAction.Stop:
                MusicManager.Instance.StopMusic();
                break;
            case TriggerAction.Pause:
                MusicManager.Instance.PauseMusic();
                break;
            case TriggerAction.FadeOut:
                MusicManager.Instance.FadeOut(fadeDuration);
                break;
            case TriggerAction.Play:
                MusicManager.Instance.PlayMusic();
                break;
            case TriggerAction.FadeIn:
                ShepardManger.Instance.FadeIn(fadeDuration);
                break;
        }
    }
    

}