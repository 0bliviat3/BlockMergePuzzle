# 🎮 새로 추가된 스크립트 파일들

## 📁 파일 위치

모든 파일이 올바른 위치에 생성되어 있습니다:

```
Assets/Scripts/
  ├── Shared/
  │   └── SceneLoader.cs          ✅ 생성됨
  │
  ├── MainMenu/
  │   └── MainMenuManager.cs      ✅ 생성됨
  │
  └── Classic2048/
      ├── Classic2048Manager.cs   ✅ 생성됨
      ├── Classic2048Grid.cs      ✅ 생성됨
      ├── Classic2048Tile.cs      ✅ 생성됨
      └── Classic2048Input.cs     ✅ 생성됨
```

---

## 🔧 Unity에서 파일이 안 보일 때

### 방법 1: Assets Refresh (권장)
```
Unity Editor:
1. Project 창에서 Assets 폴더 선택
2. 우클릭 → Reimport All
또는
3. Ctrl+R (Refresh)
```

### 방법 2: Unity 재시작
```
1. Unity Editor 완전 종료
2. Unity 다시 실행
3. Project 창에서 Scripts 폴더 확인
```

### 방법 3: 폴더 새로고침
```
1. Project 창에서 Scripts 폴더 우클릭
2. Reimport
```

---

## 📂 Windows 탐색기에서 확인

```
경로: E:\claude_src\BlockMergePuzzle\Assets\Scripts

확인 사항:
✅ Shared 폴더 있음
✅ MainMenu 폴더 있음  
✅ Classic2048 폴더 있음
✅ 각 폴더 안에 .cs 파일 있음
```

---

## 🐛 여전히 안 보이면?

파일을 수동으로 확인:

```
Windows 탐색기:
1. E:\claude_src\BlockMergePuzzle\Assets\Scripts 열기
2. Shared, MainMenu, Classic2048 폴더 확인
3. 폴더 안에 .cs 파일 있는지 확인
```

만약 파일이 없다면 알려주세요!
제가 다른 방법으로 생성하겠습니다.

---

## ✅ 파일이 보이면

다음 단계로 진행:
1. MainMenu 씬 생성
2. Classic2048 씬 생성
3. Build Settings 설정
4. 테스트!
