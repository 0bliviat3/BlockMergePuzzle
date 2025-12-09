using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// 게임 전체 관리 클래스 - 개선 버전 (난이도 상향, 게임오버 조건 명확화)
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("게임 상태")]
    public bool isGameActive = false;
    public int maxBlockLevel = 1;
    
    [Header("게임 설정 - 난이도 조정")]
    public int startingBlocks = 5;  // 8 → 5 (어려워짐)
    public int newBlocksPerTurn = 1;
    public float newBlockDelay = 0.5f;
    
    [Header("난이도 설정 ⭐")]
    [Tooltip("레벨 1 블록 출현 확률 (%)")]
    public float level1Probability = 60f;  // 90 → 60
    [Tooltip("레벨 2 블록 출현 확률 (%)")]
    public float level2Probability = 30f;  // 10 → 30
    [Tooltip("레벨 3 블록 출현 확률 (%)")]
    public float level3Probability = 10f;  // 0 → 10
    
    [Header("필수 참조")]
    public Grid grid;
    public BlockMerger blockMerger;
    public ScoreManager scoreManager;
    public EffectManager effectManager;
    public InputHandler inputHandler;
    
    [Header("UI - 선택적")]
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject gameOverPanel;
    public TextMeshProUGUI highestBlockText;
    public TextMeshProUGUI movesText;
    
    private int moveCount = 0;
    private GameObject mainMenuButton; // 메인 메뉴 버튼 참조
    
    private void Awake()
    {
        Debug.Log("=== GameManager Awake 시작 ===");
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        CheckRequiredReferences();
    }
    
    private void Start()
    {
        Debug.Log("=== GameManager Start 시작 ===");
        Initialize();
        CreateMainMenuButton();
    }
    
    /// <summary>
    /// 필수 참조 체크
    /// </summary>
    private void CheckRequiredReferences()
    {
        Debug.Log("[참조 체크]");
        
        bool hasError = false;
        
        if (grid == null)
        {
            Debug.LogError("!!! Grid가 연결되지 않았습니다!");
            hasError = true;
        }
        else
        {
            Debug.Log("✓ Grid 연결됨");
        }
        
        if (blockMerger == null)
        {
            Debug.LogWarning("BlockMerger가 연결되지 않았습니다.");
        }
        
        if (scoreManager == null)
        {
            Debug.LogWarning("ScoreManager가 연결되지 않았습니다.");
        }
        
        if (effectManager == null)
        {
            Debug.LogWarning("EffectManager가 연결되지 않았습니다.");
        }
        
        if (inputHandler == null)
        {
            Debug.LogWarning("InputHandler가 연결되지 않았습니다.");
        }
        
        if (hasError)
        {
            Debug.LogError("필수 참조가 없어 게임을 시작할 수 없습니다!");
        }
    }
    
    /// <summary>
    /// 게임 초기화
    /// </summary>
    private void Initialize()
    {
        Debug.Log("[Initialize 시작]");
        
        if (grid == null)
        {
            Debug.LogError("Grid가 null이라 Initialize를 진행할 수 없습니다!");
            return;
        }
        
        try
        {
            Debug.Log("Grid.Initialize() 호출...");
            grid.Initialize();
            Debug.Log("✓ Grid 초기화 완료");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Grid 초기화 실패: {e.Message}\n{e.StackTrace}");
            return;
        }
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            Debug.Log("✓ GameOverPanel 비활성화");
        }
        
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
            Debug.Log("✓ RestartButton 리스너 추가");
        }
        
        Debug.Log("StartNewGame() 호출...");
        StartNewGame();
    }
    
    /// <summary>
    /// 새 게임 시작
    /// </summary>
    public void StartNewGame()
    {
        Debug.Log("=== StartNewGame 시작 ===");
        
        if (grid == null)
        {
            Debug.LogError("Grid가 null이라 게임을 시작할 수 없습니다!");
            return;
        }
        
        // 이전 게임 데이터 초기화
        grid.ClearAllBlocks();
        Debug.Log("✓ 기존 블록 제거 완료");
        
        if (scoreManager != null)
        {
            scoreManager.ResetScore();
            Debug.Log("✓ 점수 리셋 완료");
        }
        
        maxBlockLevel = 1;
        moveCount = 0;
        UpdateMovesUI();
        
        // 시작 블록 추가
        Debug.Log($"시작 블록 {startingBlocks}개 생성 시작...");
        for (int i = 0; i < startingBlocks; i++)
        {
            try
            {
                int level = GetRandomBlockLevel();
                Block block = grid.AddRandomBlock(level);
                if (block != null)
                {
                    Debug.Log($"✓ 블록 {i + 1}/{startingBlocks} 생성 성공 - 위치: {block.gridPosition}, 레벨: {level}");
                }
                else
                {
                    Debug.LogError($"✗ 블록 {i + 1}/{startingBlocks} 생성 실패");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"블록 생성 중 에러: {e.Message}\n{e.StackTrace}");
            }
        }
        
        var blocks = grid.GetAllBlocks();
        Debug.Log($"현재 생성된 블록 수: {blocks.Count}");
        
        isGameActive = true;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        UpdateHighestBlockUI();
        
        Debug.Log("=== StartNewGame 완료 ===");
    }
    
    /// <summary>
    /// 랜덤 블록 레벨 결정 (난이도 상향)
    /// </summary>
    public int GetRandomBlockLevel()  // private → public (폭발 후 빈 칸 채우기에 사용)
    {
        float random = Random.Range(0f, 100f);
        
        if (random < level1Probability)
        {
            return 1; // 60% 확률
        }
        else if (random < level1Probability + level2Probability)
        {
            return 2; // 30% 확률
        }
        else
        {
            return 3; // 10% 확률
        }
    }
    
    /// <summary>
    /// 게임 재시작
    /// </summary>
    public void RestartGame()
    {
        Debug.Log("RestartGame 호출");
        StartNewGame();
    }
    
    /// <summary>
    /// 새 블록 추가
    /// </summary>
    public void AddNewBlock()
    {
        if (!isGameActive) return;
        
        moveCount++;
        UpdateMovesUI();
        
        StartCoroutine(AddNewBlockCoroutine());
    }
    
    /// <summary>
    /// 새 블록 추가 코루틴
    /// </summary>
    private IEnumerator AddNewBlockCoroutine()
    {
        yield return new WaitForSeconds(newBlockDelay);
        
        // 난이도 상향: 다양한 레벨 블록 추가
        int blockLevel = GetRandomBlockLevel();
        
        Block newBlock = grid.AddRandomBlock(blockLevel);
        
        if (newBlock == null)
        {
            Debug.Log("⚠️ 빈 공간이 없음 - 게임오버 체크");
            CheckGameOverImmediate();
        }
        else
        {
            Debug.Log($"✓ 새 블록 추가됨: 레벨 {blockLevel} (위치: {newBlock.gridPosition})");
            
            // 최고 레벨 블록 업데이트
            if (newBlock.level > maxBlockLevel)
            {
                maxBlockLevel = newBlock.level;
                UpdateHighestBlockUI();
            }
            
            // ⭐ 핵심: 블록 추가 후 즉시 게임오버 체크
            CheckGameOverImmediate();
        }
    }
    
    /// <summary>
    /// 즉시 게임 오버 체크 (개선된 방식) ⭐
    /// </summary>
    public void CheckGameOverImmediate()  // private → public (폭발 후에도 호출 가능)
    {
        if (blockMerger == null || grid == null)
        {
            return;
        }
        
        // 병합 가능한 블록이 있는지 체크
        bool hasPossibleMerges = blockMerger.HasPossibleMerges();
        
        Debug.Log($"🎮 게임오버 체크 - 병합 가능: {hasPossibleMerges}");
        
        if (!hasPossibleMerges)
        {
            Debug.Log("❌ 더 이상 병합할 수 없습니다!");
            GameOver();
        }
        else
        {
            Debug.Log("✅ 계속 플레이 가능");
        }
    }
    
    /// <summary>
    /// 게임 오버 (더 이상 이동 불가능)
    /// </summary>
    private void GameOver()
    {
        Debug.Log("=== 게임 오버 ===");
        isGameActive = false;
        
        // 게임 오버 UI 표시
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            
            if (gameOverText != null && scoreManager != null)
            {
                int score = scoreManager.GetCurrentScore();
                int highestBlock = GetHighestBlockValue();
                
                gameOverText.text = $"Game Over!\n\n" +
                    $"Score: {score:N0}\n" +
                    $"Moves: {moveCount}\n" +
                    $"Highest Block: {highestBlock:N0}\n\n" +
                    $"No More Moves!";
                
                Debug.Log($"📊 최종 결과 - 점수: {score}, 이동: {moveCount}, 최고블록: {highestBlock}");
            }
        }
        
        // 최고 기록 저장
        if (scoreManager != null)
        {
            scoreManager.SaveHighScore();
        }
    }
    
    /// <summary>
    /// 최고 블록 값 반환
    /// </summary>
    public int GetHighestBlockValue()
    {
        return (int)Mathf.Pow(2, maxBlockLevel);
    }
    
    /// <summary>
    /// 최고 블록 UI 업데이트
    /// </summary>
    private void UpdateHighestBlockUI()
    {
        if (highestBlockText != null)
        {
            highestBlockText.text = $"Highest: {GetHighestBlockValue()}";
        }
    }
    
    /// <summary>
    /// 이동 횟수 UI 업데이트
    /// </summary>
    private void UpdateMovesUI()
    {
        if (movesText != null)
        {
            movesText.text = $"Moves: {moveCount}";
        }
    }
    
    /// <summary>
    /// 블록 레벨 업데이트
    /// </summary>
    public void UpdateMaxBlockLevel(int level)
    {
        if (level > maxBlockLevel)
        {
            maxBlockLevel = level;
            UpdateHighestBlockUI();
            
            // 마일스톤 달성 효과
            if (level >= 8 && effectManager != null) // 256 이상
            {
                effectManager.PlayMilestoneEffect();
                Debug.Log($"🎉 마일스톤 달성! 레벨 {level} ({GetHighestBlockValue()})");
            }
        }
    }
    
    /// <summary>
    /// 힌트 표시
    /// </summary>
    public void ShowHint()
    {
        if (!isGameActive || grid == null || effectManager == null) return;
        
        var allBlocks = grid.GetAllBlocks();
        
        foreach (Block block in allBlocks)
        {
            var adjacentBlocks = grid.GetAdjacentBlocks(block.gridPosition);
            
            foreach (Block adjacent in adjacentBlocks)
            {
                if (block.level == adjacent.level)
                {
                    effectManager.PlayHintEffect(block.transform.position);
                    effectManager.PlayHintEffect(adjacent.transform.position);
                    Debug.Log($"💡 힌트: {block.gridPosition}와 {adjacent.gridPosition}를 병합하세요");
                    return;
                }
            }
        }
        
        Debug.Log("❌ 병합 가능한 블록이 없습니다!");
    }
    
    /// <summary>
    /// 게임 일시정지
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("게임 일시정지");
    }
    
    /// <summary>
    /// 게임 재개
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Debug.Log("게임 재개");
    }
    
    /// <summary>
    /// 메인 메뉴 이동 버튼 생성
    /// </summary>
    private void CreateMainMenuButton()
    {
        // Canvas 찾기
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("Canvas를 찾을 수 없어 메인 메뉴 버튼을 생성하지 못했습니다.");
            return;
        }
        
        // 버튼 생성
        GameObject buttonObj = new GameObject("MainMenuButton");
        buttonObj.transform.SetParent(canvas.transform, false);
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.3f, 0.4f); // 어두운 파란색
        
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(() => BackToMainMenu());
        
        RectTransform rect = buttonObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.sizeDelta = new Vector2(150, 80);
        rect.anchoredPosition = new Vector2(-100, -75); // 우측상단 (Classic2048과 동일)
        
        // 텍스트
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        Text text = textObj.AddComponent<Text>();
        text.text = "← MENU";
        text.fontSize = 32;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        // 버튼 참조 저장
        mainMenuButton = buttonObj;
        
        Debug.Log("✓ 메인 메뉴 버튼 생성 완료");
    }
    
    /// <summary>
    /// 메인 메뉴로 이동
    /// </summary>
    private void BackToMainMenu()
    {
        Debug.Log("메인 메뉴로 이동");
        Time.timeScale = 1f; // 시간 정상화
        
        // 메인 메뉴 버튼 삭제
        if (mainMenuButton != null)
        {
            Destroy(mainMenuButton);
            Debug.Log("✓ 메인 메뉴 버튼 삭제");
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
}
