# 📝 변경 로그

## [2025-12-09] 2048 Collection - Multi-Game Architecture

### 추가된 기능:
- ✅ 메인 메뉴 시스템 (게임 선택)
- ✅ Classic 2048 게임 (4x4 오리지널)
- ✅ 씬 전환 시스템 (페이드 효과)
- ✅ 멀티 게임 아키텍처

### 새로 작성된 스크립트 (6개):
1. SceneLoader.cs - 씬 전환 관리
2. MainMenuManager.cs - 메인 메뉴 UI
3. Classic2048Manager.cs - 2048 게임 관리
4. Classic2048Grid.cs - 4x4 그리드 관리
5. Classic2048Tile.cs - 타일 클래스
6. Classic2048Input.cs - 스와이프 입력

### 주요 특징:
- 자동 UI 생성 (MainMenu, Classic2048)
- 스와이프 + 키보드(W/A/S/D) 입력
- 2048 달성 감지 & 승리 패널
- 게임오버 감지 & 재시작
- 점수 시스템 & 최고 점수 저장
- 부드러운 씬 전환 (페이드)
- 뒤로가기 버튼

### 폴더 구조:
```
Assets/Scripts/
  ├── Shared/        (공통)
  ├── MainMenu/      (메인 메뉴)
  ├── Classic2048/   (2048 게임)
  └── BlockMerge/    (기존 게임, 향후 이동)
```

### 씬 구조:
```
0. MainMenu (시작 화면)
1. BlockMergePuzzle (기존 게임)
2. Classic2048 (새 게임)
```

### 확장 가능성:
- 새 게임 추가 준비 완료
- 게임별 독립적인 폴더 구조
- 공통 시스템 공유 (Audio, Settings, SceneLoader)

### 문서 작성:
- CLASSIC2048_SETUP_GUIDE.md (상세 가이드)
- CLASSIC2048_QUICK_START.md (5분 시작)
- MULTI_GAME_SUMMARY.md (전체 요약)
- PROJECT_RESTRUCTURE.md (재구성 계획)

### 테스트 필요:
- MainMenu 씬 생성 및 테스트
- Classic2048 씬 생성 및 테스트
- 씬 전환 테스트
- 스와이프 조작 테스트
- Build Settings 설정

---

## 이전 버전

### [2025-12-09] Audio System Improvements
- AudioManager에 combo, click 사운드 추가
- BlockMerger merge 사운드 재생 코드 추가
- 디버그 로그 강화

### [2025-12-09] Mobile UX Improvements v2.1
- 블록 크기 40% 증가 (140px)
- Settings 패널 구현
- 폭발 후 빈 칸 자동 채우기
- 설정 버튼 아이콘 수정

### [2025-12-08] Initial Release
- Block Merge Puzzle 게임 완성
- 기본 게임 로직
- 점수/콤보 시스템
- 업적 시스템
