using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

/// <summary>
/// 게임 디버깅 및 진단 도구 - 수정 버전 (NullReference 수정)
/// </summary>
public class GameDebugger : MonoBehaviour
{
    [Header("디버그 UI")]
    public TextMeshProUGUI debugText;
    public bool showDebugInfo = true;
    
    private StringBuilder debugLog = new StringBuilder();
    private GameManager gameManager;
    private Grid grid;
    private BlockMerger blockMerger;
    private InputHandler inputHandler;
    
    private void Start()
    {
        // 컴포넌트 찾기
        gameManager = FindObjectOfType<GameManager>();
        grid = FindObjectOfType<Grid>();
        blockMerger = FindObjectOfType<BlockMerger>();
        inputHandler = FindObjectOfType<InputHandler>();
        
        // 디버그 텍스트가 없으면 생성
        if (debugText == null)
        {
            CreateDebugUI();
        }
        
        // 초기 진단
        StartCoroutine(RunInitialDiagnostics());
    }
    
    private void Update()
    {
        if (showDebugInfo && debugText != null)
        {
            UpdateDebugInfo();
        }
        
        // 터치 디버깅
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            Vector3 inputPos = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
            LogDebug($"터치/클릭 감지: {inputPos}");
        }
    }
    
    /// <summary>
    /// 초기 진단 실행
    /// </summary>
    private System.Collections.IEnumerator RunInitialDiagnostics()
    {
        yield return new WaitForSeconds(0.5f);
        
        LogDebug("=== 게임 진단 시작 ===");
        
        // 1. 씬 구조 확인
        CheckSceneStructure();
        
        // 2. 컴포넌트 확인
        CheckComponents();
        
        // 3. 프리팹 확인
        CheckPrefabs();
        
        // 4. Canvas 설정 확인
        CheckCanvasSettings();
        
        // 5. EventSystem 확인
        CheckEventSystem();
        
        // 6. Camera 확인
        CheckCamera();
        
        // 7. Physics 설정 확인
        CheckPhysicsSettings();
        
        // 8. 블록 생성 확인
        CheckBlockGeneration();
        
        LogDebug("=== 진단 완료 ===");
        
        // 로그를 파일로 저장 (Android)
        SaveLogToFile();
    }
    
    /// <summary>
    /// 씬 구조 확인
    /// </summary>
    private void CheckSceneStructure()
    {
        LogDebug("\n[씬 구조 체크]");
        
        Canvas canvas = FindObjectOfType<Canvas>();
        LogDebug($"Canvas 존재: {canvas != null}");
        
        if (canvas != null)
        {
            Transform gridContainer = canvas.transform.Find("GridContainer");
            Transform blocksContainer = canvas.transform.Find("BlocksContainer");
            
            LogDebug($"GridContainer 존재: {gridContainer != null}");
            LogDebug($"BlocksContainer 존재: {blocksContainer != null}");
        }
    }
    
    /// <summary>
    /// 컴포넌트 확인
    /// </summary>
    private void CheckComponents()
    {
        LogDebug("\n[컴포넌트 체크]");
        
        LogDebug($"GameManager: {gameManager != null}");
        LogDebug($"Grid: {grid != null}");
        LogDebug($"BlockMerger: {blockMerger != null}");
        LogDebug($"InputHandler: {inputHandler != null}");
        
        if (gameManager != null)
        {
            LogDebug($"GameManager.isGameActive: {gameManager.isGameActive}");
        }
    }
    
    /// <summary>
    /// 프리팹 확인
    /// </summary>
    private void CheckPrefabs()
    {
        LogDebug("\n[프리팹 체크]");
        
        if (grid != null)
        {
            LogDebug($"Block Prefab: {grid.blockPrefab != null}");
            LogDebug($"Cell Prefab: {grid.cellPrefab != null}");
            
            if (grid.blockPrefab != null)
            {
                Block blockScript = grid.blockPrefab.GetComponent<Block>();
                LogDebug($"Block Prefab에 Block.cs: {blockScript != null}");
                
                BoxCollider2D collider = grid.blockPrefab.GetComponent<BoxCollider2D>();
                LogDebug($"Block Prefab에 BoxCollider2D: {collider != null}");
                
                if (collider != null)
                {
                    LogDebug($"BoxCollider2D Size: {collider.size}");
                }
            }
        }
    }
    
    /// <summary>
    /// Canvas 설정 확인
    /// </summary>
    private void CheckCanvasSettings()
    {
        LogDebug("\n[Canvas 설정 체크]");
        
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            LogDebug($"Render Mode: {canvas.renderMode}");
            
            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            if (scaler != null)
            {
                LogDebug($"UI Scale Mode: {scaler.uiScaleMode}");
                LogDebug($"Reference Resolution: {scaler.referenceResolution}");
            }
        }
    }
    
    /// <summary>
    /// EventSystem 확인
    /// </summary>
    private void CheckEventSystem()
    {
        LogDebug("\n[EventSystem 체크]");
        
        UnityEngine.EventSystems.EventSystem eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        LogDebug($"EventSystem 존재: {eventSystem != null}");
        
        if (eventSystem != null)
        {
            LogDebug($"EventSystem 활성화: {eventSystem.enabled}");
        }
        else
        {
            LogDebug("!!! EventSystem이 없습니다! UI → Event System을 추가하세요.");
        }
    }
    
    /// <summary>
    /// Camera 확인
    /// </summary>
    private void CheckCamera()
    {
        LogDebug("\n[Camera 체크]");
        
        Camera mainCamera = Camera.main;
        LogDebug($"Main Camera 존재: {mainCamera != null}");
        
        if (mainCamera != null)
        {
            LogDebug($"Camera Position: {mainCamera.transform.position}");
            LogDebug($"Camera Orthographic: {mainCamera.orthographic}");
            if (mainCamera.orthographic)
            {
                LogDebug($"Camera Size: {mainCamera.orthographicSize}");
            }
        }
        else
        {
            LogDebug("!!! Main Camera가 없습니다!");
            LogDebug("해결 방법 1: Hierarchy에 Camera가 있다면 Tag를 'MainCamera'로 설정");
            LogDebug("해결 방법 2: Hierarchy → Create → Camera 추가");
            
            // Camera 없이도 계속 진행
            Camera[] allCameras = FindObjectsOfType<Camera>();
            LogDebug($"씬의 모든 Camera 개수: {allCameras.Length}");
            foreach (Camera cam in allCameras)
            {
                LogDebug($"- Camera: {cam.name}, Tag: {cam.tag}");
            }
        }
    }
    
    /// <summary>
    /// Physics 설정 확인
    /// </summary>
    private void CheckPhysicsSettings()
    {
        LogDebug("\n[Physics 2D 설정]");
        LogDebug($"Gravity: {Physics2D.gravity}");
        
        // Camera가 있을 때만 Raycast 테스트
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Raycast 테스트
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            LogDebug($"중앙 Raycast 히트: {hit.collider != null}");
            if (hit.collider != null)
            {
                LogDebug($"히트된 오브젝트: {hit.collider.gameObject.name}");
            }
        }
        else
        {
            LogDebug("Main Camera가 없어 Raycast 테스트를 건너뜁니다.");
        }
        
        // Layer 확인
        int blockLayer = LayerMask.NameToLayer("BlockLayer");
        if (blockLayer == -1)
        {
            LogDebug("!!! BlockLayer가 없습니다!");
            LogDebug("Edit → Project Settings → Tags and Layers → User Layer 8에 'BlockLayer' 추가");
        }
        else
        {
            LogDebug($"BlockLayer 존재: Layer {blockLayer}");
        }
    }
    
    /// <summary>
    /// 블록 생성 확인
    /// </summary>
    private void CheckBlockGeneration()
    {
        LogDebug("\n[블록 생성 체크]");
        
        if (grid != null)
        {
            var blocks = grid.GetAllBlocks();
            LogDebug($"현재 블록 수: {blocks.Count}");
            
            if (blocks.Count > 0)
            {
                foreach (var block in blocks)
                {
                    LogDebug($"블록 - 레벨: {block.level}, 위치: {block.gridPosition}, 값: {block.GetBlockValue()}");
                    
                    // 블록 Layer 확인
                    LogDebug($"  Layer: {LayerMask.LayerToName(block.gameObject.layer)}");
                    
                    // Collider 확인
                    BoxCollider2D collider = block.GetComponent<BoxCollider2D>();
                    if (collider != null)
                    {
                        LogDebug($"  Collider Size: {collider.size}");
                    }
                    else
                    {
                        LogDebug($"  !!! Collider 없음");
                    }
                }
            }
            else
            {
                LogDebug("!!! 블록이 하나도 생성되지 않았습니다!");
                LogDebug("GameManager.StartNewGame()이 호출되었는지 확인하세요.");
            }
            
            var emptyPositions = grid.GetEmptyPositions();
            LogDebug($"빈 공간 수: {emptyPositions.Count}");
        }
        else
        {
            LogDebug("Grid가 null입니다!");
        }
    }
    
    /// <summary>
    /// 실시간 디버그 정보 업데이트
    /// </summary>
    private void UpdateDebugInfo()
    {
        StringBuilder info = new StringBuilder();
        
        info.AppendLine($"FPS: {(int)(1f / Time.deltaTime)}");
        info.AppendLine($"Screen: {Screen.width}x{Screen.height}");
        info.AppendLine($"Touch: {Input.touchCount}");
        
        Camera mainCamera = Camera.main;
        info.AppendLine($"Camera: {(mainCamera != null ? "OK" : "MISSING!")}");
        
        if (gameManager != null)
        {
            info.AppendLine($"Game Active: {gameManager.isGameActive}");
        }
        
        if (grid != null)
        {
            info.AppendLine($"Blocks: {grid.GetAllBlocks().Count}");
            info.AppendLine($"Empty: {grid.GetEmptyPositions().Count}");
        }
        
        debugText.text = info.ToString();
    }
    
    /// <summary>
    /// 디버그 UI 생성
    /// </summary>
    private void CreateDebugUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            LogDebug("Canvas를 찾을 수 없어 디버그 UI를 생성할 수 없습니다.");
            return;
        }
        
        GameObject debugPanel = new GameObject("DebugPanel");
        debugPanel.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = debugPanel.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(10, -10);
        rect.sizeDelta = new Vector2(400, 300);
        
        Image bg = debugPanel.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.7f);
        
        GameObject textObj = new GameObject("DebugText");
        textObj.transform.SetParent(debugPanel.transform, false);
        
        debugText = textObj.AddComponent<TextMeshProUGUI>();
        debugText.fontSize = 18;
        debugText.color = Color.white;
        debugText.alignment = TextAlignmentOptions.TopLeft;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(5, 5);
        textRect.offsetMax = new Vector2(-5, -5);
    }
    
    /// <summary>
    /// 디버그 로그 추가
    /// </summary>
    private void LogDebug(string message)
    {
        Debug.Log(message);
        debugLog.AppendLine(message);
    }
    
    /// <summary>
    /// 로그를 파일로 저장
    /// </summary>
    private void SaveLogToFile()
    {
        try
        {
            string path = Application.persistentDataPath + "/debug_log.txt";
            System.IO.File.WriteAllText(path, debugLog.ToString());
            LogDebug($"\n로그 저장됨: {path}");
            LogDebug("Android: 설정 → 앱 → Block Merge Puzzle → 저장공간 → 파일 탐색으로 확인 가능");
        }
        catch (System.Exception e)
        {
            LogDebug($"로그 저장 실패: {e.Message}");
        }
    }
    
    /// <summary>
    /// 강제 블록 생성 (테스트용)
    /// </summary>
    [ContextMenu("Force Add Block")]
    public void ForceAddBlock()
    {
        if (grid != null)
        {
            Block block = grid.AddRandomBlock(1);
            if (block != null)
            {
                LogDebug("블록 강제 생성 성공");
            }
            else
            {
                LogDebug("블록 강제 생성 실패");
            }
        }
    }
}
