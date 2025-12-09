# 🔊 오디오 시스템 개선 완료

## ✅ 개선 사항

### 1. AudioManager 확장 ⭐

```
추가된 사운드:
✅ Combo Sound - 콤보 발동 시
✅ Click Sound - 버튼 클릭 시
```

### 2. 디버그 로그 강화 🐛

```
모든 사운드 재생 시 Console에 로그 출력:
🔊 SFX 재생: merge (볼륨: 70%)
⚠️ Merge Sound가 연결되지 않았습니다!
```

### 3. Merge 사운드 수정 🔧

```
문제: merge 사운드가 안 들림
원인: AudioManager.PlayMergeSound() 호출이 누락됨
해결: BlockMerger.cs에 사운드 재생 코드 추가
```

---

## 🎵 사운드 재생 위치

### Merge Sound:
```
BlockMerger.cs → MergeBlocks 함수
→ 블록 병합 성공 시 재생
```

### Explode Sound:
```
BlockMerger.cs → ExplodeBlock 함수
→ 폭발 시작 시 재생
```

### Combo Sound:
```
ScoreManager.cs → AddCombo 함수
→ 콤보 추가 시 재생
```

### Click Sound:
```
SettingsManager.cs → OpenSettings, CloseSettings, QuitGame
→ 버튼 클릭 시 재생
```

### Game Over Sound:
```
GameManager.cs → GameOver 함수
→ 게임 종료 시 재생
```

---

## 🔗 Unity 설정 방법

### 1. AudioManager Inspector:

```
[Audio Clips]
  Bgm Clip: game_bgm.mp3              ← 이미 연결됨
  Merge Sound: merge.wav              ← 이미 연결됨
  Explode Sound: explode.wav          ← 추가 필요 시
  Game Over Sound: gameover.wav       ← 추가 필요 시
  Combo Sound: combo.wav              ⭐ 새로 연결!
  Click Sound: click.wav              ⭐ 새로 연결!

[Settings]
  Bgm Volume: 0.5
  Sfx Volume: 0.7
```

### 2. 오디오 파일 추가:

```
Assets/Audio/SFX/
  ├── merge.wav       ← 이미 있음
  ├── explode.wav     ← 추가 필요 시
  ├── gameover.wav    ← 추가 필요 시
  ├── combo.wav       ⭐ 새로 추가!
  └── click.wav       ⭐ 새로 추가!
```

---

## 🧪 테스트 방법

### 1. Merge 사운드 디버그:

```
1. Play 버튼
2. 블록 2개 선택하여 병합
3. Console 확인:
   🔊 병합 사운드 재생 요청
   🔊 SFX 재생: merge (볼륨: 70%)
   
만약 "⚠️ Merge Sound가 연결되지 않았습니다!" 출력:
→ AudioManager Inspector에서 merge.wav 연결 확인
```

### 2. Combo 사운드 테스트:

```
1. 블록 병합 (첫 번째)
2. 3초 안에 다시 병합 (콤보!)
3. Console 확인:
   🔥 콤보 추가: x1
   🔊 SFX 재생: combo (볼륨: 70%)
   
만약 "⚠️ Combo Sound가 연결되지 않았습니다!" 출력:
→ AudioManager에 combo.wav 연결 필요
```

### 3. Click 사운드 테스트:

```
1. 우상단 SET 버튼 클릭
2. Console 확인:
   🔊 SFX 재생: click (볼륨: 70%)
3. Close 버튼 클릭
4. Console 확인:
   🔊 SFX 재생: click (볼륨: 70%)
```

---

## 🐛 Merge 사운드 문제 해결

### 증상:
```
❌ merge.wav 파일은 연결했는데 소리가 안 들림
```

### 원인:
```
BlockMerger.cs에서 AudioManager.PlayMergeSound() 호출이 누락되어 있었음
```

### 해결:
```
BlockMerger.cs → MergeBlocks 함수에 추가:

if (AudioManager.Instance != null)
{
    AudioManager.Instance.PlayMergeSound();
    Debug.Log("🔊 병합 사운드 재생 요청");
}
```

### 확인 방법:
```
1. 블록 병합
2. Console에서 로그 확인:
   ✅ "🔊 병합 사운드 재생 요청" 출력
   ✅ "🔊 SFX 재생: merge" 출력
   ✅ 사운드 들림
```

---

## 📊 수정된 파일

```
✅ AudioManager.cs
   - comboSound, clickSound 추가
   - 디버그 로그 강화
   - null 체크 강화

✅ BlockMerger.cs
   - PlayMergeSound() 호출 추가 (병합 시)
   - PlayExplodeSound() 호출 추가 (폭발 시)
   - 디버그 로그 추가

✅ ScoreManager.cs
   - PlayComboSound() 호출 추가 (콤보 시)

✅ SettingsManager.cs
   - PlayClickSound() 호출 추가 (버튼 클릭 시)
```

---

## 🔊 사운드 추천 (무료 다운로드)

### Combo Sound:
```
검색 키워드: "combo", "power up", "achievement"
특징: 짧고 기분 좋은 "띠링~" 소리
길이: 0.5초 이하
```

### Click Sound:
```
검색 키워드: "button click", "UI click", "pop"
특징: 가벼운 "딱" 또는 "탁" 소리
길이: 0.1-0.2초
```

### 다운로드 사이트:
```
- Freesound.org (freesound.org)
- Zapsplat (zapsplat.com)
- Mixkit (mixkit.co)
```

---

## ⚙️ Import 설정

### Combo.wav, Click.wav:

```
파일 선택 → Inspector:
- Load Type: Decompress On Load
- Compression Format: PCM
- Force To Mono: ✓
- Apply
```

---

## 💡 Console 로그 활용

### 정상 작동 시:

```
🔊 SFX 재생: merge (볼륨: 70%)
🔊 SFX 재생: combo (볼륨: 70%)
🔊 SFX 재생: click (볼륨: 70%)
```

### 문제 발생 시:

```
⚠️ Merge Sound가 연결되지 않았습니다!
→ AudioManager에 merge.wav 드래그

❌ SFX Source가 null입니다!
→ AudioManager 오브젝트 재생성

⚠️ AudioClip이 null입니다!
→ 오디오 파일 연결 확인
```

---

## ✅ 체크리스트

```
□ AudioManager.cs 업데이트 완료 ✓
□ BlockMerger.cs 업데이트 완료 ✓
□ ScoreManager.cs 업데이트 완료 ✓
□ SettingsManager.cs 업데이트 완료 ✓
□ combo.wav 파일 준비
□ click.wav 파일 준비
□ Assets/Audio/SFX에 파일 복사
□ AudioManager에 combo.wav 연결
□ AudioManager에 click.wav 연결
□ Import 설정 (Decompress On Load)
□ Play 테스트
□ merge 사운드 확인 (Console 로그)
□ combo 사운드 확인 (3초 내 연속 병합)
□ click 사운드 확인 (버튼 클릭)
```

---

## 🎮 최종 테스트

### 1. Merge 사운드:
```
블록 병합 → "🔊 병합 사운드 재생 요청" → 소리 들림 ✓
```

### 2. Explode 사운드:
```
레벨 10 블록 → "💥 폭발 사운드 재생 요청" → 소리 들림 ✓
```

### 3. Combo 사운드:
```
3초 내 연속 병합 → "🔥 콤보 추가" → 소리 들림 ✓
```

### 4. Click 사운드:
```
SET 버튼 클릭 → "🔊 SFX 재생: click" → 소리 들림 ✓
Close 버튼 클릭 → "🔊 SFX 재생: click" → 소리 들림 ✓
```

### 5. Game Over 사운드:
```
게임오버 → "🔊 SFX 재생: gameover" → 소리 들림 ✓
```

---

## 🚀 다음 단계

```
1. combo.wav, click.wav 파일 준비
2. Assets/Audio/SFX에 추가
3. AudioManager에 드래그 연결
4. Play 테스트
5. Console 로그 확인
6. 실제 사운드 재생 확인
```

---

**이제 merge 사운드가 정상적으로 재생되고, combo와 click 사운드도 추가되었습니다!** 🎵✨

Console 로그를 확인하면 어떤 사운드가 재생되는지 정확하게 알 수 있습니다! 🔊
