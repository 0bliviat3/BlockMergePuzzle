# Block Prefab Raycast 설정 가이드

## 🎯 문제 상황
BlockMerge 게임에서 블록 클릭이 인식되지 않는 문제

## 📝 원인
Block 프리팹의 Image 컴포넌트에서 `Raycast Target`이 비활성화됨

## ✅ 해결 방법

### 1️⃣ Unity Editor에서 Block 프리팹 설정

#### 단계:
1. **Project 창** → `Assets/Prefabs/` → `Block.prefab` 찾기
2. **Block 프리팹 더블클릭** (또는 Inspector에서 Open Prefab)
3. **Hierarchy에서 Block 선택**
4. **Inspector 창에서 Image 컴포넌트 찾기**
5. **Raycast Target 체크박스 활성화** ✅

#### 시각적 가이드:
```
Inspector
├── Transform
├── Rect Transform
├── Canvas Renderer
└── Image (Script)
    ├── Source Image: None
    ├── Color: White
    ├── Material: None
    └── ☑️ Raycast Target  ← 여기를 체크!
```

### 2️⃣ 코드로 자동 활성화 (이미 적용됨)

**Block.cs의 Awake()에서 자동 설정:**
```csharp
private void Awake()
{
    blockImage = GetComponent<Image>();
    if (blockImage != null)
    {
        blockImage.raycastTarget = true; // 자동 활성화
    }
}
```

### 3️⃣ Canvas 설정 확인

**Canvas에 필요한 컴포넌트:**
- ✅ Canvas
- ✅ Canvas Scaler
- ✅ **Graphic Raycaster** ← 자동으로 추가됨
- ✅ Canvas Renderer

**InputHandler.cs가 자동으로 추가함:**
```csharp
if (graphicRaycaster == null)
{
    graphicRaycaster = canvas.gameObject.AddComponent<GraphicRaycaster>();
}
```

## 🧪 테스트 방법

### Console 로그 확인:

#### 정상 작동 시:
```
=== InputHandler Start ===
✓ Canvas 발견: Canvas
✓ GraphicRaycaster 찾음
✓ EventSystem 찾음
✓ BlockMerger 연결됨

[InputHandler] 입력 감지: (696.25, 1485.42)
Raycast 결과: 3개 오브젝트
- 히트: Block(Clone), Layer: Default
  → Image 발견, raycastTarget: True
✓ Block 컴포넌트 찾음! 레벨: 1
블록 클릭됨: 레벨 1, 위치 (2, 3)
```

#### 문제 있을 시:
```
Raycast 결과: 0개 오브젝트
블록이 클릭되지 않음
```

### Unity Play Mode에서 확인:

1. **Play 모드 실행**
2. **Hierarchy에서 실시간 블록 확인**
   ```
   Canvas
   └── Blocks Container
       └── Block(Clone)
           └── Check Inspector → Image → Raycast Target
   ```
3. **블록 클릭 시 Console 확인**

## 🔧 추가 수정 사항

### Block.cs
```csharp
✅ raycastTarget 자동 활성화
✅ 디버그 로그 추가
❌ BoxCollider2D 제거 (UI에서는 불필요)
```

### InputHandler.cs
```csharp
✅ GraphicRaycaster 자동 추가
✅ EventSystem 자동 생성
✅ 상세 디버그 로그
✅ raycastTarget 상태 출력
```

## 🎮 빌드 후 테스트

### Android/iOS에서:
1. 빌드 및 설치
2. BlockMerge 게임 실행
3. 블록 터치
4. 정상 작동 확인

### 문제 발생 시:
- **Unity Editor → Play Mode로 먼저 테스트**
- **Console 로그 확인**
- **Hierarchy에서 Block(Clone) 확인**
  - Image 컴포넌트 있는가?
  - Raycast Target 활성화되어 있는가?
- **Canvas에 GraphicRaycaster 있는가?**

## 📱 모바일 터치 디버깅

### 로그를 통한 확인:
```
1. ADB (Android) 또는 Xcode Console (iOS)에서 로그 확인
2. "Raycast 결과: N개 오브젝트" 확인
3. N > 0 이면: Block 컴포넌트 문제
4. N = 0 이면: raycastTarget 또는 Canvas 문제
```

## ✨ 요약

### 해결됨:
- ✅ Block.cs에서 raycastTarget 자동 활성화
- ✅ InputHandler에서 GraphicRaycaster 자동 추가
- ✅ EventSystem 자동 생성
- ✅ 상세 디버그 로그

### 수동 확인 필요:
- ⚠️ Block 프리팹의 Raycast Target 체크 (Unity Editor)
- ⚠️ 기존 씬의 Canvas 설정

### 다음 단계:
1. Unity Editor에서 Block 프리팹 열기
2. Raycast Target 체크
3. Play Mode로 테스트
4. 정상 작동하면 빌드

---

**작성일:** 2024-12-10
**파일 위치:** E:\claude_src\BlockMergePuzzle\RAYCAST_FIX.md
