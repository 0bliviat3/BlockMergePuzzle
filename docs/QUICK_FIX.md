# 🚨 즉시 해결 가이드 - 3분 체크리스트

## 현재 상황
- 블록 1개만 하얗게 보임
- 터치 안됨
- 게임 시작 안됨

## ✅ 3분 긴급 체크

### 1️⃣ EventSystem 확인 (30초)
```
Hierarchy 창에서:
- "EventSystem" 오브젝트가 있나요?

없으면:
Hierarchy 우클릭 → UI → Event System
```

### 2️⃣ Block 프리팹 Layer 설정 (30초)
```
Project 창에서:
1. Block 프리팹 선택
2. Inspector 상단 Layer → BlockLayer 선택
3. "Yes, change children" 클릭

BlockLayer가 없다면:
Edit → Project Settings → Tags and Layers
→ User Layer 8에 "BlockLayer" 입력
```

### 3️⃣ Block 프리팹 BoxCollider2D 확인 (30초)
```
Block 프리팹 선택 후:
- BoxCollider2D 컴포넌트가 있나요?
- Size가 100x100인가요?

없으면:
Add Component → Box Collider 2D
Size: X=100, Y=100
```

### 4️⃣ GameManager 참조 연결 (60초)
```
Hierarchy에서 GameManager 선택:

필수 연결 확인:
✓ Grid → Grid 컴포넌트
✓ Block Merger → BlockMerger 컴포넌트
✓ Score Manager → ScoreManager 컴포넌트
✓ Effect Manager → EffectManager 컴포넌트
✓ Input Handler → InputHandler 컴포넌트

하나라도 None이면 연결!
```

### 5️⃣ Grid 참조 연결 (30초)
```
Grid 컴포넌트 Inspector:

✓ Block Prefab → Block 프리팹 드래그
✓ Cell Prefab → Cell 프리팹 드래그
✓ Grid Container → GridContainer 드래그
✓ Blocks Container → BlocksContainer 드래그
```

---

## 🔧 빠른 테스트 방법

### 방법 A: 간단 테스트 (추천)

1. **GameManager 비활성화**
```
Hierarchy에서 GameManager 선택
Inspector 상단 체크박스 해제 (비활성화)
```

2. **새 GameObject 생성**
```
Hierarchy 우클릭 → Create Empty
이름: TestManager
```

3. **SimpleGameManager 추가**
```
TestManager 선택
Add Component → Simple Game Manager
Grid: Grid 컴포넌트 드래그
```

4. **Play 테스트**
```
Play 버튼 클릭
Console 창(Ctrl+Shift+C) 확인
```

### 결과 판단
```
✓ "블록 1 생성 완료" ~ "블록 5 생성 완료" 보임
  → Grid와 Block 프리팹은 정상!

✗ "블록 생성 실패" 또는 에러 메시지
  → 아래 상세 체크 필요
```

---

## 🎯 상세 체크 (문제가 계속되면)

### Block 프리팹 완전 체크
```
Block 프리팹 더블클릭:

필수 컴포넌트:
✓ RectTransform (자동)
✓ Canvas Renderer (자동)
✓ Image
✓ BoxCollider2D
✓ Block (스크립트)

자식 오브젝트:
✓ LevelText (TextMeshProUGUI)

Block 스크립트 Inspector:
✓ Block Image → Image 컴포넌트 드래그
✓ Level Text → LevelText 오브젝트 드래그
```

### InputHandler 완전 체크
```
InputHandler 컴포넌트:

✓ Block Merger → BlockMerger 컴포넌트
✓ Main Camera → Main Camera
✓ Block Layer → BlockLayer (드롭다운에서 선택)

!!!주의: "Nothing"이나 "Everything"이 아니라
         반드시 "BlockLayer"만 선택!!!
```

---

## 📱 Android 빌드 후 테스트

### logcat으로 로그 확인
```
PC에 폰 연결 후:

방법 1: Android Studio
- Logcat 탭 열기
- 필터: "Unity"

방법 2: 명령어
adb logcat -s Unity

찾아볼 내용:
- "블록 초기화"가 5번 나오나요?
- "NullReferenceException"이 있나요?
- 에러 메시지가 있나요?
```

---

## 💡 가장 흔한 원인 TOP 3

### 1. EventSystem이 없음 (70%)
```
해결: UI → Event System 추가
```

### 2. Block Layer 설정 안됨 (20%)
```
해결: 
1. Tags and Layers에 BlockLayer 추가
2. Block 프리팹 Layer를 BlockLayer로
3. InputHandler에서 BlockLayer 선택
```

### 3. 참조 연결 안됨 (10%)
```
해결: Inspector에서 None인 필드 모두 연결
```

---

## 🆘 그래도 안되면

### Console 로그 캡처해서 공유
```
Unity Editor에서:
1. Play 버튼
2. Console 창 열기 (Ctrl+Shift+C)
3. 우클릭 → Copy All
4. 텍스트 파일로 저장해서 공유
```

### 파일 공유
```
다음 파일들을 확인:
1. Block.prefab 설정 스크린샷
2. GameManager Inspector 스크린샷
3. Grid Inspector 스크린샷
4. Hierarchy 구조 스크린샷
```

---

## ⚡ 초고속 해결 (1분)

가장 빠른 방법:

1. **EventSystem 추가** (없다면)
2. **Block 프리팹 Layer → BlockLayer**
3. **Grid.blockPrefab에 Block 프리팹 드래그**
4. **Play 테스트**

이 3가지만 해도 80% 해결됩니다!
