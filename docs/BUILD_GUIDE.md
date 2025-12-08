# Block Merge Puzzle - 빌드 가이드

## 🎯 빌드 준비사항

### 필수 소프트웨어
- Unity 2021.3 LTS 이상
- Android: Android SDK, NDK
- iOS: Xcode 13 이상, macOS

### 프로젝트 설정 확인
```
1. Unity 프로젝트 열기
2. File → Build Settings 확인
3. Player Settings 검토
4. 모든 씬이 Build Settings에 포함되었는지 확인
```

## 📱 Android 빌드

### 1단계: Build Settings

```
File → Build Settings

Platform: Android
- Add Open Scenes (메인 씬 추가)
- Switch Platform 클릭
```

### 2단계: Player Settings

#### Company 및 Product
```
Player Settings → Company & Product

Company Name: [회사명]
Product Name: Block Merge Puzzle
```

#### Identification
```
Player Settings → Identification

Package Name: com.[회사명].blockmerge
Version: 1.0.0
Bundle Version Code: 1

Minimum API Level: Android 5.0 (API level 21)
Target API Level: Automatic (highest installed)
```

#### Resolution and Presentation
```
Player Settings → Resolution and Presentation

Default Orientation: Portrait
Allowed Orientations:
- Portrait: ✓
- Portrait Upside Down: ✗
- Landscape Right: ✗
- Landscape Left: ✗
```

#### Icon
```
Player Settings → Icon

Default Icon: [1024x1024 아이콘]
Adaptive Icon:
- Foreground: [포그라운드 레이어]
- Background: [배경 색상 또는 이미지]
```

#### Splash Image
```
Player Settings → Splash Image

Show Splash Screen: ✓
Splash Screen Logo: [로고 이미지]
Background Color: [원하는 색상]
```

#### Other Settings
```
Player Settings → Other Settings

Rendering:
- Color Space: Linear (권장)
- Graphics API: OpenGLES3, OpenGLES2

Scripting Backend: IL2CPP
Target Architectures:
- ARMv7: ✗ (구형 기기 지원 시 체크)
- ARM64: ✓ (필수)

Managed Stripping Level: Medium
```

### 3단계: Keystore 생성 (첫 빌드 시)

```
Player Settings → Publishing Settings

Create a new keystore:
1. Keystore Name: [프로젝트명].keystore
2. Password: [강력한 비밀번호]

Create new key:
- Alias: [키 별칭]
- Password: [키 비밀번호]
- Validity: 25+ years
- First and Last Name: [이름]
- Organizational Unit: [부서]
- Organization: [회사명]
- City: [도시]
- State: [주/도]
- Country Code: [KR/US 등]

⚠️ 중요: Keystore 파일과 비밀번호를 안전하게 보관!
```

### 4단계: 빌드 옵션

```
Build Settings

Development Build: 
- 테스트용: ✓
- 출시용: ✗

Compression Method:
- LZ4: 빠른 로딩 (권장)
- LZ4HC: 작은 용량

Build App Bundle (Google Play):
- ✓ (Google Play 출시 시)
- ✗ (APK 직접 배포 시)
```

### 5단계: 빌드 실행

```
Build Settings → Build

1. 저장 위치 선택
2. 파일명: BlockMergePuzzle.apk (또는 .aab)
3. Build 클릭
4. 빌드 완료 대기 (5-15분)
```

### 6단계: 테스트

```
1. APK를 Android 기기에 설치
2. 모든 기능 테스트
3. 다양한 기기에서 테스트
4. 성능 모니터링
```

## 🍎 iOS 빌드

### 1단계: Build Settings

```
File → Build Settings

Platform: iOS
- Add Open Scenes
- Switch Platform 클릭
```

### 2단계: Player Settings

#### Identification
```
Player Settings → Identification

Bundle Identifier: com.[회사명].blockmerge
Version: 1.0.0
Build: 1
```

#### Resolution and Presentation
```
Player Settings → Resolution and Presentation

Default Orientation: Portrait
Status Bar: Hidden
Requires Fullscreen: ✓
```

#### Icon
```
Player Settings → Icon

iOS App Icon: [아이콘 세트]
(1024x1024, 180x180, 120x120 등)
```

#### Other Settings
```
Player Settings → Other Settings

Target minimum iOS Version: 12.0
Architecture: ARM64

Camera Usage Description: ""
(카메라 미사용 시 공란)

Scripting Backend: IL2CPP
```

### 3단계: Xcode 프로젝트 생성

```
Build Settings → Build

1. 폴더 선택 (예: Builds/iOS)
2. Build 클릭
3. Xcode 프로젝트 생성 대기
```

### 4단계: Xcode에서 설정

```
1. Xcode에서 생성된 프로젝트 열기
2. Signing & Capabilities:
   - Team: [개발자 계정]
   - Automatically manage signing: ✓
   
3. General:
   - Display Name: Block Merge Puzzle
   - Bundle Identifier: 확인
   - Version/Build: 확인
   
4. Build Settings:
   - Enable Bitcode: No
```

### 5단계: 아카이브 및 제출

```
1. Xcode: Product → Archive
2. Window → Organizer
3. Distribute App
4. App Store Connect 선택
5. Upload
6. TestFlight 또는 출시
```

## 🔍 빌드 최적화

### 용량 최적화

#### Texture 압축
```
Project Settings → Quality

Texture Quality: Medium
```

#### Audio 압축
```
모든 AudioClip 선택:
- Load Type: Compressed In Memory
- Compression Format: Vorbis
- Quality: 70%
```

#### Script Stripping
```
Player Settings → Other Settings

Managed Stripping Level: High
(테스트 후 문제 없으면 적용)
```

### 성능 최적화

#### 프레임레이트 제한
```csharp
// GameManager.cs Awake()에 추가
Application.targetFrameRate = 60;
```

#### 배터리 최적화
```
Player Settings → Resolution and Presentation

Run In Background: ✗
```

## 🐛 일반적인 빌드 오류 및 해결

### Android

#### 오류: "SDK not found"
```
해결: Unity Hub → Installs → 
      Android Build Support 설치
```

#### 오류: "NDK not found"
```
해결: Edit → Preferences → External Tools
      NDK 경로 설정
```

#### 오류: "Keystore not found"
```
해결: 올바른 Keystore 파일 경로 확인
      비밀번호 재확인
```

### iOS

#### 오류: "Xcode not found"
```
해결: Xcode 설치
      Command Line Tools 설치
```

#### 오류: "Signing error"
```
해결: Apple Developer 계정 확인
      Certificate 재발급
```

## 📦 출시 전 체크리스트

### 필수 확인사항
- [ ] 모든 기능 정상 작동
- [ ] 크래시 없음
- [ ] 메모리 누수 없음
- [ ] 성능 문제 없음
- [ ] UI 텍스트 오타 확인
- [ ] 권한 설정 확인
- [ ] 개인정보 처리방침 준비

### Android 추가 체크
- [ ] 다양한 해상도 테스트
- [ ] 다양한 Android 버전 테스트
- [ ] Google Play Console 설정
- [ ] 스크린샷 준비 (최소 2개)
- [ ] 앱 설명 작성

### iOS 추가 체크
- [ ] 다양한 iOS 기기 테스트
- [ ] App Store Connect 설정
- [ ] 스크린샷 준비 (각 디바이스별)
- [ ] 앱 미리보기 비디오 (선택)

## 🚀 출시 프로세스

### Google Play Console

#### 1. 앱 만들기
```
1. Google Play Console 로그인
2. 모든 앱 → 앱 만들기
3. 앱 세부정보 입력
4. 앱 카테고리 선택
```

#### 2. 스토어 등록정보
```
제품 세부정보:
- 앱 이름: Block Merge Puzzle
- 간단한 설명: (80자)
- 자세한 설명: (4000자)
- 스크린샷: 최소 2개, 권장 8개
- 아이콘: 512x512
```

#### 3. 앱 콘텐츠
```
- 개인정보처리방침 URL
- 앱 카테고리 및 연락처 세부정보
- 타겟층 및 콘텐츠
```

#### 4. 내부 테스트 (권장)
```
1. 내부 테스트 트랙 생성
2. AAB 업로드
3. 테스터 추가
4. 테스트 진행
```

#### 5. 프로덕션 출시
```
1. 프로덕션 트랙 선택
2. AAB 업로드
3. 출시 정보 입력
4. 심사 제출
```

### App Store Connect

#### 1. 앱 정보
```
1. App Store Connect 로그인
2. My Apps → + → New App
3. 플랫폼: iOS
4. 앱 이름 및 Bundle ID
5. SKU 및 언어 설정
```

#### 2. 앱 정보 입력
```
- 부제목 (30자)
- 설명 (4000자)
- 키워드 (100자)
- 스크린샷 (각 크기별)
- 앱 미리보기 (선택)
```

#### 3. 빌드 업로드
```
1. Xcode에서 Archive
2. Distribute App → App Store Connect
3. Upload 완료 대기 (10-60분)
```

#### 4. TestFlight 테스트 (권장)
```
1. 테스터 그룹 생성
2. 빌드 선택
3. 내부/외부 테스터 추가
4. 테스트 진행
```

#### 5. 출시 제출
```
1. 버전 정보 확인
2. 심사 정보 입력
3. 가격 및 배포 국가 설정
4. 심사 제출
```

## 📊 출시 후 모니터링

### 필수 확인사항
- 크래시 리포트 확인
- 사용자 리뷰 모니터링
- 다운로드 수 추적
- 리텐션율 분석
- 수익 데이터 확인

### 분석 도구
- Google Play Console Analytics
- App Store Connect Analytics
- Firebase Analytics (추가 설치)
- Unity Analytics

## 🔄 업데이트 프로세스

### 버전 업데이트
```
1. 버전 번호 증가
   - Android: Version Code +1
   - iOS: Build Number +1

2. 변경사항 문서화
3. 빌드 생성
4. 테스트
5. 스토어 제출
```

### 핫픽스
```
긴급 버그 수정:
1. 버그 수정
2. 빠른 테스트
3. 긴급 출시 요청
```

## 💡 팁 및 권장사항

### 개발 단계
- 자주 빌드하여 문제 조기 발견
- 다양한 기기에서 테스트
- 프로파일러로 성능 최적화

### 출시 준비
- 출시 전 충분한 테스트 기간 확보
- 내부/외부 테스터 활용
- A/B 테스트로 스토어 페이지 최적화

### 출시 후
- 빠른 버그 대응
- 사용자 피드백 경청
- 정기적인 콘텐츠 업데이트

---

**Good luck with your launch! 🎉**
