using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicPlayerUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider timeSlider;
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI totalTimeText;
    public TextMeshProUGUI songNameText;

    [Header("Pseudo Settings")]
    public bool usePseudoMode = true;       // 開啟假進度條模式
    public float pseudoTotalTime = 180f;    // 自定義總時間（秒）
    public string pseudoSongName = "BGM";  // 自定義歌名
    public bool loop = true;               // 是否重複

    private AudioSource audioSource;
    private bool isDragging = false;

    // Pseudo 狀態
    private float pseudoCurrentTime = 0f;
    private bool pseudoIsRunning = false;

    private void Start()
    {
        audioSource = ShepardManger.Instance.GetAudioSource();

        if (usePseudoMode)
        {
            songNameText.text = pseudoSongName;
            totalTimeText.text = FormatTime(pseudoTotalTime);
            currentTimeText.text = FormatTime(0f);
            timeSlider.value = 0f;
            pseudoIsRunning = true;
        }
        else
        {
            if (audioSource.clip != null)
            {
                totalTimeText.text = FormatTime(audioSource.clip.length);
                songNameText.text = audioSource.clip.name;
            }
        }

        timeSlider.onValueChanged.AddListener(OnSliderDrag);
        StopAndReset();
    }

    private void Update()
    {
        if (isDragging) return;

        if (usePseudoMode)
        {
            UpdatePseudo();
        }
        else
        {
            UpdateReal();
        }
    }

    // ── Pseudo 模式 ──────────────────────────

    private void UpdatePseudo()
    {
        if (!pseudoIsRunning) return;

        pseudoCurrentTime += Time.deltaTime;

        // 重複
        if (pseudoCurrentTime >= pseudoTotalTime)
        {
            if (loop)
                pseudoCurrentTime = 0f;
            else
            {
                pseudoCurrentTime = pseudoTotalTime;
                pseudoIsRunning = false;
            }
        }

        timeSlider.value = pseudoCurrentTime / pseudoTotalTime;
        currentTimeText.text = FormatTime(pseudoCurrentTime);
    }

    // 從外部（ShepardTrigger）呼叫：停止並重設
    public void StopAndReset()
    {
        pseudoIsRunning = false;
        pseudoCurrentTime = 0f;
        timeSlider.value = 0f;
        currentTimeText.text = FormatTime(0f);
    }

    // 從外部呼叫：繼續播放
    public void ResumePseudo()
    {
        pseudoIsRunning = true;
    }

    // 從外部呼叫：更換歌曲資訊
    public void SetPseudoTrack(string songName, float totalTime)
    {
        pseudoSongName = songName;
        pseudoTotalTime = totalTime;
        pseudoCurrentTime = 0f;
        pseudoIsRunning = true;

        songNameText.text = pseudoSongName;
        totalTimeText.text = FormatTime(pseudoTotalTime);
        currentTimeText.text = FormatTime(0f);
        timeSlider.value = 0f;
    }

    // ── Real 模式 ────────────────────────────

    private void UpdateReal()
    {
        if (audioSource == null || audioSource.clip == null) return;

        float progress = audioSource.time / audioSource.clip.length;
        timeSlider.value = progress;
        currentTimeText.text = FormatTime(audioSource.time);
    }

    // ── Slider 拖動 ──────────────────────────

    private void OnSliderDrag(float value)
    {
        if (!isDragging) return;

        if (usePseudoMode)
            pseudoCurrentTime = value * pseudoTotalTime;
        else if (audioSource.clip != null)
            audioSource.time = value * audioSource.clip.length;
    }

    public void OnBeginDrag()
    {
        isDragging = true;
    }

    public void OnEndDrag()
    {
        isDragging = false;

        if (usePseudoMode)
            pseudoCurrentTime = timeSlider.value * pseudoTotalTime;
        else if (audioSource.clip != null)
            audioSource.time = timeSlider.value * audioSource.clip.length;
    }

    // ── 格式化時間 ────────────────────────────

    private string FormatTime(float seconds)
    {
        int m = Mathf.FloorToInt(seconds / 60);
        int s = Mathf.FloorToInt(seconds % 60);
        return $"{m}:{s:00}";
    }
}