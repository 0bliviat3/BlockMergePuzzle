# 🎮 2048 종합 게임 앱 - 설정 가이드

## ✅ 완료된 작업

### 새로 추가된 스크립트:

```
Assets/Scripts/
  ├── Shared/
  │   └── SceneLoader.cs          ⭐ 씬 전환
  │
  ├── MainMenu/
  │   └── MainMenuManager.cs      ⭐ 메인 메뉴
  │
  └── Classic2048/
      ├── Classic2048Manager.cs   ⭐ 게임 관리
      ├── Classic2048Grid.cs      ⭐ 그리드 관리
      ├── Classic2048Tile.cs      ⭐ 타일 클래스
      └── Classic2048Input.cs     ⭐ 스와이프 입력
```

### 기존 게임:
```
✅ Block Merge Puzzle - 기존 게임 (그대로 유지)
```

---

## 🚀 Unity 설정 순서

### 1️⃣ MainMenu 씬 생성

```
Unity:
1. File → New Scene
2. 이름: MainMenu
3. File → Save Scene As → Assets/Scenes/MainMenu.unity
```

**MainMenu 씬 구성:**

```
1. Hierarchy에서:
   - Create Empty → MainMenuManager
   - Add Component → MainMenuManager
   - Auto Create UI: ✓ 체크

2. AudioManager & SettingsManager:
   - 기존 AudioManager, SettingsManager 오브젝트를
     MainMenu 씬에도 배치 (DontDestroyOnLoad이므로 자동 유지)

3. SceneLoader 오브젝트 생성:
   - Create Empty → SceneLoader
   - Add Component → SceneLoader
```

---

### 2️⃣ Classic2048 씬 생성

```
Unity:
1. File → New Scene
2. 이름: Classic2048
3. File → Save Scene As → Assets/Scenes/Classic2048.unity
```

**Classic2048 씬 구성:**

```
1. Hierarchy에서:
   - Create Empty → Classic2048Manager
   - Add Component → Classic2048Manager
   - Auto Create UI: ✓ 체크
   - Starting Tiles: 2

2. Inspector:
   - Game BGM: (나중에 추가 가능)
```

---

### 3️⃣ BlockMergePuzzle 씬 수정 (뒤로가기 버튼 추가)

**BlockMergePuzzle 씬에 Back 버튼 추가:**

현재 씬에는 Settings만 있고 메뉴로 돌아가는 버튼이 없으므로 추가 필요!

```
방법 1: 수동으로 버튼 추가
1. BlockMergePuzzle 씬 열기
2. Hierarchy → Canvas 우클릭 → UI → Button
3. 이름: BackButton
4. Inspector:
   - Position: 좌상단 (X: 100, Y: -50)
   - Size: 150 x 80
   - Text: "← MENU"
5. On Click() 이벤트:
   - SceneLoader.LoadScene("MainMenu")

방법 2: 코드로 자동 생성 (권장)
→ GameManager.cs에 Back 버튼 자동 생성 코드 추가
```

---

### 4️⃣ Build Settings 설정

```
Unity:
1. File → Build Settings
2. Scenes In Build:
   0. MainMenu              ⭐ 드래그
   1. BlockMergePuzzle      (기존)
   2. Classic2048           ⭐ 드래그

3. Player Settings:
   - Product Name: "2048 Collection"
   - Company Name: (당신의 이름)
   - Default Icon: (아이콘 이미지)
```

**씬 순서가 중요합니다!**
- MainMenu가 첫 번째(0번)여야 앱 시작 시 메뉴가 표시됩니다!

---

## 🎮 테스트 순서

### 1단계: MainMenu 씬 테스트

```
1. MainMenu 씬 열기
2. Play 버튼
3. 확인:
   ✅ "2048 COLLECTION" 타이틀 표시
   ✅ "Block Merge Puzzle" 버튼 표시
   ✅ "Classic 2048" 버튼 표시
   ✅ "SETTINGS" 버튼 표시
   ✅ BGM 재생 (있다면)
```

### 2단계: Classic2048 씬 테스트

```
1. Classic2048 씬 열기
2. Play 버튼
3. 확인:
   ✅ "2048" 타이틀 표시
   ✅ SCORE, BEST 박스 표시
   ✅ 4x4 그리드 생성
   ✅ 타일 2개 자동 생성
   ✅ "← MENU" 버튼 표시

4. 조작:
   ✅ 스와이프 (또는 키보드 W/A/S/D)
   ✅ 타일 이동 및 병합
   ✅ 병합 시 merge.wav 재생
   ✅ 점수 증가
   ✅ 새 타일 자동 생성
```

### 3단계: 씬 전환 테스트

```
1. MainMenu 씬에서 Play
2. "Block Merge Puzzle" 버튼 클릭
   → BlockMergePuzzle 씬 로드
3. Back 버튼 클릭
   → MainMenu 씬 복귀
4. "Classic 2048" 버튼 클릭
   → Classic2048 씬 로드
5. ← MENU 버튼 클릭
   → MainMenu 씬 복귀
```

---

## 🐛 예상 문제 & 해결

### 문제 1: "씬을 찾을 수 없습니다"

```
증상:
Scene 'MainMenu' couldn't be loaded because it has not been added to the build settings

해결:
File → Build Settings → Add Open Scenes
또는
씬 파일을 직접 드래그
```

### 문제 2: 타일이 생성 안 됨

```
증상:
4x4 그리드는 보이지만 타일이 안 생김

해결:
Classic2048Manager 선택 → Inspector:
- Auto Create UI: ✓ 확인
- Starting Tiles: 2 확인

Console 확인:
"✓ 타일 생성: 위치 (x,y), 값 2" 로그 확인
```

### 문제 3: 스와이프 안 됨

```
증상:
그리드를 스와이프해도 타일이 안 움직임

해결:
1. Classic2048Input 컴포넌트 확인
2. Console에서 "👆 스와이프: 위" 로그 확인
3. 키보드 W/A/S/D로 테스트
```

### 문제 4: 뒤로가기 버튼 클릭해도 안 됨

```
증상:
← MENU 버튼 클릭해도 MainMenu로 안 감

해결:
1. SceneLoader 오브젝트가 MainMenu에 있는지 확인
2. Build Settings에서 씬 순서 확인
3. Console에서 에러 확인
```

---

## 🎨 UI 커스터마이징

### 색상 변경:

```csharp
// MainMenuManager.cs
bgImage.color = new Color(0.17f, 0.24f, 0.31f); // 배경색 변경

// Classic2048Manager.cs
bgImage.color = new Color(0.98f, 0.97f, 0.94f); // 배경색 변경
```

### 그리드 크기 변경:

```csharp
// Classic2048Manager.cs → CreateGrid()
grid.cellSize = 140f;      // 타일 크기
grid.cellSpacing = 15f;    // 타일 간격
```

### 타이틀 변경:

```csharp
// MainMenuManager.cs
titleText.text = "MY 2048 GAMES"; // 타이틀 변경

// Classic2048Manager.cs
titleText.text = "CLASSIC 2048"; // 타이틀 변경
```

---

## 🔊 오디오 추가 (선택 사항)

### MainMenu BGM:

```
1. Assets/Audio/Music/menu_bgm.mp3 추가
2. MainMenu 씬에서:
   - MainMenuManager 선택
   - Menu BGM: menu_bgm.mp3 드래그
```

### Classic2048 BGM:

```
1. Assets/Audio/Music/classic2048_bgm.mp3 추가
2. Classic2048 씬에서:
   - Classic2048Manager 선택
   - Game BGM: classic2048_bgm.mp3 드래그
```

### 스와이프 사운드 추가 (선택):

```
1. Assets/Audio/SFX/slide.wav 추가
2. AudioManager에 추가:
   - public AudioClip slideSound;
   - public void PlaySlideSound() { PlaySFX(slideSound); }
3. Classic2048Manager → ProcessMove()에서 호출
```

---

## 📱 모바일 빌드

### Android:

```
1. File → Build Settings
2. Platform: Android 선택
3. Switch Platform
4. Build And Run
```

### 테스트:

```
✅ 메인 메뉴에서 게임 선택
✅ 스와이프로 타일 이동
✅ 뒤로가기 버튼으로 메뉴 복귀
✅ Settings 패널 열기/닫기
✅ 음량 조절
```

---

## 🎯 다음 단계

### 현재 완료:
```
✅ MainMenu 씬 (게임 선택)
✅ Classic2048 게임 (4x4 오리지널 2048)
✅ BlockMergePuzzle 게임 (기존)
✅ 씬 전환 시스템
✅ 공통 오디오 관리
✅ 공통 설정 관리
```

### 앞으로 추가 가능:
```
⬜ 2048 x 2 (2개 그리드 동시 플레이)
⬜ 2048 5x5 (큰 그리드)
⬜ 2048 Hexagon (육각형 그리드)
⬜ Time Attack 모드
⬜ Challenge 모드
⬜ 리더보드
⬜ 업적 시스템
```

---

## ✅ 최종 체크리스트

### 씬 파일:
```
□ MainMenu.unity 생성 ✓
□ Classic2048.unity 생성 ✓
□ BlockMergePuzzle.unity 확인 ✓
```

### 스크립트:
```
□ SceneLoader.cs 생성 ✓
□ MainMenuManager.cs 생성 ✓
□ Classic2048Manager.cs 생성 ✓
□ Classic2048Grid.cs 생성 ✓
□ Classic2048Tile.cs 생성 ✓
□ Classic2048Input.cs 생성 ✓
```

### Unity 설정:
```
□ MainMenu 씬에 MainMenuManager 배치
□ Classic2048 씬에 Classic2048Manager 배치
□ Build Settings에 씬 추가 (순서: 0, 1, 2)
□ AudioManager, SettingsManager DontDestroyOnLoad 확인
□ SceneLoader 배치
```

### 테스트:
```
□ MainMenu → BlockMergePuzzle 전환
□ MainMenu → Classic2048 전환
□ 각 게임에서 메뉴로 복귀
□ Classic2048 스와이프 조작
□ Classic2048 타일 병합
□ Classic2048 2048 달성 (테스트)
□ Classic2048 게임오버
□ Settings 패널 작동
□ 음량 조절
```

---

## 🎮 빠른 시작

```
1. Unity에서 MainMenu.unity 열기
2. Play 버튼
3. "Classic 2048" 클릭
4. W/A/S/D 또는 스와이프로 플레이!
```

**축하합니다! 2048 종합 게임 앱이 완성되었습니다!** 🎉✨
