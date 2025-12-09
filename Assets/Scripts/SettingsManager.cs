using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 설정 패널 관리 클래스 - 아이콘 개선 버전
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("UI 참조")]
    public GameObject settingsPanel;
    public Button closeButton;
    public Button quitButton;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public TextMeshProUGUI bgmValueText;
    public TextMeshProUGUI sfxValueText;
    
    [Header("자동 생성 설정")]
    public bool autoCreateUI = true;

    public static SettingsManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        Debug.Log("=== SettingsManager Start ===");
        
        // UI 자동 생성
        if (autoCreateUI && settingsPanel == null)
        {
            CreateSettingsUI();
        }
        
        // UI 초기화
        InitializeUI();
    }
    
    /// <summary>
    /// 설정 UI 자동 생성
    /// </summary>
    private void CreateSettingsUI()
    {
        Debug.Log("설정 UI 자동 생성 시작...");
        
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas를 찾을 수 없습니다!");
            return;
        }
        
        // ===== 설정 패널 (팝업) =====
        GameObject panelObj = new GameObject("SettingsPanel");
        panelObj.transform.SetParent(canvas.transform, false);
        
        RectTransform panelRect = panelObj.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;
        
        // 어두운 배경
        Image panelBg = panelObj.AddComponent<Image>();
        panelBg.color = new Color(0, 0, 0, 0.8f);
        
        // 중앙 패널
        GameObject centerPanelObj = new GameObject("CenterPanel");
        centerPanelObj.transform.SetParent(panelObj.transform, false);
        
        RectTransform centerPanelRect = centerPanelObj.AddComponent<RectTransform>();
        centerPanelRect.anchorMin = new Vector2(0.5f, 0.5f);
        centerPanelRect.anchorMax = new Vector2(0.5f, 0.5f);
        centerPanelRect.pivot = new Vector2(0.5f, 0.5f);
        centerPanelRect.sizeDelta = new Vector2(600, 800);
        
        Image centerPanelBg = centerPanelObj.AddComponent<Image>();
        centerPanelBg.color = new Color(0.15f, 0.15f, 0.15f, 1f);
        
        // 수직 레이아웃
        VerticalLayoutGroup layout = centerPanelObj.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(40, 40, 40, 40);
        layout.spacing = 30;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;
        
        // 제목
        GameObject titleObj = CreateText(centerPanelObj.transform, "Settings", 48, TextAlignmentOptions.Center);
        LayoutElement titleLayout = titleObj.AddComponent<LayoutElement>();
        titleLayout.preferredHeight = 80;
        
        // BGM 슬라이더
        GameObject bgmGroupObj = CreateSliderGroup(centerPanelObj.transform, "BGM Volume", out Slider bgmSliderComp, out TextMeshProUGUI bgmTextComp);
        bgmSlider = bgmSliderComp;
        bgmValueText = bgmTextComp;
        
        // SFX 슬라이더
        GameObject sfxGroupObj = CreateSliderGroup(centerPanelObj.transform, "SFX Volume", out Slider sfxSliderComp, out TextMeshProUGUI sfxTextComp);
        sfxSlider = sfxSliderComp;
        sfxValueText = sfxTextComp;
        
        // 공간
        GameObject spacerObj = new GameObject("Spacer");
        spacerObj.transform.SetParent(centerPanelObj.transform, false);
        LayoutElement spacerLayout = spacerObj.AddComponent<LayoutElement>();
        spacerLayout.preferredHeight = 50;
        
        // 게임 종료 버튼
        GameObject quitButtonObj = CreateButton(centerPanelObj.transform, "Quit Game", Color.red);
        quitButton = quitButtonObj.GetComponent<Button>();
        
        // 닫기 버튼
        GameObject closeButtonObj = CreateButton(centerPanelObj.transform, "Close", new Color(0.3f, 0.6f, 1f));
        closeButton = closeButtonObj.GetComponent<Button>();
        
        // 참조 저장
        settingsPanel = panelObj;
        settingsPanel.SetActive(false);
        
        Debug.Log("✓ 설정 UI 자동 생성 완료");
    }
    
    /// <summary>
    /// 텍스트 생성
    /// </summary>
    private GameObject CreateText(Transform parent, string text, float fontSize, TextAlignmentOptions alignment)
    {
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(parent, false);
        
        TextMeshProUGUI textComp = textObj.AddComponent<TextMeshProUGUI>();
        textComp.text = text;
        textComp.fontSize = fontSize;
        textComp.alignment = alignment;
        textComp.color = Color.white;
        
        return textObj;
    }
    
    /// <summary>
    /// 슬라이더 그룹 생성
    /// </summary>
    private GameObject CreateSliderGroup(Transform parent, string label, out Slider slider, out TextMeshProUGUI valueText)
    {
        GameObject groupObj = new GameObject("SliderGroup_" + label);
        groupObj.transform.SetParent(parent, false);
        
        LayoutElement groupLayout = groupObj.AddComponent<LayoutElement>();
        groupLayout.preferredHeight = 120;
        
        // 레이블
        GameObject labelObj = CreateText(groupObj.transform, label, 32, TextAlignmentOptions.Left);
        RectTransform labelRect = labelObj.GetComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0, 1);
        labelRect.anchorMax = new Vector2(1, 1);
        labelRect.pivot = new Vector2(0.5f, 1);
        labelRect.anchoredPosition = new Vector2(0, 30);
        labelRect.sizeDelta = new Vector2(0, 50);
        
        // 슬라이더
        GameObject sliderObj = new GameObject("Slider");
        sliderObj.transform.SetParent(groupObj.transform, false);
        
        RectTransform sliderRect = sliderObj.AddComponent<RectTransform>();
        sliderRect.anchorMin = new Vector2(0, 0);
        sliderRect.anchorMax = new Vector2(0.75f, 0);  // 0.8 → 0.75 (슬라이더 영역 축소)
        sliderRect.pivot = new Vector2(0, 0);
        sliderRect.anchoredPosition = new Vector2(0, 10);
        sliderRect.sizeDelta = new Vector2(0, 50);
        
        Slider sliderComp = sliderObj.AddComponent<Slider>();
        sliderComp.minValue = 0f;
        sliderComp.maxValue = 1f;
        sliderComp.value = 0.5f;
        
        // 슬라이더 배경
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(sliderObj.transform, false);
        RectTransform bgRect = bgObj.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0.3f, 0.3f, 0.3f);
        
        // 슬라이더 핸들 영역
        GameObject handleAreaObj = new GameObject("Handle Slide Area");
        handleAreaObj.transform.SetParent(sliderObj.transform, false);
        RectTransform handleAreaRect = handleAreaObj.AddComponent<RectTransform>();
        handleAreaRect.anchorMin = Vector2.zero;
        handleAreaRect.anchorMax = Vector2.one;
        handleAreaRect.sizeDelta = new Vector2(-20, 0);
        
        // 슬라이더 핸들
        GameObject handleObj = new GameObject("Handle");
        handleObj.transform.SetParent(handleAreaObj.transform, false);
        RectTransform handleRect = handleObj.AddComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(40, 50);
        Image handleImage = handleObj.AddComponent<Image>();
        handleImage.color = Color.white;
        
        sliderComp.targetGraphic = handleImage;
        sliderComp.handleRect = handleRect;
        
        // 값 텍스트
        GameObject valueObj = CreateText(groupObj.transform, "50%", 28, TextAlignmentOptions.Center);
        RectTransform valueRect = valueObj.GetComponent<RectTransform>();
        valueRect.anchorMin = new Vector2(0.78f, 0);  // 0.85 → 0.78 (왼쪽으로 시작)
        valueRect.anchorMax = new Vector2(1f, 0);  // 1.15 → 1 (오른쪽 끝까지)
        valueRect.pivot = new Vector2(0, 0);  // 0.5 → 0 (왼쪽 기준)
        valueRect.anchoredPosition = new Vector2(10, 10);
        valueRect.sizeDelta = new Vector2(0, 50);
        
        slider = sliderComp;
        valueText = valueObj.GetComponent<TextMeshProUGUI>();
        
        return groupObj;
    }
    
    /// <summary>
    /// 버튼 생성
    /// </summary>
    private GameObject CreateButton(Transform parent, string text, Color color)
    {
        GameObject buttonObj = new GameObject("Button_" + text);
        buttonObj.transform.SetParent(parent, false);
        
        LayoutElement buttonLayout = buttonObj.AddComponent<LayoutElement>();
        buttonLayout.preferredHeight = 100;
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = color;
        
        Button buttonComp = buttonObj.AddComponent<Button>();
        
        GameObject textObj = CreateText(buttonObj.transform, text, 36, TextAlignmentOptions.Center);
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        return buttonObj;
    }
    
    /// <summary>
    /// UI 초기화
    /// </summary>
    private void InitializeUI()
    {
        if (settingsPanel == null)
        {
            Debug.LogError("설정 패널이 없습니다!");
            return;
        }
        
        // 버튼 리스너
        
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseSettings);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        
        // 슬라이더 리스너
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
            
            // 초기값 설정
            if (AudioManager.Instance != null)
            {
                bgmSlider.value = AudioManager.Instance.GetBGMVolume();
                UpdateBGMValueText(bgmSlider.value);
            }
        }
        
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            
            // 초기값 설정
            if (AudioManager.Instance != null)
            {
                sfxSlider.value = AudioManager.Instance.GetSFXVolume();
                UpdateSFXValueText(sfxSlider.value);
            }
        }
        
        settingsPanel.SetActive(false);
        
        Debug.Log("✓ 설정 UI 초기화 완료");
    }
    
    /// <summary>
    /// 설정 열기
    /// </summary>
    public void OpenSettings()
    {
        // ⭐ 클릭 사운드 재생
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayClickSound();
        }
        
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("⚙️ 설정 패널 열림");
        }
    }
    
    /// <summary>
    /// 설정 닫기
    /// </summary>
    public void CloseSettings()
    {
        // ⭐ 클릭 사운드 재생
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayClickSound();
        }
        
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            Time.timeScale = 1f;
            Debug.Log("⚙️ 설정 패널 닫힘");
        }
    }
    
    /// <summary>
    /// 게임 종료
    /// </summary>
    public void QuitGame()
    {
        // ⭐ 클릭 사운드 재생
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayClickSound();
        }
        
        Debug.Log("🚪 게임 종료");
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    /// <summary>
    /// BGM 볼륨 변경
    /// </summary>
    private void OnBGMVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(value);
        }
        UpdateBGMValueText(value);
    }
    
    /// <summary>
    /// SFX 볼륨 변경
    /// </summary>
    private void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
        UpdateSFXValueText(value);
    }
    
    /// <summary>
    /// BGM 값 텍스트 업데이트
    /// </summary>
    private void UpdateBGMValueText(float value)
    {
        if (bgmValueText != null)
        {
            bgmValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
        }
    }
    
    /// <summary>
    /// SFX 값 텍스트 업데이트
    /// </summary>
    private void UpdateSFXValueText(float value)
    {
        if (sfxValueText != null)
        {
            sfxValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
        }
    }
}
