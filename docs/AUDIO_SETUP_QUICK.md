# 🎵 빠른 오디오 적용 (3분)

## 📁 1단계: 폴더 구조 만들기

```
Assets/Audio/
  ├── Music/
  │   └── game_bgm.mp3      ← 배경음악
  └── SFX/
      ├── merge.wav         ← 병합 소리
      ├── explode.wav       ← 폭발 소리
      └── gameover.wav      ← 게임오버 소리
```

---

## 🎼 2단계: 오디오 파일 준비

### 최소 필수 파일:

```
✅ game_bgm.mp3 - 게임 음악
✅ merge.wav - 블록 병합
✅ explode.wav - 폭발
✅ gameover.wav - 게임오버
```

### 권장 형식:

```
BGM: MP3 (128-192kbps)
SFX: WAV (16-bit, 44.1kHz)
```

---

## 📥 3단계: Unity에 추가

### 방법 1: Unity 내에서

```
1. Project 창
2. Assets 우클릭 → Create → Folder → Audio
3. Audio 안에 Music, SFX 폴더 생성
4. 오디오 파일 드래그
```

### 방법 2: Windows 탐색기

```
1. E:\claude_src\BlockMergePuzzle\Assets
2. Audio 폴더 생성
3. Music, SFX 폴더 생성
4. 파일 복사/붙여넣기
5. Unity로 돌아가면 자동 Import
```

---

## 🔗 4단계: AudioManager 연결

```
1. Hierarchy → AudioManager 선택
2. Inspector:
   - Bgm Clip: game_bgm.mp3 드래그
   - Merge Sound: merge.wav 드래그
   - Explode Sound: explode.wav 드래그
   - Game Over Sound: gameover.wav 드래그
```

---

## ⚙️ 5단계: Import 설정

### BGM:

```
game_bgm.mp3 선택 → Inspector:
- Load Type: Streaming
- Compression: Vorbis
- Apply
```

### SFX:

```
merge.wav, explode.wav 선택 → Inspector:
- Load Type: Decompress On Load
- Compression: PCM
- Apply
```

---

## 🧪 6단계: 테스트

```
1. Play 버튼
2. BGM 재생 확인
3. 블록 병합 → merge.wav
4. 폭발 → explode.wav
5. SET 버튼 → 음량 조절
```

---

## 🎵 무료 오디오 다운로드

### BGM:
- Incompetech (incompetech.com)
- Bensound (bensound.com)

### SFX:
- Freesound (freesound.org)
- Zapsplat (zapsplat.com)

---

## ✅ 완료!

```
파일 준비 → Assets/Audio에 복사 → AudioManager 연결 → Play!
```

**상세 가이드: AUDIO_SETUP_GUIDE.md 참고**
