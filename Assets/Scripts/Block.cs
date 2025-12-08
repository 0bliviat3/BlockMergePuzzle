using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 개별 블록을 나타내는 클래스 - 수정된 버전 (디버깅 강화)
/// </summary>
public class Block : MonoBehaviour
{
    [Header("블록 정보")]
    public int level = 1;           // 블록 레벨 (1, 2, 4, 8, 16, ...)
    public Vector2Int gridPosition; // 그리드 상의 위치
    
    [Header("UI 컴포넌트")]
    public Image blockImage;
    public TextMeshProUGUI levelText;
    
    [Header("애니메이션 설정")]
    public float mergeAnimationDuration = 0.3f;
    public float spawnAnimationDuration = 0.2f;
    
    private RectTransform rectTransform;
    private bool isAnimating = false;
    
    // 블록 레벨별 색상
    private static readonly Color[] levelColors = new Color[]
    {
        new Color(0.93f, 0.89f, 0.85f), // 레벨 1
        new Color(0.93f, 0.88f, 0.78f), // 레벨 2
        new Color(0.95f, 0.69f, 0.47f), // 레벨 3
        new Color(0.96f, 0.58f, 0.39f), // 레벨 4
        new Color(0.96f, 0.49f, 0.37f), // 레벨 5
        new Color(0.96f, 0.37f, 0.23f), // 레벨 6
        new Color(0.93f, 0.78f, 0.31f), // 레벨 7
        new Color(0.93f, 0.76f, 0.22f), // 레벨 8
        new Color(0.93f, 0.74f, 0.13f), // 레벨 9
        new Color(0.93f, 0.71f, 0.05f), // 레벨 10
        new Color(0.24f, 0.71f, 0.95f), // 레벨 11
    };
    
    private void Awake()
    {
        Debug.Log($"Block Awake 시작: {name}");
        
        rectTransform = GetComponent<RectTransform>();
        
        // 참조 자동 설정
        if (blockImage == null)
        {
            blockImage = GetComponent<Image>();
            if (blockImage != null)
            {
                Debug.Log($"Block {name}: blockImage 자동 찾기 성공");
            }
            else
            {
                Debug.LogError($"Block {name}: Image 컴포넌트를 찾을 수 없습니다!");
            }
        }
        
        if (levelText == null)
        {
            levelText = GetComponentInChildren<TextMeshProUGUI>();
            if (levelText != null)
            {
                Debug.Log($"Block {name}: levelText 자동 찾기 성공");
            }
            else
            {
                Debug.LogError($"Block {name}: TextMeshProUGUI를 찾을 수 없습니다!");
            }
        }
        
        // BoxCollider2D 확인
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            Debug.LogError($"Block {name}: BoxCollider2D가 없습니다! 터치가 작동하지 않을 수 있습니다.");
        }
        else
        {
            Debug.Log($"Block {name}: BoxCollider2D 확인 완료");
        }
    }
    
    /// <summary>
    /// 블록 초기화
    /// </summary>
    public void Initialize(int level, Vector2Int position)
    {
        this.level = level;
        this.gridPosition = position;
        
        Debug.Log($"블록 초기화: 레벨 {level}, 위치 {position}, 값 {GetBlockValue()}");
        
        UpdateVisuals();
        PlaySpawnAnimation();
    }
    
    /// <summary>
    /// 블록 외관 업데이트
    /// </summary>
    public void UpdateVisuals()
    {
        Debug.Log($"UpdateVisuals 시작: 레벨 {level}");
        
        // 레벨 텍스트 업데이트
        if (levelText != null)
        {
            levelText.text = GetBlockValue().ToString();
            Debug.Log($"텍스트 설정: {levelText.text}");
            
            // 텍스트 크기 조정
            if (GetBlockValue() >= 1000)
            {
                levelText.fontSize = 40;
            }
            else if (GetBlockValue() >= 100)
            {
                levelText.fontSize = 50;
            }
            else
            {
                levelText.fontSize = 60;
            }
        }
        else
        {
            Debug.LogError("levelText가 null입니다!");
        }
        
        // 색상 업데이트
        if (blockImage != null)
        {
            int colorIndex = Mathf.Min(level - 1, levelColors.Length - 1);
            blockImage.color = levelColors[colorIndex];
            Debug.Log($"색상 설정: {levelColors[colorIndex]}, 인덱스: {colorIndex}");
        }
        else
        {
            Debug.LogError("blockImage가 null입니다!");
        }
    }
    
    /// <summary>
    /// 블록의 실제 값 반환 (2의 거듭제곱)
    /// </summary>
    public int GetBlockValue()
    {
        return (int)Mathf.Pow(2, level);
    }
    
    /// <summary>
    /// 블록 레벨 업그레이드
    /// </summary>
    public void LevelUp()
    {
        level++;
        Debug.Log($"블록 레벨업: {level}, 값: {GetBlockValue()}");
        UpdateVisuals();
    }
    
    /// <summary>
    /// 스폰 애니메이션
    /// </summary>
    private void PlaySpawnAnimation()
    {
        transform.localScale = Vector3.zero;
        
        LeanTween.scale(gameObject, Vector3.one, spawnAnimationDuration)
                .setEase(LeanTweenType.easeOutBack);
    }
    
    /// <summary>
    /// 병합 애니메이션
    /// </summary>
    public void PlayMergeAnimation(Vector3 targetPosition, System.Action onComplete)
    {
        if (isAnimating) return;
        
        isAnimating = true;
        
        // 확대 후 축소 효과
        LeanTween.scale(gameObject, Vector3.one * 1.1f, mergeAnimationDuration * 0.5f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                LeanTween.scale(gameObject, Vector3.one, mergeAnimationDuration * 0.5f)
                    .setEase(LeanTweenType.easeInQuad);
            });
        
        // 위치 이동
        LeanTween.move(rectTransform, targetPosition, mergeAnimationDuration)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                isAnimating = false;
                onComplete?.Invoke();
            });
    }
    
    /// <summary>
    /// 폭발 애니메이션
    /// </summary>
    public void PlayExplodeAnimation(System.Action onComplete)
    {
        if (isAnimating) return;
        
        isAnimating = true;
        
        // 크기 확대 후 사라지기
        LeanTween.scale(gameObject, Vector3.one * 1.5f, 0.2f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                LeanTween.scale(gameObject, Vector3.zero, 0.2f)
                    .setEase(LeanTweenType.easeInQuad)
                    .setOnComplete(() =>
                    {
                        isAnimating = false;
                        onComplete?.Invoke();
                    });
            });
        
        // 회전 효과
        LeanTween.rotateZ(gameObject, 360f, 0.4f);
    }
    
    /// <summary>
    /// 이동 애니메이션
    /// </summary>
    public void MoveTo(Vector3 targetPosition, System.Action onComplete = null)
    {
        LeanTween.move(rectTransform, targetPosition, 0.2f)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnComplete(() => onComplete?.Invoke());
        
    }
}
