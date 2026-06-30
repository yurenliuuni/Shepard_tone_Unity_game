from manim import *
import numpy as np

class ShepardToneMechanism(Scene):
    def construct(self):
        # ==========================================
        # 1. 建立座標軸與標籤
        # X軸：以中心頻率為基準的對數頻率 (Octaves)
        # Y軸：相對振幅 (0 到 1)
        # ==========================================
        ax = Axes(
            x_range=[-2.5, 2.5, 1],  # 涵蓋中心前後各兩個八度
            y_range=[0, 1.2, 0.5],
            axis_config={"include_tip": False},
            x_length=10,
            y_length=5
        ).shift(DOWN * 0.5)

        x_label = ax.get_x_axis_label("Log_2(f / f_c)")
        y_label = ax.get_y_axis_label("Amplitude")
        self.add(ax, x_label, y_label)

        # ==========================================
        # 2. 繪製固定不變的高斯包絡線 (Gaussian Envelope)
        # 數學模型： A(x) = exp(-x^2 / (2 * sigma^2))
        # ==========================================
        sigma = 0.8
        def envelope_func(x):
            return np.exp(-(x**2) / (2 * sigma**2))

        envelope_curve = ax.plot(envelope_func, color=YELLOW, use_smoothing=True)
        
        # 標示數學公式
        math_label = MathTex(
            r"A(f) = \exp\left(-\frac{(\log_2 f - \log_2 f_c)^2}{2\sigma^2}\right)"
        ).next_to(envelope_curve, UP, buff=0.5).scale(0.8)

        # self.play(Create(ax), Write(math_label))
        self.play(Create(envelope_curve))

        # ==========================================
        # 3. 建立三個相距八度的動態音調 (Dots)
        # ==========================================
        # tracker 用於控制基礎頻率的平移量 (以八度為單位)
        shift_tracker = ValueTracker(0)

        # 視覺化三個音調：-1 Octave (藍), 0 Octave (綠), +1 Octave (紅)
        # 定義邊界以產生 Wrap-around 效應，模擬超出人類聽覺閾值後的重置
        left_bound = -1.5
        right_bound = 1.5
        domain_width = right_bound - left_bound

        def get_dynamic_dot(octave_offset, color, label_text):
            dot = Dot(color=color, radius=0.1)
            label = Tex(label_text, color=color).scale(0.6)
            
            # 使用 updater 讓點與標籤跟隨 tracker 的數值動態更新
            def updater(mobj):
                # 計算當前對數頻率位置，並實作模運算 (Modulo) 以達成循環
                current_x = shift_tracker.get_value() + octave_offset
                wrapped_x = ((current_x - left_bound) % domain_width) + left_bound
                current_y = envelope_func(wrapped_x)
                
                # 更新位置
                mobj[0].move_to(ax.c2p(wrapped_x, current_y))
                mobj[1].next_to(mobj[0], UP, buff=0.1)
                
                # 根據振幅調整不透明度，強化漸強/漸弱的視覺感知
                opacity = current_y
                mobj[0].set_fill(opacity=opacity)
                mobj[1].set_fill(opacity=opacity)

            group = VGroup(dot, label)
            group.add_updater(updater)
            return group

        tone_minus1 = get_dynamic_dot(-1, BLUE, "-1 Oct.")
        tone_0 = get_dynamic_dot(0, GREEN, "0 Oct.")
        tone_plus1 = get_dynamic_dot(1, RED, "+1 Oct.")

        self.add(tone_minus1, tone_0, tone_plus1)

        # ==========================================
        # 4. 動畫執行：頻率連續上升的錯覺
        # ==========================================
        # 平移 1 個完整的八度。
        # 由於 Wrap-around 機制，當 +1 Octave 衰減至零並跨越右邊界時，
        # 會無縫從左邊界作為 -1 Octave 重新出現並開始漸強。
        self.play(
            shift_tracker.animate.set_value(1),
            run_time=6,
            rate_func=linear # 使用線性速率以精確對應等比頻率上升
        )
        
        # 再次平移一個八度以展示循環性
        shift_tracker.set_value(0)
        self.play(
            shift_tracker.animate.set_value(1),
            run_time=6,
            rate_func=linear
        )

        self.wait(1)