# Shepard Tone Unity Game

An interactive Unity game demonstrating the **Shepard Tone** — a psychological auditory illusion where a tone appears to infinitely ascend or descend in pitch without actually getting higher or lower.


## 📁 Project Structure

Shepard_tone_Unity_game/
├── mario_like_game/                    # Main Unity game project
│   ├── shepard_illusion_game/          # 🎮 Open in Unity Hub (import this folder)
│   └── unity-super-mario-tutorial-main/ # Base tutorial repository
│       └── Source: https://github.com/zigurous/unity-super-mario-tutorial
├── shepard/                            # Audio theory visualization scripts
│   └── Code for generating 4 auditory theory explanation figures
└── depth_illusion_pyvideo/             # Depth illusion animation (Manim)
    └── Output: gemini/media/videos/perspective/480p15/PerspectiveFirstPrinciples.mp4


## 🎮 Getting Started

### quick demo exe file
https://drive.google.com/drive/folders/1rqts8llK2B0XhYLbt5Fr4uB7OrUT6MbY?usp=drive_link

### Prerequisites
- [Unity Hub](https://unity.com/download) (2022.3 LTS or later recommended)
- Unity Editor

### Open the Project
1. Launch **Unity Hub**
2. Click **Open** → **Add project from disk**
3. Select the `mario_like_game/shepard_illusion_game/` folder
4. Open the project and press **Play** in the Editor

---

## 🔊 What is the Shepard Tone?

The **Shepard Tone** is an auditory illusion created by superimposing multiple sine waves separated by octaves. As the pitch of each tone rises, it fades out at the top of its range while a new tone fades in at the bottom, creating the perception of an infinitely ascending (or descending) scale.

This game integrates the illusion into a platformer environment to demonstrate the effect in an interactive context.

---

## 📂 Subprojects

| Folder | Description |
|--------|-------------|
| `shepard_illusion_game/` | Main Unity project — import this into Unity Hub |
| `shepard/` | Python scripts for generating 4 auditory theory explanation figures |
| `depth_illusion_pyvideo/` | Manim code for **Ames Window** depth projection theory explanation |

---

## 🎬 Video Output

The `depth_illusion_pyvideo` module generates: depth_illusion_pyvideo/gemini/media/videos/perspective/480p15/PerspectiveFirstPrinciples.mp4

This video explains the **Ames Window** — a classic depth perception illusion demonstrating how our brain interprets 2D projections as 3D objects.

---

## 🙏 Credits

- Base Mario tutorial: [zigurous/unity-super-mario-tutorial](https://github.com/zigurous/unity-super-mario-tutorial)
- Audio theory figures: Custom Python scripts
- Depth illusion animation: [Manim](https://www.manim.community/) (Python mathematical animation engine)

---

