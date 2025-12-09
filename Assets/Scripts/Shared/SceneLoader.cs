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
        
        Canvas canvas = fadeObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999;
        
        fadeObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        fadeObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        GameObject panel = new GameObject("FadePanel");
        panel.transform.SetParent(fadeObj.transform);
        
        UnityEngine.UI.Image image = panel.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.black;
        
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        
        fadeCanvasGroup = panel.AddComponent<CanvasGroup>();
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
        
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
        
        // Fade Out
        yield return StartCoroutine(Fade(1f));
        
        // 씬 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        Debug.Log($"✓ 씬 로드 완료: {sceneName}");
        
        // 짧은 대기
        yield return new WaitForSeconds(0.1f);
        
        // Fade In
        yield return StartCoroutine(Fade(0f));
        
        isTransitioning = false;
    }
    
    /// <summary>
    /// 페이드 효과
    /// </summary>
    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeCanvasGroup == null) yield break;
        
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
