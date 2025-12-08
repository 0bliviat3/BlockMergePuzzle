# Unity 프로젝트 설정 가이드

## 🎯 빠른 시작

### 1단계: Unity 프로젝트 열기
```
1. Unity Hub 실행
2. "Open" → BlockMergePuzzle 폴더 선택
3. Unity 버전: 2021.3 LTS 이상 권장
```

### 2단계: 필수 에셋 다운로드

#### LeanTween 설치
```
1. GitHub에서 LeanTween.cs 다운로드
   URL: https://github.com/dentedpixel/LeanTween
2. Assets/Scripts/ 폴더에 LeanTween.cs 복사
```

### 3단계: 씬 설정

#### 메인 Canvas 생성
```csharp
// Hierarchy에서 우클릭
UI → Canvas

Canvas 설정:
- Render Mode: Screen Space - Overlay
- Canvas Scaler:
  - UI Scale Mode: Scale With Screen Size
  - Reference Resolution: 1080 x 1920 (세로 모드)
  - Match: 0.5
```

#### Grid Container 설정
```csharp
// Canvas 하위에 Empty GameObject 생성
이름: GridContainer

RectTransform:
- Anchors: Center
- Position: (0, 0, 0)
- Scale: (1, 1, 1)
```

#### Blocks Container 설정
```csharp
// Canvas 하위에 Empty GameObject 생성
이름: BlocksContainer

RectTransform:
- Anchors: Center
- Position: (0, 0, 0)
- Scale: (1, 1, 1)
```

### 4단계: 프리팹 생성

#### Block Prefab
```
1. Hierarchy에서 우클릭
   UI → Image

2. 이름: Block

3. 컴포넌트 추가:
   - Image (자동 추가됨)
   - BoxCollider2D
   - Block (스크립트)

4. 자식 오브젝트 추가:
   - TextMeshPro - Text
   - 이름: LevelText
   - Font Size: 60
   - Alignment: Center, Middle
   - Color: Black

5. RectTransform 설정:
   - Width: 100
   - Height: 100

6. Prefabs 폴더에 드래그하여 저장
```

#### Cell Prefab
```
1. Hierarchy에서 우클릭
   UI → Image

2. 이름: Cell

3. Image 설정:
   - Color: RGBA(200, 190, 180, 100)

4. RectTransform 설정:
   - Width: 100
   - Height: 100

5. Prefabs 폴더에 드래그하여 저장
```

### 5단계: UI 생성

#### 점수 UI
```
1. Canvas 하위에 Empty GameObject 생성
   이름: UI

2. UI 하위에 생성:

ScoreText (TextMeshPro):
- Text: "점수: 0"
- Font Size: 48
- Color: White
- Alignment: Left, Top
- Position: (-450, 800)

HighScoreText (TextMeshPro):
- Text: "최고 점수: 0"
- Font Size: 36
- Color: Yellow
- Alignment: Left, Top
- Position: (-450, 740)

HighestBlockText (TextMeshPro):
- Text: "최고 블록: 2"
- Font Size: 36
- Color: Cyan
- Alignment: Right, Top
- Position: (450, 800)
```

#### 콤보 UI
```
ComboPanel (GameObject):
- 위치: (0, 600)

ComboText (TextMeshPro):
- Text: "콤보 x1"
- Font Size: 48
- Color: Orange
- Alignment: Center, Middle
```

#### 게임 오버 UI
```
GameOverPanel (GameObject):
- Image 컴포넌트 추가
- Color: RGBA(0, 0, 0, 200)
- RectTransform: Stretch to fill canvas

GameOverText (TextMeshPro):
- Text: "게임 오버!"
- Font Size: 72
- Color: White
- Alignment: Center, Middle
- Position: (0, 200)

RestartButton (Button):
- Text: "다시 시작"
- Position: (0, -100)
- Width: 300, Height: 100
```

### 6단계: GameManager 설정

```
1. Hierarchy에 Empty GameObject 생성
   이름: GameManager

2. 컴포넌트 추가:
   - Grid
   - BlockMerger
   - GameManager
   - ScoreManager
   - EffectManager
   - InputHandler

3. Inspector에서 참조 연결:

Grid:
- Grid Size: 5
- Cell Size: 100
- Cell Spacing: 10
- Block Prefab: Block 프리팹 드래그
- Cell Prefab: Cell 프리팹 드래그
- Grid Container: GridContainer 드래그
- Blocks Container: BlocksContainer 드래그

BlockMerger:
- Explode Level: 10
- Explode Radius: 1
- Grid: Grid 컴포넌트 참조
- Score Manager: ScoreManager 컴포넌트 참조
- Effect Manager: EffectManager 컴포넌트 참조

GameManager:
- Starting Blocks: 3
- New Blocks Per Turn: 1
- Grid: Grid 컴포넌트 참조
- Block Merger: BlockMerger 컴포넌트 참조
- Score Manager: ScoreManager 컴포넌트 참조
- Effect Manager: EffectManager 컴포넌트 참조
- Input Handler: InputHandler 컴포넌트 참조
- Game Over Panel: GameOverPanel 드래그
- Game Over Text: GameOverText 드래그
- Restart Button: RestartButton 드래그
- Highest Block Text: HighestBlockText 드래그

ScoreManager:
- Combo Time Limit: 3
- Combo Multiplier: 1.5
- Score Text: ScoreText 드래그
- High Score Text: HighScoreText 드래그
- Combo Text: ComboText 드래그
- Combo Panel: ComboPanel 드래그

EffectManager:
- Audio Source: AudioSource 컴포넌트 추가 및 참조

InputHandler:
- Block Merger: BlockMerger 컴포넌트 참조
- Main Camera: Main Camera 드래그
- Block Layer: BlockLayer 선택
```

### 7단계: Layer 설정

```
1. Edit → Project Settings → Tags and Layers

2. Layers에 추가:
   User Layer 8: BlockLayer

3. Block 프리팹 선택
   Inspector → Layer → BlockLayer 선택
```

### 8단계: Physics 2D 설정

```
Edit → Project Settings → Physics 2D

Gravity: (0, 0)  // 중력 제거
```

### 9단계: 빌드 설정

#### Android
```
File → Build Settings → Android

Player Settings:

Company Name: [회사명]
Product Name: Block Merge Puzzle
Package Name: com.[회사명].blockmerge
Version: 1.0.0

Resolution and Presentation:
- Default Orientation: Portrait
- Allowed Orientations: Portrait only

Other Settings:
- Scripting Backend: IL2CPP
- Target Architectures: ARM64
- Minimum API Level: 21 (Android 5.0)
- Target API Level: Automatic

Publishing Settings:
- Create Keystore 또는 기존 Keystore 사용
```

#### iOS
```
File → Build Settings → iOS

Player Settings:

Company Name: [회사명]
Product Name: Block Merge Puzzle
Bundle Identifier: com.[회사명].blockmerge
Version: 1.0.0

Resolution and Presentation:
- Default Orientation: Portrait
- Allowed Orientations: Portrait only

Other Settings:
- Target minimum iOS Version: 12.0
- Architecture: ARM64
```

## 🎨 추가 커스터마이징

### 블록 스킨 변경
```csharp
Assets/Sprites/ 폴더 생성

블록 이미지 추가:
- block_background.png
- block_border.png

Block 프리팹의 Image 컴포넌트:
- Source Image: 원하는 스프라이트 선택
```

### 배경 추가
```
Canvas 하위에 Image 생성:
- 이름: Background
- Anchors: Stretch
- Source Image: 배경 이미지
- Move to Top (Hierarchy에서 맨 위로)
```

### 사운드 추가
```
Assets/Audio/ 폴더 생성

필요한 사운드:
- merge_sound.wav
- explode_sound.wav
- milestone_sound.wav
- bgm.mp3

EffectManager:
- Merge Sound: merge_sound 드래그
- Explode Sound: explode_sound 드래그
- Milestone Sound: milestone_sound 드래그

배경음악:
- AudioSource 추가 (GameManager에)
- AudioClip: bgm 드래그
- Loop: 체크
- Play On Awake: 체크
```

## 🔧 문제 해결

### TextMeshPro 에러
```
Window → TextMeshPro → Import TMP Essential Resources
```

### 스크립트 컴파일 에러
```
1. LeanTween.cs 설치 확인
2. Assets → Reimport All
```

### 터치가 작동하지 않음
```
1. EventSystem 확인 (자동 생성)
2. Block Layer 설정 확인
3. Canvas의 Raycast Target 확인
```

### 애니메이션이 느림
```
Edit → Project Settings → Time
Time Scale: 1.0 확인
```

## ✅ 체크리스트

설정 완료 전 확인사항:

- [ ] LeanTween.cs 설치
- [ ] Block 프리팹 생성 완료
- [ ] Cell 프리팹 생성 완료
- [ ] Canvas 설정 완료
- [ ] UI 요소 모두 생성
- [ ] GameManager 설정 완료
- [ ] 모든 참조 연결 완료
- [ ] Layer 설정 완료
- [ ] Physics 2D 설정 완료
- [ ] 빌드 설정 완료
- [ ] 테스트 플레이 성공

## 🎮 테스트

Play 버튼 클릭 후 확인:
1. 그리드가 표시되는가?
2. 시작 블록들이 생성되는가?
3. 블록을 클릭할 수 있는가?
4. 같은 레벨 블록이 병합되는가?
5. 점수가 증가하는가?
6. 새 블록이 추가되는가?
7. 레벨 10 블록이 폭발하는가?

모두 확인되면 설정 완료! 🎉
