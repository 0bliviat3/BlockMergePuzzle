# Project Structure

## 📁 Directory Overview

```
BlockMergePuzzle/
├── Assets/
│   ├── Scenes/                    # Unity scenes
│   │   ├── MainMenu.unity         # Main menu launcher
│   │   ├── SampleScene.unity      # Block Merge Puzzle (main game)
│   │   └── Classic2048.unity      # Classic 2048 game
│   │
│   ├── Scripts/                   # C# game scripts
│   │   ├── Shared/                # Shared systems
│   │   │   ├── AudioManager.cs
│   │   │   └── SceneLoader.cs
│   │   │
│   │   ├── MainMenu/              # Main menu
│   │   │   └── MainMenuManager.cs
│   │   │
│   │   ├── Classic2048/           # Classic 2048 game
│   │   │   ├── Classic2048Manager.cs
│   │   │   ├── Classic2048Grid.cs
│   │   │   ├── Classic2048Tile.cs
│   │   │   └── Classic2048Input.cs
│   │   │
│   │   └── (Block Merge Puzzle)  # Main game scripts
│   │       ├── GameManager.cs
│   │       ├── Grid.cs
│   │       ├── Block.cs
│   │       ├── BlockMerger.cs
│   │       ├── ScoreManager.cs
│   │       ├── EffectManager.cs
│   │       └── InputHandler.cs
│   │
│   ├── Prefabs/                   # Reusable game objects
│   │   ├── Block.prefab
│   │   ├── Tile.prefab
│   │   └── Particle Effects/
│   │
│   ├── Audio/                     # Sound files
│   │   ├── BGM/
│   │   └── SFX/
│   │
│   ├── Materials/                 # Unity materials
│   └── Resources/                 # Runtime-loaded assets
│
├── ProjectSettings/               # Unity project settings
├── Packages/                      # Package Manager dependencies
│
├── README.md                      # Project documentation
├── LICENSE                        # MIT License
├── STRUCTURE.md                   # This file
└── .gitignore                     # Git ignore rules
```

## 🎯 Scene Hierarchy

### MainMenu Scene
```
MainMenu
├── SceneLoader (DontDestroyOnLoad)
│   └── FadeCanvas
│       └── FadePanel (CanvasGroup)
│
├── AudioManager (DontDestroyOnLoad)
│   ├── BGM (AudioSource)
│   └── SFX (AudioSource)
│
└── MainMenuManager
    └── MenuCanvas (Auto-generated)
        ├── Background
        ├── Title Text
        └── Game Buttons
            ├── Block Merge Button
            └── Classic 2048 Button
```

### BlockMergePuzzle Scene (SampleScene)
```
BlockMergePuzzle
├── GameManager
│   ├── Grid
│   ├── BlockMerger
│   ├── ScoreManager
│   ├── EffectManager
│   └── InputHandler
│
├── Canvas (Scene UI)
│   ├── Score Display
│   ├── High Score Display
│   └── Game Over Panel
│
├── MainMenuButton (Auto-generated)
│
└── Particle Systems
    ├── Merge Effect
    ├── Explosion Effect
    └── Chain Effect
```

### Classic2048 Scene
```
Classic2048
├── Classic2048Manager
│   ├── Grid Component
│   └── Input Handler Component
│
├── GameCanvas (Auto-generated)
│   ├── Title
│   ├── Score Box
│   │   ├── Label: "SCORE"
│   │   └── Score Text
│   ├── Best Box
│   │   ├── Label: "BEST"
│   │   └── Best Score Text
│   ├── Grid Container
│   │   ├── Grid Background
│   │   ├── Cell Backgrounds (4x4)
│   │   └── Tiles (Dynamic)
│   ├── Game Over Panel
│   └── Main Menu Button
│
└── EventSystem (Auto-generated)
```

## 🔧 Script Dependencies

### Shared Systems
```
AudioManager
└── (No dependencies)

SceneLoader
├── AudioManager
└── UnityEngine.SceneManagement
```

### Block Merge Puzzle
```
GameManager (Main Controller)
├── Grid
├── BlockMerger
├── ScoreManager
├── EffectManager
├── InputHandler
└── AudioManager

Grid
└── Block

BlockMerger
├── Grid
└── ScoreManager

ScoreManager
└── Grid

EffectManager
└── Grid

InputHandler
└── GameManager
```

### Classic 2048
```
Classic2048Manager
├── Classic2048Grid
├── Classic2048Input
├── AudioManager
└── SceneLoader

Classic2048Grid
├── Classic2048Tile
└── Classic2048Manager

Classic2048Tile
└── LeanTween

Classic2048Input
└── Classic2048Manager
```

## 📦 Package Dependencies

### Unity Packages
- **Unity UI** (com.unity.ugui)
- **TextMeshPro** (com.unity.textmeshpro) - Optional
- **LeanTween** (Asset Store or manual import)

### External Assets
- **LeanTween** - Animation library
  - Location: `Assets/LeanTween/`
  - Usage: Smooth animations for tiles and UI

## 🎨 Asset Organization

### Sprites & Textures
```
Assets/Sprites/
├── UI/
│   ├── Buttons/
│   └── Icons/
└── Effects/
    └── Particles/
```

### Audio Files
```
Assets/Audio/
├── BGM/
│   ├── menu_bgm.mp3
│   ├── game_bgm.mp3
│   └── classic2048_bgm.mp3
└── SFX/
    ├── click.wav
    ├── merge.wav
    ├── explosion.wav
    └── game_over.wav
```

## 🏗️ Build Output

### Android Build
```
Builds/Android/
├── 2048Collection.apk      # Debug build
└── 2048Collection.aab      # Release build (Google Play)
```

### iOS Build
```
Builds/iOS/
└── 2048Collection.app      # Xcode project
```

## 📊 File Statistics

### Code Files
- **Total Scripts:** ~20 C# files
- **Total Lines:** ~5,000+ lines of code
- **Average File Size:** ~250 lines

### Assets
- **Scenes:** 3 Unity scenes
- **Prefabs:** ~5 prefabs
- **Audio:** 8-10 audio files
- **Total Size:** ~50-100 MB

## 🔄 Data Flow

### Scene Transition Flow
```
MainMenu
   ↓ (User Selection)
   ↓
SceneLoader.LoadScene("GameScene")
   ↓
FadeOut → UnloadOldScene → LoadNewScene → FadeIn
   ↓
New Scene Loaded
```

### Game Loop (Block Merge Puzzle)
```
User Input (Touch)
   ↓
InputHandler.OnTouchUp()
   ↓
GameManager.PlaceBlock(position)
   ↓
Grid.AddBlock(block, position)
   ↓
BlockMerger.CheckMerges()
   ↓
EffectManager.PlayMergeEffect()
   ↓
ScoreManager.AddScore(points)
   ↓
Check Game Over
```

### Game Loop (Classic 2048)
```
User Input (Swipe)
   ↓
Classic2048Input.DetectSwipe()
   ↓
Classic2048Manager.Move(direction)
   ↓
Classic2048Grid.MoveTiles(direction)
   ├─→ Process Row/Column
   ├─→ Merge Matching Tiles
   └─→ Update Grid State
   ↓
AddRandomTile()
   ↓
Check Game Over
```

## 🔐 Persistent Data

### PlayerPrefs Keys
```
Classic2048:
- "Classic2048_BestScore" (int)

Block Merge Puzzle:
- "HighScore" (int)
- "TotalGamesPlayed" (int)

Audio:
- "BGM_Volume" (float)
- "SFX_Volume" (float)
- "Audio_Muted" (bool)
```

### Save File Locations
- **Android:** `/data/data/com.company.2048collection/files/`
- **iOS:** `Application.persistentDataPath`
- **Editor:** `PlayerPrefs` (Registry on Windows)

## 🚀 Build Process

### Required Build Steps
1. Set bundle identifier
2. Configure player settings
3. Set up signing (Android/iOS)
4. Add all scenes to build
5. Build and test

### Build Configurations
```
Development Build:
- Script Debugging: Enabled
- Profiler: Enabled
- Optimization: None

Release Build:
- Script Debugging: Disabled
- Profiler: Disabled  
- Optimization: Maximum
- Code Stripping: High
```

## 📝 Notes

### Important Files
- **README.md** - Main documentation
- **LICENSE** - MIT License
- **.gitignore** - Git ignore rules
- **STRUCTURE.md** - This file

### Maintenance
- Keep dependencies updated
- Regular testing on devices
- Monitor performance metrics
- Update documentation as needed

---

*Last Updated: December 2025*
