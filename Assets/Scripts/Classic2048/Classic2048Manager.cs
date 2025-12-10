using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Classic 2048 게임 관리 클래스
/// </summary>
public class Classic2048Manager : MonoBehaviour
{
    public static Classic2048Manager Instance { get; private set; }
    
    [Header("자동 UI 생성")]
    public bool autoCreateUI = true;
    
    [Header("게임 설정")]
    public int startingTiles = 2;
    
    [Header("BGM")]
    public AudioClip gameBGM;
    
    [Header("컴포넌트")]
    public Classic2048Grid grid;
    public Classic2048Input inputHandler;
    public Button backButton;
    public GameObject gameOverPanel;
    // winPanel 제거 - 무한 플레이 모드
    
    [Header("UI 텍스트")]
    public Text scoreText;
    public Text bestScoreText;
    
    private Canvas canvas;
    private int currentScore = 0;
    private int bestScore = 0;
    private bool isGameOver = false;
    private bool isProcessingMove = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        Debug.Log("=== Classic2048Manager Start ===");
        
        LoadBestScore();
        
        if (autoCreateUI)
        {
            CreateCompleteGameUI();
        }
        
        StartGame();
    }
    
    private void OnDestroy()
    {
        // 씬 전환 시 Canvas 명시적 삭제
        if (canvas != null)
        {
            Destroy(canvas.gameObject);
        }
    }
    
    private void CreateCompleteGameUI()
    {
        // EventSystem이 없으면 생성
        if (UnityEngine.EventSystems.EventSystem.current == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            Debug.Log("✓ EventSystem 생성");
        }
        
        // 캔버스 생성
        GameObject canvasObj = new GameObject("GameCanvas");
        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // 배경
        CreateBackground(canvas);
        
        // 타이틀
        CreateTitle(canvas);
        
        // 점수 UI
        CreateScoreUI(canvas);
        
        // 그리드
        CreateGrid(canvas);
        
        // 입력 핸들러
        CreateInputHandler();
        
        // 뒤로가기 버튼
        CreateBackButton(canvas);
        
        // 게임오버 패널
        CreateGameOverPanel(canvas);
        
        // 승리 패널 제거 - 무한 플레이 모드
        // CreateWinPanel(canvas);
        
        Debug.Log("✓ Classic 2048 UI 생성 완료");
    }
    
    private void CreateBackground(Canvas canvas)
    {
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(canvas.transform, false);
        
        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0.98f, 0.97f, 0.94f);
        
        RectTransform bgRect = bgObj.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
    }
    
    private void CreateTitle(Canvas canvas)
    {
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(canvas.transform, false);
        
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "2048";
        titleText.fontSize = 80;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(0.47f, 0.43f, 0.40f);
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 1);
        titleRect.anchorMax = new Vector2(0, 1);
        titleRect.sizeDelta = new Vector2(300, 100);
        titleRect.anchoredPosition = new Vector2(200, -50);
    }
    
    private void CreateScoreUI(Canvas canvas)
    {
        // SCORE 박스 (우측 상단, 더 안쪽으로)
        scoreText = CreateScoreBox("SCORE", new Vector2(-300, -50));
        
        // BEST 박스 (우측 상단, SCORE 왼쪽)
        bestScoreText = CreateScoreBox("BEST", new Vector2(-480, -50));
        
        // 초기 점수 표시
        UpdateScoreUI();
    }
    
    private Text CreateScoreBox(string label, Vector2 position)
    {
        GameObject boxObj = new GameObject(label + "Box");
        boxObj.transform.SetParent(canvas.transform, false);
        
        Image boxImage = boxObj.AddComponent<Image>();
        boxImage.color = new Color(0.73f, 0.68f, 0.63f);
        
        RectTransform boxRect = boxObj.GetComponent<RectTransform>();
        boxRect.anchorMin = new Vector2(1, 1);
        boxRect.anchorMax = new Vector2(1, 1);
        boxRect.sizeDelta = new Vector2(150, 80);
        boxRect.anchoredPosition = position;
        
        // 라벨
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(boxObj.transform, false);
        
        Text labelText = labelObj.AddComponent<Text>();
        labelText.text = label;
        labelText.fontSize = 20;
        labelText.fontStyle = FontStyle.Bold;
        labelText.alignment = TextAnchor.MiddleCenter;
        labelText.color = new Color(0.93f, 0.89f, 0.85f);
        labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        RectTransform labelRect = labelObj.GetComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0.5f, 1);
        labelRect.anchorMax = new Vector2(0.5f, 1);
        labelRect.sizeDelta = new Vector2(140, 30);
        labelRect.anchoredPosition = new Vector2(0, -15);
        
        // 점수
        GameObject scoreObj = new GameObject("Score");
        scoreObj.transform.SetParent(boxObj.transform, false);
        
        Text scoreTextComponent = scoreObj.AddComponent<Text>();
        scoreTextComponent.text = "0";
        scoreTextComponent.fontSize = 32;
        scoreTextComponent.fontStyle = FontStyle.Bold;
        scoreTextComponent.alignment = TextAnchor.MiddleCenter;
        scoreTextComponent.color = Color.white;
        scoreTextComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        RectTransform scoreRect = scoreObj.GetComponent<RectTransform>();
        scoreRect.anchorMin = new Vector2(0.5f, 0);
        scoreRect.anchorMax = new Vector2(0.5f, 0);
        scoreRect.sizeDelta = new Vector2(140, 40);
        scoreRect.anchoredPosition = new Vector2(0, 20);
        
        return scoreTextComponent; // Text 컴포넌트 반환
    }
    
    private void CreateGrid(Canvas canvas)
    {
        GameObject gridObj = new GameObject("Grid");
        gridObj.transform.SetParent(canvas.transform, false);
        
        Image gridBg = gridObj.AddComponent<Image>();
        gridBg.color = new Color(0.73f, 0.68f, 0.63f);
        
        RectTransform gridRect = gridObj.GetComponent<RectTransform>();
        gridRect.anchorMin = new Vector2(0.5f, 0.5f);
        gridRect.anchorMax = new Vector2(0.5f, 0.5f);
        gridRect.sizeDelta = new Vector2(650, 650);
        gridRect.anchoredPosition = new Vector2(0, -50);
        
        grid = gridObj.AddComponent<Classic2048Grid>();
        grid.gridSize = 4;
        grid.cellSize = 140f;
        grid.cellSpacing = 15f;
        
        // 셀 배경 생성
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                GameObject cellObj = new GameObject($"Cell_{x}_{y}");
                cellObj.transform.SetParent(gridObj.transform, false);
                
                Image cellImage = cellObj.AddComponent<Image>();
                cellImage.color = new Color(0.80f, 0.76f, 0.71f, 0.35f);
                
                RectTransform cellRect = cellObj.GetComponent<RectTransform>();
                cellRect.sizeDelta = new Vector2(140f, 140f);
                
                float totalSize = 4 * 140f + 3 * 15f;
                float startX = -totalSize / 2f + 140f / 2f;
                float startY = -totalSize / 2f + 140f / 2f;
                float posX = startX + x * (140f + 15f);
                float posY = startY + y * (140f + 15f);
                
                cellRect.anchoredPosition = new Vector2(posX, posY);
            }
        }
        
        grid.Initialize();
    }
    
    private void CreateInputHandler()
    {
        GameObject inputObj = new GameObject("InputHandler");
        inputObj.transform.SetParent(transform);
        inputHandler = inputObj.AddComponent<Classic2048Input>();
    }
    
    private void CreateBackButton(Canvas canvas)
    {
        GameObject buttonObj = new GameObject("BackButton");
        buttonObj.transform.SetParent(canvas.transform, false);
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.55f, 0.47f, 0.43f);
        
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(() => BackToMenu());
        
        RectTransform rect = buttonObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.sizeDelta = new Vector2(150, 80);
        rect.anchoredPosition = new Vector2(-100, -75); // 우측상단에서 75px 왼쪽, 50px 아래
        
        // 텍스트
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        Text text = textObj.AddComponent<Text>();
        text.text = "← MENU";
        text.fontSize = 32;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = new Color(0.98f, 0.97f, 0.94f);
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        backButton = button;
    }
    
    private void CreateGameOverPanel(Canvas canvas)
    {
        GameObject panelObj = new GameObject("GameOverPanel");
        panelObj.transform.SetParent(canvas.transform, false);
        
        Image panelImage = panelObj.AddComponent<Image>();
        panelImage.color = new Color(0.98f, 0.97f, 0.94f, 0.8f);
        
        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;
        
        // Game Over 텍스트
        GameObject textObj = new GameObject("GameOverText");
        textObj.transform.SetParent(panelObj.transform, false);
        
        Text text = textObj.AddComponent<Text>();
        text.text = "Game Over!";
        text.fontSize = 80;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = new Color(0.47f, 0.43f, 0.40f);
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.sizeDelta = new Vector2(600, 120);
        textRect.anchoredPosition = new Vector2(0, 100);
        
        // 재시작 버튼
        CreatePanelButton(panelObj, "Try Again", new Vector2(0, -50), () => RestartGame());
        
        // 메뉴 버튼
        CreatePanelButton(panelObj, "Menu", new Vector2(0, -180), () => BackToMenu());
        
        gameOverPanel = panelObj;
        gameOverPanel.SetActive(false);
    }
    
    // 승리 패널 UI 생성 함수 제거 - 무한 플레이 모드
    /*
    private void CreateWinPanel(Canvas canvas)
    {
        GameObject panelObj = new GameObject("WinPanel");
        panelObj.transform.SetParent(canvas.transform, false);
        
        Image panelImage = panelObj.AddComponent<Image>();
        panelImage.color = new Color(0.93f, 0.81f, 0.45f, 0.9f);
        
        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;
        
        // You Win! 텍스트
        GameObject textObj = new GameObject("WinText");
        textObj.transform.SetParent(panelObj.transform, false);
        
        Text text = textObj.AddComponent<Text>();
        text.text = "You Win!";
        text.fontSize = 80;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.sizeDelta = new Vector2(600, 120);
        textRect.anchoredPosition = new Vector2(0, 100);
        
        // 계속하기 버튼
        CreatePanelButton(panelObj, "Keep Going", new Vector2(0, -50), () => ContinueGame());
        
        // 재시작 버튼
        CreatePanelButton(panelObj, "New Game", new Vector2(0, -180), () => RestartGame());
        
        winPanel = panelObj;
        winPanel.SetActive(false);
    }
    */
    
    private void CreatePanelButton(GameObject parent, string label, Vector2 position, UnityEngine.Events.UnityAction onClick)
    {
        GameObject buttonObj = new GameObject(label + "_Button");
        buttonObj.transform.SetParent(parent.transform, false);
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.55f, 0.47f, 0.43f);
        
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(onClick);
        button.onClick.AddListener(() => 
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayClickSound();
        });
        
        RectTransform rect = buttonObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(400, 100);
        rect.anchoredPosition = position;
        
        // 텍스트
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        Text text = textObj.AddComponent<Text>();
        text.text = label;
        text.fontSize = 40;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
    }
    
    private void StartGame()
    {
        Debug.Log("=== 게임 시작 ===");
        
        // BGM 재생
        if (AudioManager.Instance != null && gameBGM != null)
        {
            AudioManager.Instance.StopBGM();
            AudioManager.Instance.bgmClip = gameBGM;
            AudioManager.Instance.PlayBGM();
        }
        
        // 초기화
        isGameOver = false;
        currentScore = 0;
        UpdateScoreUI();
        
        if (grid != null)
        {
            grid.Initialize();
            
            // 시작 타일 추가
            for (int i = 0; i < startingTiles; i++)
            {
                grid.AddRandomTile();
            }
        }
    }
    
    public void OnSwipe(Vector2Int direction)
    {
        if (isGameOver || isProcessingMove)
            return;
        
        StartCoroutine(ProcessMove(direction));
    }
    
    private IEnumerator ProcessMove(Vector2Int direction)
    {
        isProcessingMove = true;
        
        bool moved = grid.MoveTiles(direction);
        
        if (moved)
        {
            // 이동 사운드
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayClickSound();
            }
            
            // 애니메이션 대기
            yield return new WaitForSeconds(0.2f);
            
            // 새 타일 추가
            grid.AddRandomTile();
            
            // 2048 달성 축하 로그 (무한 플레이 계속)
            if (grid.Has2048Tile())
            {
                Debug.Log("🎉 2048 달성! 계속해서 더 높은 점수를 목표로!");
            }
            
            // 게임오버 체크
            if (!grid.CanMove())
            {
                GameOver();
            }
        }
        
        isProcessingMove = false;
    }
    
    public void AddScore(int points)
    {
        currentScore += points;
        
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            SaveBestScore();
        }
        
        UpdateScoreUI();
    }
    
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
            Debug.Log($"점수 업데이트: {currentScore}");
        }
        else
        {
            Debug.LogWarning("scoreText가 null입니다!");
        }
        
        if (bestScoreText != null)
        {
            bestScoreText.text = bestScore.ToString();
        }
        else
        {
            Debug.LogWarning("bestScoreText가 null입니다!");
        }
    }
    
    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("🎮 게임오버");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOverSound();
        }
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
    
    // 승리 패널 기능 제거 - 무한 플레이 모드
    /*
    private void ShowWinPanel()
    {
        Debug.Log("🎉 2048 달성!");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayComboSound();
        }
        
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }
    
    private void ContinueGame()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }
    */
    
    private void RestartGame()
    {
        Debug.Log("🔄 게임 재시작");
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        if (grid != null)
        {
            grid.Clear();
        }
        
        StartGame();
    }
    
    private void BackToMenu()
    {
        Debug.Log("📋 메인 메뉴로 이동");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayClickSound();
        }
        
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("MainMenu");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
    
    private void SaveBestScore()
    {
        PlayerPrefs.SetInt("Classic2048_BestScore", bestScore);
        PlayerPrefs.Save();
    }
    
    private void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt("Classic2048_BestScore", 0);
        Debug.Log($"최고 점수 로드: {bestScore}");
    }
}
