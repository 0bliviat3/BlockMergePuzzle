using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 씬 전환 관리 클래스
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    
    [Header("페이드 설정")]
    public float fadeDuration = 0.5f;
    
    private CanvasGroup fadeCanvasGroup;
    private GameObject fadeCanvas; // Fade Canvas 저장 변수 추가
    private bool isTransitioning = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateFadeCanvas();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 페이드 캔버스 생성
    /// </summary>
    private void CreateFadeCanvas()
    {
        GameObject fadeObj = new GameObject("FadeCanvas");
        fadeObj.transform.SetParent(transform);
        DontDestroyOnLoad(fadeObj); // FadeCanvas도 유지
        
        fadeCanvas = fadeObj; // 변수에 저장!
        
        Canvas canvas = fadeObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // 최상위
        
        fadeObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        fadeObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        GameObject panel = new GameObject("FadePanel");
        panel.transform.SetParent(fadeObj.transform);
        
        UnityEngine.UI.Image image = panel.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.black; // 검은색
        
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero; // 중앙 정렬
        
        fadeCanvasGroup = panel.AddComponent<CanvasGroup>();
        fadeCanvasGroup.alpha = 0f; // 시작은 투명
        fadeCanvasGroup.blocksRaycasts = false; // 입력 차단 해제
        
        Debug.Log("✓ Fade Canvas 생성 완료");
    }
    
    /// <summary>
    /// 씬 로드
    /// </summary>
    public void LoadScene(string sceneName)
    {
        if (!isTransitioning)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }
    }
    
    /// <summary>
    /// 씬 로드 코루틴
    /// </summary>
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        isTransitioning = true;
        
        Debug.Log($"🔄 씬 전환 시작: {sceneName}");
        
        // Fade Out
        yield return StartCoroutine(Fade(1f));
        
        // 씬 로드 전에 모든 Canvas 확인 및 정리
        Debug.Log("=== 씬 전환 전 Canvas 목록 ===");
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        Debug.Log($"현재 Canvas 개수: {allCanvases.Length}");
        
        foreach (Canvas c in allCanvases)
        {
            Debug.Log($"Canvas 발견: {c.gameObject.name} (씬: {c.gameObject.scene.name}, sortOrder: {c.sortingOrder})");
            
            // FadeCanvas는 유지
            if (c.gameObject == fadeCanvas)
            {
                Debug.Log($"  → FadeCanvas 유지");
                continue;
            }
            
            // DontDestroyOnLoad가 아닌 Canvas만 삭제
            if (c.gameObject.scene.isLoaded)
            {
                Debug.Log($"  → 삭제 예정: {c.gameObject.name}");
                Destroy(c.gameObject);
            }
            else
            {
                Debug.Log($"  → DontDestroyOnLoad 오브젝트 - 유지");
            }
        }
        
        // 잠시 대기 (Canvas 삭제 완료)
        yield return null;
        
        // 씬 로드
        Debug.Log($"▶ 씬 로드 시작: {sceneName}");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        Debug.Log($"✓ 씬 로드 완료: {sceneName}");
        
        // 씬 로드 직후 깜빡임 방지 — CanvasScaler 초기화 기다림
        yield return null;  // 1프레임 대기
        yield return null;  // 1프레임 더 대기 (중요)

        asyncLoad.allowSceneActivation = true;

        // 씬 로드 완료 후 1프레임 대기
        yield return null;
        
        Debug.Log("=== 씬 전환 후 Canvas 목록 ===");
        allCanvases = FindObjectsOfType<Canvas>();
        Debug.Log($"현재 Canvas 개수: {allCanvases.Length}");
        foreach (Canvas c in allCanvases)
        {
            CanvasGroup cg = c.GetComponent<CanvasGroup>();
            string alphaInfo = cg != null ? cg.alpha.ToString("F2") : "없음";
            Debug.Log($"Canvas: {c.gameObject.name} (sortOrder: {c.sortingOrder}, CanvasGroup alpha: {alphaInfo})");
        }
        
        // Fade In
        yield return StartCoroutine(Fade(0f));
        
        // Fade 완료 후 raycast 차단 해제
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = false;
        }
        
        Debug.Log($"✅ 씬 전환 완료: {sceneName}");
        isTransitioning = false;
    }
    
    /// <summary>
    /// 페이드 효과
    /// </summary>
    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeCanvasGroup == null) yield break;
        
        // Fade 시작 전 활성화
        if (fadeCanvasGroup.gameObject != null)
        {
            fadeCanvasGroup.gameObject.SetActive(true);
        }
        
        fadeCanvasGroup.blocksRaycasts = targetAlpha > 0.5f;
        
        float startAlpha = fadeCanvasGroup.alpha;
        float elapsed = 0f;
        
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / fadeDuration;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }
        
        fadeCanvasGroup.alpha = targetAlpha;
        
        // Fade In 완료 후 패널 비활성화 (alpha = 0)
        if (targetAlpha <= 0f && fadeCanvasGroup.gameObject != null)
        {
            // 완전히 투명하면 비활성화하지 않고 alpha만 0으로 유지
            // (다음 Fade Out을 위해)
            Debug.Log("✓ Fade In 완료 - 패널 투명 상태 유지");
        }
    }
    
    /// <summary>
    /// 즉시 페이드 아웃 (앱 시작 시)
    /// </summary>
    public void FadeOutImmediate()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
            StartCoroutine(Fade(0f));
        }
    }
}
