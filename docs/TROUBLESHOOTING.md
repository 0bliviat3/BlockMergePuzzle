# 🔧 긴급 트러블슈팅 가이드

## 현재 증상 분석
- ✅ 5x5 그리드는 보임 (Grid는 작동)
- ❌ 블록이 1개만 있고 하얀색
- ❌ 터치 입력이 안됨
- ❌ 게임이 시작되지 않음

## 🚨 즉시 확인할 사항

### 1단계: GameDebugger 추가 (필수!)

```
1. GameDebugger.cs를 GameManager 오브젝트에 추가
2. Unity에서 Play 버튼 누르기
3. Console 창(Ctrl+Shift+C)에서 로그 확인
4. 문제 지점 파악
```

### 2단계: 핵심 체크리스트

#### ✓ EventSystem 확인
```
Hierarchy에서 확인:
- EventSystem 오브젝트가 있는가?
- 없으면: Hierarchy 우클릭 → UI → Event System
```

#### ✓ GameManager 참조 연결
```
GameManager Inspector에서 모든 필드 확인:

[필수 연결]
✓ Grid → Grid 컴포넌트
✓ Block Merger → BlockMerger 컴포넌트  
✓ Score Manager → ScoreManager 컴포넌트
✓ Effect Manager → EffectManager 컴포넌트
✓ Input Handler → InputHandler 컴포넌트

[UI 연결]
✓ Game Over Panel → GameOverPanel 오브젝트
✓ Game Over Text → GameOverText TMP
✓ Restart Button → RestartButton
✓ Highest Block Text → HighestBlockText TMP

누락된 게 하나라도 있으면 NullReferenceException!
```

#### ✓ Grid 참조 연결
```
Grid Inspector 확인:

✓ Block Prefab → Block 프리팹 드래그
✓ Cell Prefab → Cell 프리팹 드래그
✓ Grid Container → GridContainer 오브젝트
✓ Blocks Container → BlocksContainer 오브젝트

Grid Size: 5
Cell Size: 100
Cell Spacing: 10
```

#### ✓ Block 프리팹 설정
```
Block 프리팹 선택 후 확인:

컴포넌트:
✓ Image (색상: 흰색이 아닌 다른 색)
✓ BoxCollider2D (크기: 100x100)
✓ Block.cs 스크립트

자식 오브젝트:
✓ LevelText (TextMeshProUGUI)

Block.cs Inspector:
✓ Block Image → Image 컴포넌트 참조
✓ Level Text → LevelText TMP 참조
```

#### ✓ InputHandler 설정
```
InputHandler Inspector:

✓ Block Merger → BlockMerger 컴포넌트
✓ Main Camera → Main Camera
✓ Block Layer → BlockLayer 선택

!!!중요: Block Layer 반드시 설정!!!
```

#### ✓ Layer 설정
```
1. Block 프리팹 선택
2. Inspector 상단 Layer → BlockLayer
3. "Yes, change children" 클릭

Layer가 없다면:
Edit → Project Settings → Tags and Layers
→ User Layer 8: BlockLayer 추가
```

## 🔍 디버깅 절차

### 방법 1: Unity Editor에서 확인

```
1. Unity에서 Play 버튼
2. Console 창 열기 (Ctrl+Shift+C)
3. GameDebugger 로그 확인
4. 에러 메시지 찾기
```

### 방법 2: Android 로그 확인

```
1. PC에 폰 연결
2. Android Studio → Logcat 열기
3. 또는: adb logcat -s Unity
4. 에러 찾기
```

### 방법 3: 디버그 파일 확인

```
GameDebugger가 자동으로 생성하는 파일 확인:
Android: /storage/emulated/0/Android/data/[패키지명]/files/debug_log.txt

파일 탐색:
1. 폰에서 "파일" 앱 열기
2. Android/data/[패키지명]/files/
3. debug_log.txt 확인
```

## 🛠️ 자주 발생하는 문제와 해결

### 문제 1: "NullReferenceException"
```
원인: 참조가 연결되지 않음
해결:
1. Console에서 에러가 발생한 스크립트 확인
2. 해당 스크립트의 Inspector에서 모든 참조 연결
3. 특히 GameManager, Grid, BlockMerger 집중 확인
```

### 문제 2: 블록이 생성되지 않음
```
원인: 프리팹이 없거나 참조 누락
해결:
1. Grid.blockPrefab이 null인지 확인
2. Block 프리팹이 제대로 설정되었는지 확인
3. Grid.Initialize()가 호출되었는지 확인
```

### 문제 3: 터치가 안됨
```
원인 1: EventSystem이 없음
→ UI → Event System 추가

원인 2: BoxCollider2D가 없음
→ Block 프리팹에 BoxCollider2D 추가

원인 3: Layer가 잘못됨
→ Block 프리팹 Layer를 BlockLayer로 변경
→ InputHandler의 Block Layer 설정 확인

원인 4: Canvas의 Raycast Target이 꺼짐
→ GridContainer, BlocksContainer의 Raycast Target 체크 해제 확인
```

### 문제 4: 하얀색 블록만 보임
```
원인: Block.cs의 UpdateVisuals()가 작동하지 않음
해결:
1. Block 프리팹에서 Block.cs 확인
2. blockImage 참조가 연결되었는지 확인
3. levelText 참조가 연결되었는지 확인
```

### 문제 5: 게임이 시작되지 않음
```
원인: GameManager.StartNewGame()이 호출되지 않음
해결:
1. GameManager의 Start()가 실행되는지 확인
2. Console에서 "게임 시작" 로그 확인
3. Initialize()에서 에러가 없는지 확인
```

## 🎯 빠른 수정 방법

### 임시 테스트 씬 만들기

1. **새 씬 생성**
```
File → New Scene → 2D
```

2. **최소 구성**
```
Canvas (Screen Space - Overlay)
├── GridContainer (Empty)
├── BlocksContainer (Empty)
└── EventSystem (자동 생성)

GameManager (Empty GameObject)
└── 모든 컴포넌트 추가
```

3. **필수만 연결**
```
GameManager:
- Grid
- BlockMerger
- 다른 건 일단 null로 두고 테스트
```

4. **Play 테스트**
```
- 블록이 생성되는가?
- Console에 에러가 있는가?
```

## 🔧 최종 점검 스크립트

Inspector에서 실행할 수 있는 검증 스크립트를 만들어드렸습니다.
GameDebugger.cs를 GameManager에 추가하고 Play하면 자동으로 모든 것을 검사합니다!

### GameDebugger 사용법

1. **추가**
```
GameManager 오브젝트 선택
→ Add Component
→ GameDebugger
```

2. **실행**
```
Play 버튼 클릭
화면 왼쪽 상단에 디버그 정보 표시됨
Console 창에서 상세 로그 확인
```

3. **로그 확인**
```
=== 게임 진단 시작 ===
[씬 구조 체크]
Canvas 존재: True/False
...
=== 진단 완료 ===

각 항목에서 False가 나오면 해당 부분 수정 필요!
```

## 📱 Android 전용 문제

### 권한 문제
```
Player Settings → Android
→ Write Permission: External (SD Card)
```

### 터치 지연
```
Player Settings → Other Settings
→ Multithreaded Rendering: 체크 해제
```

### 해상도 문제
```
Canvas Scaler 확인:
- UI Scale Mode: Scale With Screen Size
- Reference Resolution: 1080 x 1920
- Match: 0.5
```

## 🆘 그래도 안되면...

### 완전 초기화 후 재시작

1. **Unity에서**
```
Edit → Clear All PlayerPrefs
Assets → Reimport All
```

2. **Android에서**
```
앱 삭제 후 재설치
```

3. **처음부터 다시**
```
새 씬 생성
최소 구성으로 테스트
하나씩 추가하면서 어디서 깨지는지 확인
```

## 💡 다음 단계

1. **GameDebugger 추가하고 Play**
2. **Console 로그 캡처해서 공유**
3. **어떤 항목이 False인지 확인**
4. **해당 부분부터 수정**

GameDebugger가 모든 것을 자동으로 체크해주니까, 이걸 먼저 실행해보세요!
그러면 정확히 어디가 문제인지 바로 알 수 있습니다. 🎯
