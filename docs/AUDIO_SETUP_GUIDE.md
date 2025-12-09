# 🎵 오디오 적용 가이드

## 📁 오디오 파일 준비 및 적용

---

## 1️⃣ 디렉토리 구조

### 권장 구조 (PROJECT_STRUCTURE.md 기반):

```
BlockMergePuzzle/
└── Assets/
    └── Audio/                      ← 이 폴더 생성
        ├── Music/                  ← 배경음악
        │   ├── menu_bgm.mp3        ← 메뉴 음악 (선택 사항)
        │   └── game_bgm.mp3        ← 게임 음악
        │
        └── SFX/                    ← 효과음
            ├── merge.wav           ← 블록 병합 소리
            ├── explode.wav         ← 폭발 소리
            ├── click.wav           ← 클릭 소리 (선택 사항)
            ├── combo.wav           ← 콤보 소리 (선택 사항)
            ├── achievement.wav     ← 업적 소리 (선택 사항)
            └── gameover.wav        ← 게임오버 소리
```

---

## 2️⃣ 오디오 파일 준비

### 필수 파일 (최소):

```
✅ game_bgm.mp3 - 게임 중 배경음악
✅ merge.wav - 블록 병합 시
✅ explode.wav - 폭발 시
✅ gameover.wav - 게임오버 시
```

### 권장 파일 형식:

| 용도 | 형식 | 이유 |
|------|------|------|
| 배경음악 (BGM) | .mp3 | 파일 크기 작음, 품질 좋음 |
| 효과음 (SFX) | .wav | 지연 없음, 빠른 재생 |

### 품질 권장:

```
배경음악 (BGM):
- 형식: MP3
- 비트레이트: 128-192 kbps
- 샘플레이트: 44.1 kHz
- 길이: 1-3분 (루프 가능)

효과음 (SFX):
- 형식: WAV
- 비트레이트: 16-bit
- 샘플레이트: 44.1 kHz
- 길이: 0.1-1초 (짧게!)
```

---

## 3️⃣ Unity에 파일 추가

### 방법 1: 폴더 생성 후 드래그

```
1. Unity Project 창에서:
   Assets → 우클릭 → Create → Folder
   이름: Audio

2. Audio 폴더 안에서:
   우클릭 → Create → Folder
   이름: Music
   우클릭 → Create → Folder
   이름: SFX

3. 준비한 오디오 파일을 해당 폴더에 드래그
   Music 폴더에: game_bgm.mp3
   SFX 폴더에: merge.wav, explode.wav, gameover.wav
```

### 방법 2: Windows 탐색기에서 직접 복사

```
1. Windows 탐색기 열기
2. 경로: E:\claude_src\BlockMergePuzzle\Assets
3. Audio 폴더 생성
4. Music, SFX 폴더 생성
5. 오디오 파일 복사/붙여넣기
6. Unity로 돌아가면 자동으로 Import됨
```

---

## 4️⃣ Unity에서 AudioManager 설정

### 단계 1: AudioManager 오브젝트 생성

```
Hierarchy → 우클릭 → Create Empty
이름: AudioManager
```

### 단계 2: AudioManager 스크립트 추가

```
AudioManager 선택
→ Inspector → Add Component
→ AudioManager
```

### 단계 3: 오디오 파일 연결

```
AudioManager 선택
→ Inspector:

[Audio Clips]
  Bgm Clip: 없음 (Audio Clip)      ← 여기에 game_bgm.mp3 드래그
  Merge Sound: 없음 (Audio Clip)    ← merge.wav
  Explode Sound: 없음 (Audio Clip)  ← explode.wav
  Game Over Sound: 없음 (Audio Clip) ← gameover.wav

[Settings]
  Bgm Volume: 0.5
  Sfx Volume: 0.7
```

**드래그 방법:**
```
Project 창에서:
Assets/Audio/Music/game_bgm.mp3
→ Inspector의 "Bgm Clip"으로 드래그

Assets/Audio/SFX/merge.wav
→ Inspector의 "Merge Sound"로 드래그

... 반복
```

---

## 5️⃣ 오디오 Import 설정 (중요!)

### BGM 설정:

```
Project 창에서 game_bgm.mp3 선택
→ Inspector:

[Load Type]
  ✓ Streaming           ← 메모리 절약

[Compression Format]
  - Vorbis (Android)    ← 모바일에 최적
  - Quality: 70%

[Force To Mono]
  ✓ 체크 (선택 사항)    ← 파일 크기 50% 감소

[Apply] 버튼 클릭
```

### SFX 설정:

```
Project 창에서 merge.wav, explode.wav 등 선택
→ Inspector:

[Load Type]
  ✓ Decompress On Load  ← 빠른 재생

[Compression Format]
  - PCM (최고 품질)
  또는
  - ADPCM (용량 절약)

[Force To Mono]
  ✓ 체크

[Apply] 버튼 클릭
```

---

## 6️⃣ 코드에서 사운드 재생

### AudioManager가 자동으로 처리합니다!

**이미 구현된 위치:**

```csharp
// BlockMerger.cs - 병합 시
if (AudioManager.Instance != null)
{
    AudioManager.Instance.PlayMergeSound();
}

// BlockMerger.cs - 폭발 시
if (AudioManager.Instance != null)
{
    AudioManager.Instance.PlayExplodeSound();
}

// GameManager.cs - 게임오버 시
if (AudioManager.Instance != null)
{
    AudioManager.Instance.PlayGameOverSound();
}
```

**추가 작업 필요 없음!** ✓

---

## 7️⃣ 테스트

### Unity Editor:

```
1. Play 버튼
2. 자동으로 BGM 재생 확인
3. 블록 병합 → merge.wav 재생 확인
4. 폭발 → explode.wav 재생 확인
5. 게임오버 → gameover.wav 재생 확인
```

### 설정 패널에서 음량 조절:

```
1. 우상단 SET 버튼 클릭
2. BGM Volume 슬라이더 조작 → 음악 볼륨 변경
3. SFX Volume 슬라이더 조작 → 효과음 볼륨 변경
4. 설정 자동 저장됨
```

---

## 🎵 무료 오디오 리소스 사이트

### 배경음악 (BGM):

1. **Incompetech** (https://incompetech.com)
   - 무료 음악
   - 상업적 사용 가능 (출처 표기)
   - 장르: 게임 음악 다수

2. **Free Music Archive** (https://freemusicarchive.org)
   - 다양한 장르
   - 무료 라이선스

3. **Bensound** (https://www.bensound.com)
   - 게임에 적합한 음악
   - 무료 사용 (출처 표기)

### 효과음 (SFX):

1. **Freesound** (https://freesound.org)
   - 방대한 효과음 라이브러리
   - 검색: "merge", "explosion", "game over"

2. **Zapsplat** (https://www.zapsplat.com)
   - 게임 효과음 전문
   - 무료 다운로드

3. **Mixkit** (https://mixkit.co/free-sound-effects/)
   - 고품질 효과음
   - 상업적 사용 가능

---

## 🔊 권장 오디오 스타일

### 블록 퍼즐 게임에 어울리는 스타일:

```
BGM:
- 밝고 경쾌한 분위기
- 반복적이지만 지루하지 않음
- 집중에 방해되지 않는 중간 템포
- 예: "Quirky" "Upbeat" "Casual Game" 키워드

효과음:
- merge.wav: 가볍고 기분 좋은 "딸랑" 소리
- explode.wav: 짧고 강렬한 "뻥" 소리
- gameover.wav: 실망스럽지만 너무 무겁지 않은 "띠링~"
```

---

## 🎯 최소 구성 (빠른 테스트용)

### 오디오 없이도 작동합니다!

```
AudioManager는 오디오 파일이 없어도 에러 없이 작동합니다.
나중에 파일 추가 → Inspector에서 연결 → 즉시 적용!
```

### 임시 테스트 방법:

```
1. 아무 소리 파일 3개 준비 (MP3, WAV)
2. 이름 변경:
   - game_bgm.mp3
   - merge.wav
   - explode.wav
3. Assets/Audio에 추가
4. AudioManager에 연결
5. 테스트!
```

---

## 📊 파일 크기 가이드

### 모바일 앱 용량 최적화:

```
BGM (2분 길이):
- 128kbps MP3: ~2MB
- 192kbps MP3: ~3MB

SFX (0.5초 평균):
- WAV: ~50KB
- MP3: ~10KB (권장 안 함, 지연 발생)

총 오디오 용량 목표:
- 5-10MB (적당)
- 20MB 이하 (권장)
```

---

## ✅ 체크리스트

```
□ Assets/Audio 폴더 생성
□ Music 폴더 생성
□ SFX 폴더 생성
□ game_bgm.mp3 준비 및 복사
□ merge.wav 준비 및 복사
□ explode.wav 준비 및 복사
□ gameover.wav 준비 및 복사 (선택)
□ Unity에서 Import 확인
□ AudioManager 오브젝트 생성
□ AudioManager 스크립트 추가
□ Bgm Clip 연결
□ Merge Sound 연결
□ Explode Sound 연결
□ Game Over Sound 연결
□ BGM Import 설정 (Streaming)
□ SFX Import 설정 (Decompress On Load)
□ Play 테스트
□ 음량 조절 테스트
□ 모바일 빌드 & 테스트
```

---

## 🆘 문제 해결

### 소리가 안 나면:

```
1. AudioManager가 Hierarchy에 있는지 확인
2. Inspector에서 오디오 클립 연결 확인
3. 볼륨이 0이 아닌지 확인
4. Unity Editor 볼륨 확인 (우하단 스피커 아이콘)
5. Project 창에서 오디오 파일 선택 → Play 버튼으로 직접 재생 테스트
```

### 오디오가 너무 크거나 작으면:

```
설정 패널 (SET 버튼) → 슬라이더 조절
또는
AudioManager 선택 → Inspector:
- Bgm Volume: 0.5 조절
- Sfx Volume: 0.7 조절
```

### 오디오 파일이 Import 안 되면:

```
1. 파일 형식 확인 (MP3, WAV, OGG만 지원)
2. Unity 재시작
3. Assets 폴더 우클릭 → Reimport All
```

---

## 💡 추가 팁

### 효과음 레이어링:

```
merge.wav + click.wav 동시 재생
→ 더 풍부한 사운드!
```

### 음량 밸런스:

```
BGM: 0.3-0.5 (배경음)
SFX: 0.7-1.0 (명확하게)
```

### 루프 설정 (BGM):

```
game_bgm.mp3 선택
→ Inspector
→ Loop: ✓ 체크
```

---

## 🎮 최종 확인

### Editor:
```
1. Play
2. BGM 자동 재생 확인
3. 블록 병합 → merge.wav
4. 폭발 → explode.wav
5. 설정 패널 → 음량 조절 작동 확인
```

### Mobile:
```
1. Build And Run
2. 실기기에서 소리 테스트
3. 이어폰/스피커 둘 다 테스트
4. 음량 조절 저장 확인
```

---

**이제 준비한 오디오 파일을 Assets/Audio 폴더에 넣고 AudioManager에 연결하면 끝!** 🎵✨

파일 준비 → 폴더에 복사 → Unity에서 드래그 → Play → 사운드 확인! 🎮
