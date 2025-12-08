# 🚨 긴급 수정 가이드 - Camera & 한글 폰트

## 문제 1: Main Camera 없음 ⭐⭐⭐

### 즉시 해결 (30초)

#### 방법 A: Camera 태그 설정 (빠름)
```
1. Hierarchy에서 "Camera" 또는 "Main Camera" 찾기
2. 선택 후 Inspector 상단
3. Tag 드롭다운 → "MainCamera" 선택
```

#### 방법 B: Camera 생성 (Camera가 아예 없는 경우)
```
1. Hierarchy 우클릭
2. Camera 선택
3. 자동으로 "Main Camera" 태그 설정됨

설정 확인:
- Position: (0, 0, -10)
- Projection: Orthographic
- Size: 5
- Clear Flags: Solid Color
- Background: 원하는 배경색
```

---

## 문제 2: 한글 텍스트 깨짐 ⭐⭐⭐

### 해결 방법: TextMeshPro 한글 폰트 생성

#### 1단계: 한글 폰트 가져오기
```
무료 한글 폰트 다운로드:
- Noto Sans KR (Google Fonts 추천)
- 나눔고딕
- 또는 Windows 폰트 사용: C:\Windows\Fonts\malgun.ttf

다운로드 후:
Assets/Fonts/ 폴더에 .ttf 파일 복사
```

#### 2단계: TMP Font Asset 생성
```
1. Window → TextMeshPro → Font Asset Creator

2. 설정:
   Source Font File: [한글 폰트 선택]
   
   Sampling Point Size: 
   - Auto Sizing
   
   Character Set:
   - Custom Characters
   
   Custom Character List에 붙여넣기:
   ```
   ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789
   .,!?:;'"()[]{}+-*/=<>@#$%&
   가나다라마바사아자차카타파하
   거너더러머버서어저처커터퍼허
   게네데레메베세에제체케테페헤
   고노도로모보소오조초코토포호
   구누두루무부수우주추쿠투푸후
   그느드르므브스으즈츠크트프흐
   기니디리미비시이지치키티피히
   점수최고레벨블록콤보게임오버다시시작
   병합폭발연쇄힌트설정음악효과음
   저장불러오기업적통계순위공유
   123456789
   ```

3. Render Mode: 
   - SDFAA (Hinted) 추천
   
4. Atlas Resolution:
   - 2048 x 2048 (한글은 용량 큼)
   
5. Generate Font Atlas 클릭
   
6. Save 클릭
   - 저장 경로: Assets/Fonts/
   - 이름: KoreanFont_SDF
```

#### 3단계: 기본 폰트로 설정
```
방법 A: TMP Settings에서 설정
1. Edit → Project Settings → TextMeshPro → Settings
2. Default Font Asset → KoreanFont_SDF

방법 B: 각 텍스트에 개별 적용
1. Hierarchy에서 각 TextMeshProUGUI 선택
2. Font Asset → KoreanFont_SDF
```

---

## 문제 3: GameDebugger NullReference 수정

GameDebugger.cs를 수정했으니 다시 다운로드하세요.

---

## 🎯 올바른 씬 구조

### 필수 오브젝트 체크리스트

```
Hierarchy 구조:

✓ Main Camera (Tag: MainCamera)
  └── 필수!

✓ Canvas (Render Mode: Screen Space - Overlay)
  ├── GridContainer
  ├── BlocksContainer
  └── UI
      ├── ScoreText (TMP)
      ├── HighScoreText (TMP)
      └── ... (기타 UI)

✓ EventSystem
  └── Standalone Input Module

✓ GameManager
  ├── Grid
  ├── BlockMerger
  ├── GameManager
  ├── ScoreManager
  ├── EffectManager
  ├── InputHandler
  └── GameDebugger
```

---

## ⚡ 빠른 수정 순서 (5분)

### 1️⃣ Camera 추가/설정 (1분)
```
Hierarchy → Create → Camera
Tag: MainCamera 확인
```

### 2️⃣ 한글 폰트 임시 해결 (1분)
```
모든 TextMeshProUGUI 선택:
- Font Asset: 일단 "LiberationSans SDF" 사용
- 한글은 깨지지만 숫자는 보임
- 나중에 한글 폰트 추가
```

### 3️⃣ GameDebugger 재실행 (1분)
```
Play 버튼 → Console 확인
"Main Camera 존재: True" 확인
```

### 4️⃣ 빌드 테스트 (2분)
```
File → Build Settings → Build
APK 설치 후 테스트
```

---

## 📱 Canvas 설정 확인

### Canvas 컴포넌트
```
Render Mode: Screen Space - Overlay

Canvas Scaler:
- UI Scale Mode: Scale With Screen Size
- Reference Resolution: 1080 x 1920
- Screen Match Mode: Match Width Or Height
- Match: 0.5
```

### Canvas와 Camera 관계
```
Screen Space - Overlay 모드에서는:
- Canvas가 Camera 없이도 렌더링됨
- 하지만 InputHandler가 Camera.main 필요!
- 그래서 Main Camera는 반드시 필요!
```

---

## 🎨 TextMeshPro 한글 폰트 빠른 설정

### 간단한 방법 (Windows 폰트 사용)

```
1. C:\Windows\Fonts\malgun.ttf 복사
   → Assets/Fonts/malgun.ttf

2. Window → TextMeshPro → Font Asset Creator

3. Source Font: malgun
   Character Set: Custom Characters
   
   Custom Character List:
   0123456789점수최고블록레벨콤보게임오버다시시작

4. Generate → Save as "Korean_SDF"

5. 모든 TMP에 적용
```

### 더 많은 글자가 필요하면

```
Character Set: Unicode Range (Hex)
Character Sequence (Hex): 
AC00-D7A3,0030-0039,0020-007E

(한글 전체 + 숫자 + 기본 영문/기호)

주의: Atlas Resolution을 4096으로 올려야 함
```

---

## 🔧 수정된 GameDebugger

NullReference를 수정한 버전을 만들어드렸습니다.
아래 내용으로 GameDebugger.cs를 교체하세요.

---

## ✅ 최종 확인

### Play 테스트 시
```
Console에서 확인:
✓ Main Camera 존재: True
✓ Canvas 존재: True
✓ EventSystem 존재: True
✓ 블록 1-5 생성 완료

화면에서 확인:
✓ 블록이 여러 개 보임
✓ 숫자가 보임 (2, 4 등)
✓ 블록을 클릭하면 반응함
```

### 빌드 테스트 시
```
✓ 블록이 다양한 색상으로 보임
✓ 숫자가 깨지지 않음
✓ 터치가 작동함
✓ 게임이 진행됨
```

---

## 💡 TIP

### 개발 중 임시 방법
```
한글 폰트 설정 전에 테스트하려면:
- UI 텍스트를 영어로 변경
- Score, High Score, Level 등
- 숫자는 기본 폰트로 표시됨
```

### 최종 빌드 전 필수
```
1. 한글 폰트 생성 (위 가이드대로)
2. 모든 TMP에 한글 폰트 적용
3. 실기기에서 테스트
4. 한글이 제대로 보이는지 확인
```

---

## 📞 다음 단계

1. **Main Camera 추가** (최우선!)
2. **GameDebugger 다시 실행**
3. **"Main Camera 존재: True" 확인**
4. **한글 폰트 생성** (시간 날 때)
5. **빌드 & 테스트**

Main Camera만 추가해도 터치가 작동할 겁니다! 🎯
