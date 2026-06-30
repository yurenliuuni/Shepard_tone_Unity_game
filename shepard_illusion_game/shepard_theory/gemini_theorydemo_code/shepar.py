import numpy as np
import matplotlib.pyplot as plt
import librosa
import librosa.display

def visualize_shepard_effect(mp3_file_path):
    # 建立 2x2 的圖表架構
    fig, axs = plt.subplots(2, 2, figsize=(15, 10))
    plt.subplots_adjust(hspace=0.4, wspace=0.2)

    # ==========================================
    # 1. MP3 檔案的梅爾頻譜圖 (Mel Spectrogram)
    # ==========================================
    ax1 = axs[0, 0]
    try:
        # 載入音訊檔案
        y, sr = librosa.load(mp3_file_path, sr=None)
        # 計算 Mel Spectrogram
        S = librosa.feature.melspectrogram(y=y, sr=sr, n_mels=128, fmax=8000)
        S_dB = librosa.power_to_db(S, ref=np.max)
        
        img = librosa.display.specshow(S_dB, x_axis='time', y_axis='mel', sr=sr, fmax=8000, ax=ax1)
        fig.colorbar(img, ax=ax1, format='%+2.0f dB')
        ax1.set_title('1. Mel Spectrogram of Shepard Tone (Empirical)')
    except FileNotFoundError:
        ax1.text(0.5, 0.5, 'MP3 file not found.\nPlease update mp3_file_path.', 
                 horizontalalignment='center', verticalalignment='center')
        ax1.set_title('1. Mel Spectrogram (File Missing)')

    # ==========================================
    # 2. 理論波形展示 (3 Tones: 0, +1, -1 Octave)
    # ==========================================
    ax2 = axs[0, 1]
    t = np.linspace(0, 0.02, 1000) # 擷取 20ms 的時間窗口
    f0 = 220.0 # 基準頻率 220 Hz (A3)
    
    # 三個八度音程的波形
    wave_minus1 = 0.5 * np.sin(2 * np.pi * (f0 / 2) * t)
    wave_0 = 1.0 * np.sin(2 * np.pi * f0 * t)
    wave_plus1 = 0.5 * np.sin(2 * np.pi * (f0 * 2) * t)
    wave_composite = wave_minus1 + wave_0 + wave_plus1

    ax2.plot(t, wave_composite, label='Composite Waveform', color='black', linewidth=2)
    ax2.plot(t, wave_0, label='0 Octave (220Hz)', linestyle='--')
    ax2.plot(t, wave_plus1, label='+1 Octave (440Hz)', linestyle=':')
    ax2.plot(t, wave_minus1, label='-1 Octave (110Hz)', linestyle='-.')
    
    ax2.set_title('2. Theoretical Waveform Superposition')
    ax2.set_xlabel('Time (s)')
    ax2.set_ylabel('Amplitude')
    ax2.legend(loc='upper right')

    # ==========================================
    # 3. 離散階梯狀頻率上升 (Time Domain Stair-like Pitch)
    # ==========================================
    ax3 = axs[1, 0]
    steps = 12 # 一個八度內的 12 個半音
    time_steps = np.arange(steps)
    
    # 離散等距音高計算 (半音階)
    freqs_0 = f0 * (2 ** (time_steps / 12))
    freqs_minus1 = freqs_0 / 2
    freqs_plus1 = freqs_0 * 2

    ax3.step(time_steps, freqs_plus1, label='+1 Octave', where='post', color='red')
    ax3.step(time_steps, freqs_0, label='0 Octave', where='post', color='green')
    ax3.step(time_steps, freqs_minus1, label='-1 Octave', where='post', color='blue')
    
    ax3.set_yscale('log') # 使用對數尺度以呈現線性聽覺感知
    ax3.set_title('3. Discrete Stair-like Increasing Pitch')
    ax3.set_xlabel('Discrete Time Steps (Semitones)')
    ax3.set_ylabel('Frequency (Hz) [Log Scale]')
    ax3.set_xticks(time_steps)
    ax3.grid(True, which="both", ls="--", alpha=0.5)
    ax3.legend()

    # ==========================================
    # 4. 振幅包絡線與漸強/漸弱效應 (Crescendo / Decrescendo)
    # ==========================================
    ax4 = axs[1, 1]
    
    # 定義高斯鐘形包絡線函數
    def envelope(f, f_center=220, sigma=1.0):
        return np.exp(-((np.log2(f) - np.log2(f_center))**2) / (2 * sigma**2))

    amp_minus1 = envelope(freqs_minus1)
    amp_0 = envelope(freqs_0)
    amp_plus1 = envelope(freqs_plus1)

    ax4.plot(time_steps, amp_plus1, 'o-', color='red', label='+1 Octave (Decrescendo)')
    ax4.plot(time_steps, amp_0, 's-', color='green', label='0 Octave (Peak/Stable)')
    ax4.plot(time_steps, amp_minus1, '^-', color='blue', label='-1 Octave (Crescendo)')
    
    ax4.set_title('4. Amplitude Envelopes (Volume Contour)')
    ax4.set_xlabel('Discrete Time Steps (Semitones)')
    ax4.set_ylabel('Relative Amplitude')
    ax4.set_xticks(time_steps)
    ax4.grid(True, ls="--", alpha=0.5)
    ax4.legend()

    plt.show()

# 執行函式 (請替換為您實際的檔案路徑)
visualize_shepard_effect('/Users/a0/Downloads/shepard/SuperMario_Looping Steps.mp3')