# 🚀 Classic 2048 빠른 시작 (5분)

## 1️⃣ MainMenu 씬 생성 (1분)

```
Unity:
1. File → New Scene
2. Ctrl+S → "MainMenu" 입력
3. Assets/Scenes/MainMenu.unity 저장
```

**씬 구성:**
```
Hierarchy:
1. 빈 오브젝트 생성: MainMenuManager
2. Add Component → MainMenuManager
3. Auto Create UI: ✓ 체크
4. Play 테스트!
```

---

## 2️⃣ Classic2048 씬 생성 (1분)

```
Unity:
1. File → New Scene
2. Ctrl+S → "Classic2048" 입력
3. Assets/Scenes/Classic2048.unity 저장
```

**씬 구성:**
```
Hierarchy:
1. 빈 오브젝트 생성: Classic2048Manager
2. Add Component → Classic2048Manager
3. Auto Create UI: ✓ 체크
4. Starting Tiles: 2
5. Play 테스트!
```

---

## 3️⃣ SceneLoader 추가 (1분)

```
MainMenu 씬에서:
1. 빈 오브젝트 생성: SceneLoader
2. Add Component → SceneLoader
```

---

## 4️⃣ Build Settings (1분)

```
File → Build Settings:

Scenes In Build:
0. MainMenu              ← 드래그
1. BlockMergePuzzle      ← 이미 있음
2. Classic2048           ← 드래그

순서 중요! MainMenu가 0번!
```

---

## 5️⃣ 테스트 (1분)

```
1. MainMenu 씬 열기
2. Play 버튼
3. "Classic 2048" 버튼 클릭
4. W/A/S/D 키로 플레이!
5. ← MENU 버튼으로 돌아가기
```

---

## ✅ 완료!

```
이제 2개 게임이 있는 앱 완성!
✅ Block Merge Puzzle
✅ Classic 2048
```

---

## 🎮 조작법

### Classic 2048:
```
PC: W/A/S/D 또는 화살표 키
모바일: 스와이프
```

### 목표:
```
2048 타일 만들기!
2 + 2 = 4
4 + 4 = 8
...
1024 + 1024 = 2048 🎉
```

---

## 🐛 문제 해결

### "씬을 찾을 수 없습니다"
```
→ Build Settings에 씬 추가 확인
```

### 타일이 안 생김
```
→ Auto Create UI 체크 확인
→ Console 로그 확인
```

### 버튼이 안 보임
```
→ Unity Editor 재시작
→ Play 모드 종료 후 재시작
```

---

## 📖 상세 가이드

```
CLASSIC2048_SETUP_GUIDE.md 참고
```

**5분 완료!** 🎉
