# 2048 Collection

A mobile-optimized multi-game collection featuring two addictive puzzle games built with Unity.

![Unity Version](https://img.shields.io/badge/Unity-2022.3+-blue)
![Platform](https://img.shields.io/badge/Platform-Android%20%7C%20iOS-green)
![Status](https://img.shields.io/badge/Status-Production%20Ready-success)

## 🎮 Games

### 1. Block Merge Puzzle
A strategic block-merging game where you combine numbered blocks to reach higher scores.

**Features:**
- Grid-based gameplay with physics-based block dropping
- Explosive merge effects with particle systems
- Progressive difficulty scaling
- Chain combo system with score multipliers
- Game over detection with replay option

**How to Play:**
- Tap to drop blocks onto the grid
- Merge blocks with the same number
- Create chains for bonus points
- Survive as long as possible

### 2. Classic 2048
The timeless 2048 game with infinite play mode and mobile-optimized controls.

**Features:**
- Smooth swipe controls
- Infinite play mode (no 2048 win limit)
- Best score tracking with local storage
- Responsive grid layout (1.5x larger tiles for mobile)
- Complete animation system with LeanTween

**How to Play:**
- Swipe in any direction to move tiles
- Combine matching numbers to create larger tiles
- Reach 2048, 4096, 8192, and beyond!
- Beat your high score

## 🏗️ Architecture

### Multi-Game Structure
```
2048 Collection/
├── MainMenu          # Game launcher with navigation
├── BlockMergePuzzle  # Main puzzle game
└── Classic2048       # Classic 2048 game
```

### Key Systems

#### Scene Management
- **SceneLoader**: Smooth fade transitions between scenes
- **DontDestroyOnLoad**: Persistent audio and scene loader
- Automatic canvas cleanup on scene transitions

#### Audio System
- **AudioManager**: Singleton audio controller
- BGM (Background Music) with seamless looping
- SFX (Sound Effects) for clicks, merges, and explosions
- Volume controls and mute functionality

#### Grid System
- **Grid**: Dynamic grid with configurable size and spacing
- **BlockMerger**: Handles merge logic and chain detection
- **Classic2048Grid**: Complete rewrite with proper state management

#### UI System
- Automatic UI generation with code
- No scene-based UI setup required
- Consistent styling across all scenes
- Mobile-optimized button sizes and positioning

## 📱 Mobile Optimization

### Touch Controls
- Swipe detection with configurable sensitivity
- Touch-to-place for block merging
- No keyboard input (mobile-only)

### Visual Optimization
- Large, readable fonts (LegacyRuntime.ttf)
- 210x210 tile size for comfortable viewing
- High-contrast colors for visibility
- Responsive layouts that adapt to screen size

### Performance
- Object pooling for particles
- Efficient grid state management
- Optimized animation using LeanTween
- Minimal memory footprint

## 🎨 Design Patterns

### Singleton Pattern
```csharp
// Used for global managers
- AudioManager
- SceneLoader
- GameManager
- Classic2048Manager
```

### Component Pattern
```csharp
// Modular game components
- Grid (data structure)
- BlockMerger (game logic)
- EffectManager (visual effects)
- ScoreManager (scoring system)
```

### State Management
```csharp
// Clean state handling
- isGameOver flag
- isProcessingMove lock
- Grid array synchronization
```

## 🔧 Technical Details

### Core Technologies
- **Unity 2022.3+**
- **C# .NET Standard 2.1**
- **LeanTween** for animations
- **TextMesh Pro** fallback to Legacy UI Text

### Key Scripts

#### Shared Components
```
/Assets/Scripts/Shared/
├── AudioManager.cs      # Global audio controller
├── SceneLoader.cs       # Scene transition manager
└── GameConfig.cs        # Game settings and constants
```

#### Block Merge Puzzle
```
/Assets/Scripts/
├── GameManager.cs       # Main game controller
├── Grid.cs              # Grid data structure
├── Block.cs             # Individual block logic
├── BlockMerger.cs       # Merge detection and logic
├── ScoreManager.cs      # Score tracking and display
├── EffectManager.cs     # Particle effects
└── InputHandler.cs      # Touch input processing
```

#### Classic 2048
```
/Assets/Scripts/Classic2048/
├── Classic2048Manager.cs   # Game controller with auto UI
├── Classic2048Grid.cs      # Complete grid rewrite
├── Classic2048Tile.cs      # Tile behavior and animation
└── Classic2048Input.cs     # Swipe detection
```

#### Main Menu
```
/Assets/Scripts/MainMenu/
└── MainMenuManager.cs      # Menu UI and navigation
```

### Grid Algorithm (Classic 2048)

The grid movement uses a **line-based processing** approach:

```csharp
// Process each row/column independently
1. Collect tiles in movement direction
2. Merge adjacent matching tiles
3. Reposition merged result
4. Update grid state synchronously
```

**Key Improvements:**
- No diagonal movement bugs
- No tile overlap issues
- Instant state updates
- Clean separation of logic

## 🎯 Game Features

### Block Merge Puzzle
- ✅ Physics-based block dropping
- ✅ Explosive merge effects
- ✅ Combo chain detection
- ✅ Progressive difficulty
- ✅ Score multipliers
- ✅ Game over detection
- ✅ Replay functionality

### Classic 2048
- ✅ Infinite play mode
- ✅ Smooth swipe controls
- ✅ Best score persistence
- ✅ No win limit (play beyond 2048)
- ✅ Proper merge logic
- ✅ Animation system
- ✅ Mobile-optimized layout

### Shared Features
- ✅ BGM and SFX
- ✅ Scene transitions with fade
- ✅ Back to menu button
- ✅ Consistent UI styling
- ✅ Auto-generated interfaces

## 🚀 Getting Started

### Prerequisites
- Unity 2022.3 or later
- LeanTween package
- Android Build Support / iOS Build Support

### Installation

1. Clone the repository:
```bash
git clone https://github.com/0bliviat3/BlockMergePuzzle.git
```

2. Open in Unity:
   - Launch Unity Hub
   - Click "Add" → Select project folder
   - Open project

3. Build Settings:
   - File → Build Settings
   - Switch Platform to Android/iOS
   - Add all scenes:
     - MainMenu
     - BlockMergePuzzle (SampleScene)
     - Classic2048

### Running the Game

1. **In Editor:**
   - Open `MainMenu` scene
   - Press Play button
   - Navigate to desired game

2. **On Device:**
   - File → Build Settings → Build
   - Install APK/IPA on device
   - Launch and play

## 📖 Development Guide

### Adding a New Game

1. **Create Scene:**
```csharp
// Create new scene in Scenes folder
Assets/Scenes/NewGame.unity
```

2. **Create Manager:**
```csharp
public class NewGameManager : MonoBehaviour
{
    public static NewGameManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    // Game logic here
}
```

3. **Add to MainMenu:**
```csharp
// In MainMenuManager.cs
private void CreateGameButton(string title, string sceneName)
{
    // Button creation code
}
```

### Modifying Grid Size

**Block Merge Puzzle:**
```csharp
// In Grid.cs
public int width = 6;  // Change grid width
public int height = 10; // Change grid height
```

**Classic 2048:**
```csharp
// In Classic2048Grid.cs
public int gridSize = 4;    // Default: 4x4
public float cellSize = 210f; // Tile size
```

### Adjusting Difficulty

**Block Merge Puzzle:**
```csharp
// In GameManager.cs
private float spawnDelay = 1.5f;     // Time between spawns
private int maxBlockValue = 64;      // Highest number
private float difficultyIncrease = 0.95f; // Speed multiplier
```

### Custom Animations

Using LeanTween for smooth animations:
```csharp
// Scale animation
LeanTween.scale(gameObject, Vector3.one * 1.2f, 0.2f)
    .setEase(LeanTweenType.easeOutQuad);

// Move animation
LeanTween.move(rectTransform, targetPosition, 0.15f)
    .setEase(LeanTweenType.easeOutQuad);

// Always cancel previous animations
LeanTween.cancel(gameObject);
```

## 🐛 Known Issues & Solutions

### Issue: Black Rectangle on Scene Transition
**Cause:** Canvas not properly cleaned up  
**Solution:** Implemented in `SceneLoader.cs` with canvas cleanup and `OnDestroy()` methods

### Issue: Tiles Moving Diagonally
**Cause:** Grid state and visual position desync  
**Solution:** Complete grid rewrite with synchronous state updates

### Issue: Blocks Overlapping
**Cause:** Animation conflicts  
**Solution:** Proper `LeanTween.cancel()` before new animations

### Issue: Score Not Displaying
**Cause:** `ref` parameter not working with Text components  
**Solution:** Return Text component directly from creation method

## 📊 Performance Metrics

### Target Performance
- **FPS:** 60fps on mid-range devices
- **Memory:** < 200MB RAM usage
- **Load Time:** < 1 second scene transitions
- **Responsiveness:** < 16ms input latency

### Optimization Techniques
- Object pooling for particles
- Efficient grid state management
- Minimal garbage collection
- Optimized animation curves

## 🎨 Asset Credits

### Fonts
- **LegacyRuntime.ttf** - Unity built-in font

### Audio
- BGM tracks (add credits)
- SFX samples (add credits)

### Visual Design
- All UI generated programmatically
- Color scheme: 2048-inspired palette

## 📝 Version History

### v1.0.0 (Current)
- ✅ Complete multi-game architecture
- ✅ Block Merge Puzzle with combos
- ✅ Classic 2048 infinite mode
- ✅ Mobile optimization (1.5x larger tiles)
- ✅ Audio system with BGM/SFX
- ✅ Scene transition system
- ✅ Auto-generated UI
- ✅ Bug fixes: diagonal movement, overlapping, black rectangle

### Development Highlights
- Complete Classic2048Grid rewrite
- Line-based merge algorithm
- Synchronous grid state management
- Canvas cleanup on scene transitions
- Mobile-optimized tile sizing

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch
3. Commit with clear messages
4. Test thoroughly on device
5. Submit pull request

## 📄 License

This project is licensed under the MIT License - see LICENSE file for details.

## 📞 Support

For issues, questions, or suggestions:
- Open an issue on GitHub
- Contact: [sjo9810@gmail.com]

---

**Built with ❤️ using Unity**

*Last Updated: December 2025*
