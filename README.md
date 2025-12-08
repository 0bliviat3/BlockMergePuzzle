# Block Merge Puzzle - Unity 모바일 게임

## 📱 게임 개요
2048 스타일의 블록 병합 퍼즐 게임으로, 연쇄 반응과 폭발 메커니즘이 추가되어 더욱 전략적이고 중독성 있는 게임플레이를 제공합니다.

## 🎮 게임 특징

### 핵심 메커니즘
1. **블록 병합**: 같은 레벨의 인접한 블록 2개를 터치하여 병합
2. **레벨 업**: 병합 시 한 단계 높은 블록으로 진화 (2 → 4 → 8 → 16 → ...)
3. **폭발 시스템**: 레벨 10 이상 블록 생성 시 폭발하여 주변 블록에 영향
4. **콤보 시스템**: 연속 병합으로 점수 배율 증가
5. **전략적 플레이**: 그리드 관리와 블록 배치가 중요

### 게임 목표
- 🏆 최고 점수 달성
- 💎 최대 레벨 블록 생성
- 🔥 연쇄 폭발로 보너스 점수 획득
- ⏱️ 그리드가 가득 차기 전까지 최대한 생존

## 🛠️ 기술 스택
- **Unity 2021.3 LTS** (이상 권장)
- **C# 스크립트**
- **TextMeshPro** (UI)
- **LeanTween** (애니메이션)
- **2D Physics**

## 📦 프로젝트 구조

```
BlockMergePuzzle/
├── Assets/
│   ├── Scripts/
│   │   ├── Block.cs              # 개별 블록 클래스
│   │   ├── Grid.cs               # 그리드 관리
│   │   ├── BlockMerger.cs        # 병합 로직
│   │   ├── GameManager.cs        # 게임 전체 관리
│   │   ├── ScoreManager.cs       # 점수 및 콤보 관리
│   │   ├── InputHandler.cs       # 터치/입력 처리
│   │   └── EffectManager.cs      # 이펙트 관리
│   ├── Scenes/
│   ├── Prefabs/
│   ├── Materials/
│   ├── Sprites/
│   └── Audio/
└── README.md
```

## 🎯 주요 클래스 설명

### Block.cs
- 개별 블록의 데이터와 동작 관리
- 레벨, 색상, 애니메이션 처리
- 병합, 폭발, 이동 애니메이션

### Grid.cs
- 5x5 그리드 관리
- 블록 추가/제거/이동
- 빈 공간 및 인접 블록 검색

### BlockMerger.cs
- 블록 선택 및 병합 로직
- 폭발 메커니즘 (레벨 10 이상)
- 주변 블록 영향 처리
- 연쇄 병합 감지

### GameManager.cs
- 게임 흐름 제어
- 게임 시작/종료/재시작
- 게임 오버 조건 체크
- 최고 블록 레벨 추적

### ScoreManager.cs
- 점수 계산 및 표시
- 콤보 시스템 관리
- 최고 점수 저장/로드

### InputHandler.cs
- 터치/마우스 입력 처리
- 블록 선택 감지
- UI 이벤트 필터링

### EffectManager.cs
- 병합/폭발 이펙트
- 사운드 재생
- 화면 효과 (흔들림, 플래시)

## 🚀 Unity 설정 가이드

### 1. 프로젝트 생성
1. Unity Hub에서 "New Project" 클릭
2. Template: **2D (URP)** 선택
3. Project Name: `BlockMergePuzzle`
4. Location: 이 폴더의 상위 디렉토리 선택

### 2. 필수 패키지 설치
**Package Manager**에서 다음 패키지 설치:
- TextMeshPro (필수)
- 2D Sprite (포함됨)
- Unity UI (포함됨)

### 3. LeanTween 설치
1. [LeanTween GitHub](https://github.com/dentedpixel/LeanTween) 방문
2. `LeanTween.cs` 다운로드
3. `Assets/Scripts/` 폴더에 추가

### 4. 프리팹 생성

#### Block Prefab
1. UI → Image 생성
2. TextMeshPro Text 자식으로 추가
3. BoxCollider2D 컴포넌트 추가
4. Block.cs 스크립트 추가
5. Prefab으로 저장

#### Cell Prefab
1. UI → Image 생성
2. 배경 색상: 회색 반투명
3. Prefab으로 저장

### 5. 씬 구성

```
Canvas (Screen Space - Overlay)
├── GridContainer (Empty GameObject)
├── BlocksContainer (Empty GameObject)
├── UI
│   ├── ScoreText (TextMeshPro)
│   ├── HighScoreText (TextMeshPro)
│   ├── HighestBlockText (TextMeshPro)
│   ├── ComboPanel
│   │   └── ComboText (TextMeshPro)
│   └── GameOverPanel
│       ├── GameOverText (TextMeshPro)
│       └── RestartButton (Button)
└── GameManager (Empty GameObject)
    ├── Grid Component
    ├── BlockMerger Component
    ├── GameManager Component
    ├── ScoreManager Component
    ├── EffectManager Component
    └── InputHandler Component
```

### 6. Layer 설정
1. Edit → Project Settings → Tags and Layers
2. Layer 추가: `BlockLayer`
3. Block 프리팹의 Layer를 `BlockLayer`로 설정

### 7. 빌드 설정 (Android)

#### Player Settings
```
Company Name: YourCompany
Product Name: Block Merge Puzzle
Package Name: com.yourcompany.blockmerge
Version: 1.0.0
```

#### Resolution and Presentation
```
Default Orientation: Portrait
Allowed Orientations: Portrait만 체크
```

#### Other Settings
```
Scripting Backend: IL2CPP
Target Architectures: ARM64 체크
Minimum API Level: Android 5.0 (API level 21)
Target API Level: Automatic (highest installed)
```

### 8. 최적화 설정

#### Quality Settings
```
Anti Aliasing: 2x Multi Sampling
VSync Count: Don't Sync
```

#### Graphics Settings
```
Use URP Asset
Enable SRP Batcher
```

## 🎨 커스터마이징 가이드

### 블록 색상 변경
`Block.cs`의 `levelColors` 배열 수정:
```csharp
private static readonly Color[] levelColors = new Color[]
{
    new Color(0.93f, 0.89f, 0.85f), // 레벨 1 색상
    // ... 원하는 색상으로 변경
};
```

### 그리드 크기 변경
`Grid.cs`의 Inspector에서:
- Grid Size: 4 (4x4) 또는 6 (6x6)
- Cell Size: 블록 크기 조정
- Cell Spacing: 블록 간격 조정

### 폭발 레벨 조정
`BlockMerger.cs`의 Inspector에서:
- Explode Level: 폭발 발동 레벨 (기본: 10)
- Explode Radius: 폭발 범위 (기본: 1)

### 점수 배율 조정
`ScoreManager.cs`의 Inspector에서:
- Combo Time Limit: 콤보 지속 시간
- Combo Multiplier: 콤보 점수 배율

## 🐛 트러블슈팅

### 블록이 클릭되지 않음
- Block 프리팹에 BoxCollider2D가 있는지 확인
- InputHandler의 Block Layer 설정 확인
- EventSystem이 씬에 있는지 확인

### 애니메이션이 작동하지 않음
- LeanTween.cs가 프로젝트에 포함되어 있는지 확인
- 스크립트 컴파일 에러가 없는지 확인

### UI 텍스트가 표시되지 않음
- TextMeshPro 패키지 설치 확인
- TMP 폰트 에셋이 있는지 확인

### 모바일에서 터치가 안됨
- Input System이 Legacy로 설정되어 있는지 확인
- Canvas의 Render Mode가 Screen Space - Overlay인지 확인

## 📱 빌드 및 배포

### Android APK 빌드
1. File → Build Settings
2. Platform: Android 선택
3. Switch Platform 클릭
4. Add Open Scenes 클릭
5. Player Settings 설정 (위 참조)
6. Build 또는 Build And Run

### iOS 빌드
1. File → Build Settings
2. Platform: iOS 선택
3. Switch Platform 클릭
4. Build 클릭
5. Xcode에서 프로젝트 열기
6. Signing & Capabilities 설정
7. 디바이스에 빌드

## 🎮 게임플레이 팁

### 초보자 전략
1. 낮은 레벨 블록을 먼저 병합
2. 한 쪽 구석에 높은 레벨 블록 모으기
3. 빈 공간을 최대한 많이 확보

### 고급 전략
1. 연쇄 병합 계획하기
2. 폭발 타이밍 조절
3. 콤보 시스템 활용
4. 그리드 전체를 균형있게 관리

### 고득점 노하우
- 🔥 연속 병합으로 콤보 유지
- 💥 계획된 폭발로 공간 확보
- 🎯 높은 레벨 블록 집중 생성
- ⚡ 빠른 판단과 실행

## 📝 향후 개선 사항

### 단기 목표
- [ ] 파티클 이펙트 추가
- [ ] 사운드 효과 강화
- [ ] 튜토리얼 추가
- [ ] 다양한 스킨/테마

### 중기 목표
- [ ] 일일 도전 과제
- [ ] 리더보드 (Google Play Games)
- [ ] 업적 시스템
- [ ] 아이템/부스터

### 장기 목표
- [ ] 멀티플레이어 모드
- [ ] 토너먼트 시스템
- [ ] 계절별 이벤트
- [ ] 캐릭터/스토리 모드

## 📄 라이선스
MIT License - 자유롭게 사용, 수정, 배포 가능

## 👥 기여
이슈나 개선 제안은 언제든 환영합니다!

## 📧 문의
개발 관련 문의나 피드백은 이슈 트래커를 이용해주세요.

---
**Enjoy the game! 🎮**
