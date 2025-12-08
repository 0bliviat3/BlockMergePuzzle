using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// 점수 관리 클래스 - Combo UI 개선
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [Header("점수")]
    private int currentScore = 0;
    private int highScore = 0;
    private int comboCount = 0;
    
    [Header("콤보 설정")]
    public float comboTimeLimit = 3f;
    public float comboMultiplier = 0.5f; // 콤보당 50% 보너스
    private float comboTimer = 0f;
    
    [Header("UI - 선택적")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI comboText;
    public GameObject comboPanel;
    
    [Header("애니메이션")]
    public float scoreAnimationDuration = 0.5f;
    
    private Coroutine comboCoroutine;
    
    private void Start()
    {
        Debug.Log("=== ScoreManager Start ===");
        LoadHighScore();
        UpdateScoreUI();
        
        if (comboPanel != null)
        {
            comboPanel.SetActive(false);
        }
        
        // Combo UI가 없으면 자동 생성 시도
        if (comboText == null)
        {
            TryAutoCreateComboUI();
        }
    }
    
    /// <summary>
    /// Combo UI 자동 생성 시도
    /// </summary>
    private void TryAutoCreateComboUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("Canvas를 찾을 수 없어 Combo UI를 생성할 수 없습니다.");
            return;
        }
        
        // Combo Panel 생성
        GameObject comboPanelObj = new GameObject("ComboPanel");
        comboPanelObj.transform.SetParent(canvas.transform, false);
        
        RectTransform comboPanelRect = comboPanelObj.AddComponent<RectTransform>();
        comboPanelRect.anchorMin = new Vector2(0.5f, 0.8f);
        comboPanelRect.anchorMax = new Vector2(0.5f, 0.8f);
        comboPanelRect.sizeDelta = new Vector2(300, 100);
        comboPanelRect.anchoredPosition = Vector2.zero;
        
        // Combo Text 생성
        GameObject comboTextObj = new GameObject("ComboText");
        comboTextObj.transform.SetParent(comboPanelObj.transform, false);
        
        TextMeshProUGUI comboTextComp = comboTextObj.AddComponent<TextMeshProUGUI>();
        comboTextComp.fontSize = 48;
        comboTextComp.fontStyle = FontStyles.Bold;
        comboTextComp.alignment = TextAlignmentOptions.Center;
        comboTextComp.color = Color.yellow;
        comboTextComp.text = "Combo x1";
        
        RectTransform comboTextRect = comboTextObj.GetComponent<RectTransform>();
        comboTextRect.anchorMin = Vector2.zero;
        comboTextRect.anchorMax = Vector2.one;
        comboTextRect.sizeDelta = Vector2.zero;
        comboTextRect.anchoredPosition = Vector2.zero;
        
        // 참조 연결
        comboPanel = comboPanelObj;
        comboText = comboTextComp;
        
        comboPanel.SetActive(false);
        
        Debug.Log("✓ Combo UI 자동 생성 완료");
    }
    
    /// <summary>
    /// 점수 추가
    /// </summary>
    public void AddScore(int points)
    {
        // 콤보 배율 적용
        int finalPoints = points;
        if (comboCount > 0)
        {
            float multiplier = 1 + (comboCount * comboMultiplier);
            finalPoints = Mathf.RoundToInt(points * multiplier);
        }
        
        currentScore += finalPoints;
        Debug.Log($"점수 추가: +{finalPoints} (기본: {points}, 콤보: x{comboCount})");
        
        // 점수 애니메이션
        AnimateScoreText(finalPoints);
        
        // 최고 점수 갱신
        if (currentScore > highScore)
        {
            highScore = currentScore;
            UpdateHighScoreUI();
            AnimateHighScoreText();
            Debug.Log($"새 최고 점수: {highScore}");
        }
        
        UpdateScoreUI();
    }
    
    /// <summary>
    /// 콤보 추가
    /// </summary>
    public void AddCombo()
    {
        comboCount++;
        comboTimer = comboTimeLimit;
        Debug.Log($"🔥 콤보 추가: x{comboCount} (다음 병합 점수 {(1 + comboCount * comboMultiplier) * 100}%)");
        
        UpdateComboUI();
        ShowComboMessage();
        
        // 콤보 타이머 시작
        if (comboCoroutine != null)
        {
            StopCoroutine(comboCoroutine);
        }
        comboCoroutine = StartCoroutine(ComboTimerCoroutine());
    }
    
    /// <summary>
    /// 콤보 메시지 표시
    /// </summary>
    private void ShowComboMessage()
    {
        if (comboPanel != null && comboText != null)
        {
            comboPanel.SetActive(true);
            
            // 펄스 애니메이션
            LeanTween.cancel(comboPanel);
            comboPanel.transform.localScale = Vector3.one * 1.5f;
            
            LeanTween.scale(comboPanel, Vector3.one, 0.3f)
                .setEase(LeanTweenType.easeOutBack);
        }
        else
        {
            Debug.LogWarning("⚠️ Combo UI가 없어서 콤보 메시지를 표시할 수 없습니다!");
        }
    }
    
    /// <summary>
    /// 콤보 초기화
    /// </summary>
    private void ResetCombo()
    {
        if (comboCount > 0)
        {
            Debug.Log($"콤보 종료: x{comboCount}");
        }
        
        comboCount = 0;
        comboTimer = 0f;
        
        if (comboPanel != null)
        {
            comboPanel.SetActive(false);
        }
        
        if (comboCoroutine != null)
        {
            StopCoroutine(comboCoroutine);
            comboCoroutine = null;
        }
    }
    
    /// <summary>
    /// 콤보 타이머 코루틴
    /// </summary>
    private IEnumerator ComboTimerCoroutine()
    {
        if (comboPanel != null)
        {
            comboPanel.SetActive(true);
        }
        
        while (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            
            // 타이머가 1초 이하일 때 깜빡임
            if (comboTimer < 1f && comboText != null)
            {
                float alpha = Mathf.PingPong(Time.time * 3f, 1f);
                Color color = comboText.color;
                color.a = alpha;
                comboText.color = color;
            }
            
            yield return null;
        }
        
        ResetCombo();
    }
    
    /// <summary>
    /// 점수 UI 업데이트
    /// </summary>
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore:N0}";
        }
    }
    
    /// <summary>
    /// 최고 점수 UI 업데이트
    /// </summary>
    private void UpdateHighScoreUI()
    {
        if (highScoreText != null)
        {
            highScoreText.text = $"Best: {highScore:N0}";
        }
    }
    
    /// <summary>
    /// 콤보 UI 업데이트
    /// </summary>
    private void UpdateComboUI()
    {
        if (comboText != null)
        {
            comboText.text = $"COMBO x{comboCount}";
            
            // 콤보 색상 변화
            if (comboCount >= 5)
            {
                comboText.color = Color.red; // 5콤보 이상: 빨강
            }
            else if (comboCount >= 3)
            {
                comboText.color = new Color(1f, 0.5f, 0f); // 3콤보 이상: 주황
            }
            else
            {
                comboText.color = Color.yellow; // 기본: 노랑
            }
        }
    }
    
    /// <summary>
    /// 점수 텍스트 애니메이션
    /// </summary>
    private void AnimateScoreText(int points)
    {
        if (scoreText == null) return;
        
        LeanTween.cancel(scoreText.gameObject);
        scoreText.transform.localScale = Vector3.one;
        
        LeanTween.scale(scoreText.gameObject, Vector3.one * 1.2f, scoreAnimationDuration * 0.5f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                LeanTween.scale(scoreText.gameObject, Vector3.one, scoreAnimationDuration * 0.5f)
                    .setEase(LeanTweenType.easeInQuad);
            });
    }
    
    /// <summary>
    /// 최고 점수 텍스트 애니메이션
    /// </summary>
    private void AnimateHighScoreText()
    {
        if (highScoreText == null) return;
        
        Color originalColor = highScoreText.color;
        highScoreText.color = Color.yellow;
        
        LeanTween.value(highScoreText.gameObject, Color.yellow, originalColor, 1f)
            .setOnUpdate((Color color) =>
            {
                highScoreText.color = color;
            });
        
        LeanTween.scale(highScoreText.gameObject, Vector3.one * 1.1f, 0.3f)
            .setEase(LeanTweenType.easeOutQuad)
            .setLoopPingPong(1);
    }
    
    /// <summary>
    /// 현재 점수 반환
    /// </summary>
    public int GetCurrentScore()
    {
        return currentScore;
    }
    
    /// <summary>
    /// 최고 점수 반환
    /// </summary>
    public int GetHighScore()
    {
        return highScore;
    }
    
    /// <summary>
    /// 점수 초기화
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
        ResetCombo();
        UpdateScoreUI();
        Debug.Log("점수 초기화");
    }
    
    /// <summary>
    /// 최고 점수 저장
    /// </summary>
    public void SaveHighScore()
    {
        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save();
            Debug.Log($"최고 점수 저장: {currentScore}");
        }
    }
    
    /// <summary>
    /// 최고 점수 불러오기
    /// </summary>
    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreUI();
        Debug.Log($"최고 점수 로드: {highScore}");
    }
}
