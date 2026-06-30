using UnityEngine;

public class AudioTriggerController : MonoBehaviour
{
    public AudioSource targetAudioSource; // 拖入你要控制的那個 AudioSource
    public bool stopOnTouch = true;       // 勾選代表此物件是用來「停止」聲音的

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (stopOnTouch)
            {
                // 停止播放
                targetAudioSource.Stop(); 
                
                // 或者如果你想暫停（下次播放從這開始）：
                // targetAudioSource.Pause();
            }
            else
            {
                // 播放
                if (!targetAudioSource.isPlaying) targetAudioSource.Play();
            }
        }
    }
}