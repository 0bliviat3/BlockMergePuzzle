# MainMenu 씬 생성 가이드

## 단계별 생성 방법

### 1단계: 새 씬 생성
```
Unity:
1. File → New Scene
2. Hierarchy에 있는 모든 오브젝트 삭제
   - Main Camera 삭제
   - Directional Light 삭제
```

### 2단계: MainMenuManager 생성
```
Hierarchy:
1. 우클릭 → Create Empty
2. 이름: MainMenuManager
3. Inspector에서:
   - Add Component 클릭
   - "MainMenuManager" 검색
   - MainMenuManager 스크립트 추가
   - Auto Create UI: ✓ 체크
```

### 3단계: SceneLoader 생성 (중요!)
```
Hierarchy:
1. 우클릭 → Create Empty
2. 이름: SceneLoader
3. Inspector에서:
   - Add Component 클릭
   - "SceneLoader" 검색
   - SceneLoader 스크립트 추가
```

### 4단계: 씬 저장
```
1. Ctrl+S 또는 File → Save Scene As
2. 파일명: MainMenu
3. 저장 위치: Assets/Scenes/MainMenu.unity
```

### 5단계: Play 테스트
```
1. Play 버튼 클릭
2. Console 창 확인 (Ctrl+Shift+C)
3. 로그 확인:
   "=== MainMenuManager Start ==="
   "✓ EventSystem 생성"
   "✓ 메인 메뉴 UI 생성 완료"
```

## 예상되는 UI

Play 후 보여야 할 것:
```
┌─────────────────────────────────────┐
│                                     │
│      2048 COLLECTION (골드색)        │
│                                     │
│  ┌─────────────────────────────┐   │
│  │  Block Merge Puzzle         │   │
│  │  인접한 같은 숫자 블록 병합  │   │
│  └─────────────────────────────┘   │
│                                     │
│  ┌─────────────────────────────┐   │
│  │  Classic 2048               │   │
│  │  오리지널 2048 게임          │   │
│  └─────────────────────────────┘   │
│                                     │
│         [ SETTINGS ]                │
└─────────────────────────────────────┘
```

## 문제 해결

### 버튼이 안 보이면?
```
Console 확인:
- "MainMenuManager Start" 로그 있나요?
- "EventSystem 생성" 로그 있나요?
- 에러 메시지 있나요?
```

### Console에 에러가 있으면?
```
에러 메시지를 복사해서 알려주세요!
```

### 여전히 SET 버튼만 보이면?
```
BlockMergePuzzle 씬을 열고 있을 가능성!
- Hierarchy에 "GameManager" 오브젝트 있나요?
- 있으면 → 잘못된 씬입니다!
- MainMenu 씬을 다시 여세요!
```
