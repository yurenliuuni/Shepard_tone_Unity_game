using UnityEngine;

public class ShepardTrigger : MonoBehaviour
{
    public enum TriggerAction { Stop, Pause, FadeOut, Play, FadeIn }

    public TriggerAction action = TriggerAction.FadeOut;
    public float fadeDuration = 1.5f;

    [Header("Music Player UI")]
    public MusicPlayerUI musicPlayerUI; // 拖入 MusicPlayerUI 物件

    private bool triggered = false;

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        triggered = false;
        Debug.Log($"Player 離開範圍, trigger: {triggered}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;

        switch (action)
        {
            case TriggerAction.Stop:
                ShepardManger.Instance.StopMusic();
                musicPlayerUI?.StopAndReset(); // 停止並重設進度條
                break;
            case TriggerAction.Pause:
                ShepardManger.Instance.PauseMusic();
                break;
            case TriggerAction.FadeOut:
                ShepardManger.Instance.FadeOut(fadeDuration);
                musicPlayerUI?.StopAndReset(); // FadeOut 也重設
                break;
            case TriggerAction.Play:
                ShepardManger.Instance.PlayMusic();
                musicPlayerUI?.ResumePseudo(); // 繼續進度條
                break;
            case TriggerAction.FadeIn:
                ShepardManger.Instance.FadeIn(fadeDuration);
                musicPlayerUI?.ResumePseudo();
                break;
        }

        Debug.Log($"Player 進入範圍, trigger: {triggered}");
    }
}