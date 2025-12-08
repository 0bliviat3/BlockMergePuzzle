# 🚀 빠른 설정 가이드 (5분)

## 📱 블록 크기 증가 + 설정 패널 추가

---

## 1️⃣ Grid 수정 (이미 완료됨 ✓)

```
Grid.cs에서:
- cellSize: 100 → 140 (40% 증가)
- cellSpacing: 10 → 12

→ 블록이 커져서 터치하기 쉬워짐!
```

---

## 2️⃣ AudioManager 추가 (1분)

### Unity Editor에서:

```
1. Hierarchy → 우클릭 → Create Empty
2. 이름: AudioManager
3. Inspector → Add Component → AudioManager
```

**끝!** ✓

---

## 3️⃣ SettingsManager 추가 (1분)

### Unity Editor에서:

```
1. Hierarchy → 우클릭 → Create Empty
2. 이름: SettingsManager
3. Inspector → Add Component → SettingsManager
4. Auto Create UI: ✓ 체크 확인
```

**끝!** ✓

---

## 4️⃣ 테스트 (1분)

### Play 버튼 클릭:

```
1. 블록이 커진 것 확인 ✓
2. 우상단에 ⚙️ 버튼 생김 ✓
3. ⚙️ 클릭 → 설정 패널 열림 ✓
4. BGM/SFX 슬라이더 작동 확인 ✓
5. Close 버튼으로 닫기 ✓
6. Quit Game 버튼 테스트 ✓
```

---

## 📱 모바일 빌드 테스트

### Android:

```
1. File → Build Settings
2. Platform → Android
3. Switch Platform
4. Build And Run
5. 실기기에서 테스트:
   - 블록 크기 (더 큼) ✓
   - 터치 조작 (편해짐) ✓
   - ⚙️ 버튼 ✓
   - 음량 조절 ✓
   - 게임 종료 ✓
```

---

## 🎯 완료!

### 이제 게임에서:

```
✅ 블록이 40% 커짐 (터치 쉬움)
✅ 우상단 설정 버튼 (⚙️)
✅ 음량 조절 (BGM, SFX)
✅ 게임 종료 버튼
✅ 설정 자동 저장
```

---

## 🆘 문제 발생 시

### 설정 버튼이 안 보이면:

```
SettingsManager 선택
→ Inspector
→ Auto Create UI: ✓ 체크
→ Play 다시 시작
```

### 음량 조절이 안되면:

```
AudioManager가 Hierarchy에 있는지 확인
(없으면 다시 생성)
```

---

## 💡 추가 조정

### 블록 더 크게:

```
Grid 선택
→ Inspector
→ Cell Size: 140 → 150 또는 160
```

### 블록 더 작게:

```
Grid 선택
→ Inspector
→ Cell Size: 140 → 130 또는 120
```

---

그게 전부입니다! 🎮

5분 안에 모바일 UX가 크게 개선됩니다! 📱✨
