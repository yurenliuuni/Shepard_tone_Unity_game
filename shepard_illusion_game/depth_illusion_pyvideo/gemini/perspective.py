from manim import *

class PerspectiveFirstPrinciples(ThreeDScene):
    def construct(self):
        # 設置 3D 視角，讓我們可以從斜上方觀察整個透視系統
        self.set_camera_orientation(phi=75 * DEGREES, theta=30 * DEGREES)

        # --------------------------------------------------
        # 基礎環境建置
        # --------------------------------------------------
        # 坐標軸
        axes = ThreeDAxes(x_range=[-5, 5], y_range=[-5, 5], z_range=[-1, 10])
        
        # 眼睛 (原點)
        eye = Dot3D(ORIGIN, color=YELLOW, radius=0.1)
        
        # 畫布 z=1 (使用一個半透明的平面代表)
        canvas = Rectangle(width=8, height=8, color=BLUE_B).set_fill(BLUE_E, opacity=0.15)
        canvas.move_to(OUT * 1) # OUT 代表 Z 軸正方向

        self.play(Create(axes), Create(eye), FadeIn(canvas))
        self.wait()

        # --------------------------------------------------
        # 第一部分：近大遠小與動態立方體投影
        # P(x, y, z) -> (x/z, y/z, 1)
        # --------------------------------------------------
        
        # 定義立方體的 8 個頂點與 12 條邊
        base_corners = [np.array([x, y, z]) for x in [-1, 1] for y in [-1, 1] for z in [-1, 1]]
        edges = [(0,1), (0,2), (1,3), (2,3), (4,5), (4,6), (5,7), (6,7), (0,4), (1,5), (2,6), (3,7)]

        # 創建 3D 空間中的立方體頂點，初始位置在 z=4
        cube_dots = VGroup(*[Dot3D(c + OUT * 4, color=BLUE, radius=0.08) for c in base_corners])
        
        # 創建立方體的邊 (會自動跟隨頂點更新)
        cube_lines = VGroup(*[Line(color=BLUE) for _ in edges])
        for idx, (i, j) in enumerate(edges):
            cube_lines[idx].add_updater(
                lambda m, i=i, j=j: m.put_start_and_end_on(cube_dots[i].get_center(), cube_dots[j].get_center())
            )

        # 創建畫布上 z=1 的投影點
        proj_dots = VGroup(*[Dot3D(color=RED, radius=0.06) for _ in range(8)])
        for i in range(8):
            proj_dots[i].add_updater(
                lambda m, i=i: m.move_to(cube_dots[i].get_center() / cube_dots[i].get_center()[2])
            )

        # 創建畫布上的投影線
        proj_lines = VGroup(*[Line(color=RED, stroke_width=4) for _ in edges])
        for idx, (i, j) in enumerate(edges):
            proj_lines[idx].add_updater(
                lambda m, i=i, j=j: m.put_start_and_end_on(proj_dots[i].get_center(), proj_dots[j].get_center())
            )

        # 創建從原點(眼睛)發出，經過頂點的射線
        rays = VGroup(*[DashedLine(color=GRAY, stroke_opacity=0.4) for _ in range(8)])
        for i in range(8):
            rays[i].add_updater(
                lambda m, i=i: m.put_start_and_end_on(ORIGIN, cube_dots[i].get_center())
            )

        self.play(Create(cube_dots), Create(cube_lines))
        self.play(Create(rays))
        self.play(Create(proj_dots), Create(proj_lines))
        self.wait()

        # 動畫：立方體旋轉並向遠處移動 (展示近大遠小)
        self.play(
            cube_dots.animate.rotate(PI/3, axis=UP).move_to(OUT * 7),
            run_time=3, rate_func=there_and_back
        )
        
        # 動畫：立方體任意旋轉與縮放
        self.play(
            cube_dots.animate.rotate(PI/2, axis=RIGHT+UP).scale(1.5).move_to(OUT * 3.5),
            run_time=4
        )
        self.wait()

        # 清理場景，準備第二部分
        self.play(
            FadeOut(cube_dots), FadeOut(cube_lines), 
            FadeOut(proj_dots), FadeOut(proj_lines), FadeOut(rays)
        )

        # --------------------------------------------------
        # 第二部分：投影的不可逆性 (多個三維四邊形投影為同一個正方形)
        # (u,v) -> (u*t, v*t, t)
        # --------------------------------------------------
        
        # 畫布上固定的一個正方形 (z=1)
        square_corners = [np.array([-1, 1, 1]), np.array([1, 1, 1]), np.array([1, -1, 1]), np.array([-1, -1, 1])]
        square_edges = [(0,1), (1,2), (2,3), (3,0)]
        
        fixed_proj_lines = VGroup(*[
            Line(square_corners[i], square_corners[j], color=RED, stroke_width=5) 
            for i, j in square_edges
        ])
        self.play(Create(fixed_proj_lines))

        # 反向射出 4 條無限延伸(此處設長度為 10)的射線
        inf_rays = VGroup(*[Line(ORIGIN, c * 10, color=GRAY, stroke_opacity=0.5) for c in square_corners])
        self.play(Create(inf_rays))

        # 4 個獨立的 t 參數，控制四個頂點在射線上的深度
        trackers = [ValueTracker(4.0) for _ in range(4)]

        # 4 個在空間中隨意跑動的點
        moving_dots = VGroup(*[Dot3D(color=GREEN, radius=0.08) for _ in range(4)])
        for i in range(4):
            moving_dots[i].add_updater(
                lambda m, i=i: m.move_to(square_corners[i] * trackers[i].get_value())
            )

        # 連接這 4 個點形成不規則四邊形
        moving_lines = VGroup(*[Line(color=GREEN, stroke_width=6) for _ in square_edges])
        for idx, (i, j) in enumerate(square_edges):
            moving_lines[idx].add_updater(
                lambda m, i=i, j=j: m.put_start_and_end_on(moving_dots[i].get_center(), moving_dots[j].get_center())
            )

        self.play(Create(moving_dots), Create(moving_lines))
        self.wait()

        # 動畫：四個點在射線上各自獨立滑動，形成扭曲的三維四邊形
        # 但你可以看到紅色的投影正方形完全沒有改變
        self.play(
            trackers[0].animate.set_value(2),
            trackers[1].animate.set_value(8),
            trackers[2].animate.set_value(3),
            trackers[3].animate.set_value(7),
            run_time=3
        )
        self.play(
            trackers[0].animate.set_value(7),
            trackers[1].animate.set_value(3),
            trackers[2].animate.set_value(9),
            trackers[3].animate.set_value(2),
            run_time=3
        )
        
        # 改變相機視角，更清楚地看這個奇怪的形狀
        self.move_camera(phi=60 * DEGREES, theta=60 * DEGREES, run_time=2)
        self.wait(2)