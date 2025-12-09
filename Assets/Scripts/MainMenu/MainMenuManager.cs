using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 메인 메뉴 관리 클래스
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("자동 UI 생성")]
    public bool autoCreateUI = true;
    
    [Header("BGM")]
    public AudioClip menuBGM;
    
    private Canvas canvas;
    
    private void Start()
    {
        Debug.Log("=== MainMenuManager Start ===");
        
        // BGM 재생
        if (AudioManager.Instance != null && menuBGM != null)
        {
            AudioManager.Instance.StopBGM();
            AudioManager.Instance.bgmClip = menuBGM;
            AudioManager.Instance.PlayBGM();
        }
        
        if (autoCreateUI)
        {
            CreateMainMenuUI();
        }
    }
    
    private void CreateMainMenuUI()
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
        GameObject canvasObj = new GameObject("MenuCanvas");
        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // 배경
        CreateBackground();
        
        // 타이틀
        CreateTitle();
        
        // 게임 버튼들
        CreateGameButtons();
        
        // 설정 버튼
        CreateSettingsButton();
        
        Debug.Log("✓ 메인 메뉴 UI 생성 완료");
    }
    
    private void CreateBackground()
    {
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(canvas.transform, false);
        
        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0.17f, 0.24f, 0.31f);
        
        RectTransform bgRect = bgObj.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
    }
    
    private void CreateTitle()
    {
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(canvas.transform, false);
        
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "2048 COLLECTION";
        titleText.fontSize = 72;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(1f, 0.84f, 0f);
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.sizeDelta = new Vector2(800, 120);
        titleRect.anchoredPosition = new Vector2(0, -150);
    }
    
    private void CreateGameButtons()
    {
        // Block Merge Puzzle 버튼
        CreateGameButton(
            "Block Merge Puzzle",
            "인접한 같은 숫자 블록을 병합하세요!",
            new Vector2(0, 100),
            () => LoadGame("BlockMerge")
        );
        
        // Classic 2048 버튼
        CreateGameButton(
            "Classic 2048",
            "오리지널 2048 게임을 플레이하세요!",
            new Vector2(0, -100),
            () => LoadGame("Classic2048")
        );
    }
    
    private void CreateGameButton(string gameName, string description, Vector2 position, UnityEngine.Events.UnityAction onClick)
    {
        GameObject buttonObj = new GameObject(gameName + "_Button");
        buttonObj.transform.SetParent(canvas.transform, false);
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.25f, 0.35f, 0.45f);
        
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(onClick);
        button.onClick.AddListener(() => 
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayClickSound();
        });
        
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.sizeDelta = new Vector2(700, 180);
        buttonRect.anchoredPosition = position;
        
        // 게임 이름 텍스트
        GameObject nameObj = new GameObject("GameName");
        nameObj.transform.SetParent(buttonObj.transform, false);
        
        Text nameText = nameObj.AddComponent<Text>();
        nameText.text = gameName;
        nameText.fontSize = 48;
        nameText.fontStyle = FontStyle.Bold;
        nameText.alignment = TextAnchor.MiddleCenter;
        nameText.color = Color.white;
        nameText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        RectTransform nameRect = nameObj.GetComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0.5f, 0.5f);
        nameRect.anchorMax = new Vector2(0.5f, 0.5f);
        nameRect.sizeDelta = new Vector2(650, 60);
        nameRect.anchoredPosition = new Vector2(0, 30);
        
        // 설명 텍스트
        GameObject descObj = new GameObject("Description");
        descObj.transform.SetParent(buttonObj.transform, false);
        
        Text descText = descObj.AddComponent<Text>();
        descText.text = description;
        descText.fontSize = 28;
        descText.alignment = TextAnchor.MiddleCenter;
        descText.color = new Color(0.8f, 0.8f, 0.8f);
        descText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        RectTransform descRect = descObj.GetComponent<RectTransform>();
        descRect.anchorMin = new Vector2(0.5f, 0.5f);
        descRect.anchorMax = new Vector2(0.5f, 0.5f);
        descRect.sizeDelta = new Vector2(650, 40);
        descRect.anchoredPosition = new Vector2(0, -30);
    }
    
    private void CreateSettingsButton()
    {
        GameObject buttonObj = new GameObject("SettingsButton");
        buttonObj.transform.SetParent(canvas.transform, false);
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.35f, 0.45f, 0.55f);
        
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(() => OpenSettings());
        button.onClick.AddListener(() => 
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayClickSound();
        });
        
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0);
        buttonRect.anchorMax = new Vector2(0.5f, 0);
        buttonRect.sizeDelta = new Vector2(400, 100);
        buttonRect.anchoredPosition = new Vector2(0, 100);
        
        // 텍스트
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        Text text = textObj.AddComponent<Text>();
        text.text = "SETTINGS";
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
    
    private void LoadGame(string sceneName)
    {
        Debug.Log($"📂 게임 로드: {sceneName}");
        
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene(sceneName);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
    
    private void OpenSettings()
    {
        SettingsManager.Instance.OpenSettings();
    }
}
