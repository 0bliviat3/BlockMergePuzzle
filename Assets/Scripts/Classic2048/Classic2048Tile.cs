using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 2048 개별 타일 클래스
/// </summary>
public class Classic2048Tile : MonoBehaviour
{
    [Header("타일 정보")]
    public int value = 2;
    public Vector2Int gridPosition;
    public bool hasMerged = false;
    
    [Header("UI 컴포넌트")]
    public Image backgroundImage;
    public Text valueText;
    
    private RectTransform rectTransform;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    /// <summary>
    /// 타일 초기화
    /// </summary>
    public void Initialize(int val, Vector2Int pos)
    {
        value = val;
        gridPosition = pos;
        hasMerged = false;
        
        // UI가 없으면 자동 생성
        if (backgroundImage == null)
        {
            CreateTileUI();
        }
        
        SetValue(val);
        PlaySpawnAnimation();
    }
    
    /// <summary>
    /// 타일 UI 자동 생성
    /// </summary>
    private void CreateTileUI()
    {
        // 배경 이미지
        backgroundImage = GetComponent<Image>();
        if (backgroundImage == null)
        {
            backgroundImage = gameObject.AddComponent<Image>();
        }
        
        // 텍스트
        GameObject textObj = new GameObject("ValueText");
        textObj.transform.SetParent(transform, false);
        
        valueText = textObj.AddComponent<Text>();
        valueText.alignment = TextAnchor.MiddleCenter;
        valueText.fontStyle = FontStyle.Bold;
        valueText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
    }
    
    /// <summary>
    /// 값 설정
    /// </summary>
    public void SetValue(int val)
    {
        value = val;
        
        if (valueText != null)
        {
            valueText.text = val.ToString();
        }
        
        UpdateAppearance();
    }
    
    /// <summary>
    /// 외형 업데이트
    /// </summary>
    private void UpdateAppearance()
    {
        if (backgroundImage == null || valueText == null) return;
        
        // 타일 색상
        Color tileColor = GetTileColor(value);
        backgroundImage.color = tileColor;
        
        // 텍스트 색상 (밝은 타일은 어두운 텍스트, 어두운 타일은 밝은 텍스트)
        valueText.color = value <= 4 ? new Color(0.47f, 0.43f, 0.40f) : Color.white;
        
        // 폰트 크기 (타일 크기 증가에 맞춰 조정)
        if (value < 100)
            valueText.fontSize = 95; // 72 → 95
        else if (value < 1000)
            valueText.fontSize = 78; // 60 → 78
        else
            valueText.fontSize = 62; // 48 → 62
    }
    
    /// <summary>
    /// 타일 색상 반환
    /// </summary>
    private Color GetTileColor(int val)
    {
        switch (val)
        {
            case 2: return new Color(0.93f, 0.89f, 0.85f);
            case 4: return new Color(0.93f, 0.88f, 0.78f);
            case 8: return new Color(0.95f, 0.69f, 0.47f);
            case 16: return new Color(0.96f, 0.58f, 0.39f);
            case 32: return new Color(0.96f, 0.49f, 0.37f);
            case 64: return new Color(0.96f, 0.37f, 0.23f);
            case 128: return new Color(0.93f, 0.81f, 0.45f);
            case 256: return new Color(0.93f, 0.80f, 0.38f);
            case 512: return new Color(0.93f, 0.78f, 0.31f);
            case 1024: return new Color(0.93f, 0.77f, 0.25f);
            case 2048: return new Color(0.93f, 0.76f, 0.18f);
            default: return new Color(0.20f, 0.20f, 0.20f);
        }
    }
    
    /// <summary>
    /// 스폰 애니메이션
    /// </summary>
    public void PlaySpawnAnimation()
    {
        if (rectTransform == null) return;
        
        rectTransform.localScale = Vector3.zero;
        LeanTween.scale(rectTransform, Vector3.one, 0.2f)
            .setEase(LeanTweenType.easeOutBack);
    }
    
    /// <summary>
    /// 병합 애니메이션
    /// </summary>
    public void PlayMergeAnimation()
    {
        if (rectTransform == null) return;
        
        // 이전 스케일 애니메이션 취소
        LeanTween.cancel(rectTransform.gameObject);
        
        LeanTween.scale(rectTransform, Vector3.one * 1.2f, 0.1f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                LeanTween.scale(rectTransform, Vector3.one, 0.1f)
                    .setEase(LeanTweenType.easeInQuad);
            });
    }
    
    /// <summary>
    /// 위치로 이동
    /// </summary>
    public void MoveTo(Vector2 targetPosition, float duration = 0.15f)
    {
        if (rectTransform == null) return;
        
        // 모든 이전 애니메이션 즉시 취소
        LeanTween.cancel(rectTransform.gameObject);
        LeanTween.cancel(gameObject);
        
        // 강제로 현재 애니메이션 정지
        rectTransform.anchoredPosition = rectTransform.anchoredPosition;
        
        // 새로운 애니메이션 시작
        LeanTween.move(rectTransform, targetPosition, duration)
            .setEase(LeanTweenType.easeOutQuad);
    }
}
