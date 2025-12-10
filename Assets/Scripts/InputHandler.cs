using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 입력 처리 클래스 - 수정 버전 (BlockMerger와 호환)
/// </summary>
public class InputHandler : MonoBehaviour
{
    [Header("참조")]
    public BlockMerger blockMerger;
    
    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;
    
    private void Start()
    {
        Debug.Log("=== InputHandler Start ===");
        
        // 모든 Canvas 찾기
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        Debug.Log($"씬에 있는 Canvas 개수: {allCanvases.Length}");
        
        Canvas canvas = null;
        foreach (Canvas c in allCanvases)
        {
            Debug.Log($"Canvas 발견: {c.name}");
            Debug.Log($"  - Render Mode: {c.renderMode}");
            Debug.Log($"  - Sort Order: {c.sortingOrder}");
            
            GraphicRaycaster gr = c.GetComponent<GraphicRaycaster>();
            Debug.Log($"  - GraphicRaycaster: {(gr != null ? "있음" : "없음")}");
            
            // GameCanvas 또는 첫 번째 Canvas 사용
            if (c.name == "Canvas" || c.name.Contains("Game"))
            {
                canvas = c;
                Debug.Log($"  ✓ 이 Canvas를 사용합니다: {c.name}");
            }
        }
        
        // Canvas가 없으면 첫 번째 것 사용
        if (canvas == null && allCanvases.Length > 0)
        {
            canvas = allCanvases[0];
            Debug.Log($"기본 Canvas 사용: {canvas.name}");
        }
        
        if (canvas != null)
        {
            graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
            if (graphicRaycaster == null)
            {
                Debug.LogWarning("Canvas에 GraphicRaycaster가 없습니다! 자동으로 추가합니다.");
                graphicRaycaster = canvas.gameObject.AddComponent<GraphicRaycaster>();
                Debug.Log("✓ GraphicRaycaster 추가됨");
            }
            else
            {
                Debug.Log("✓ GraphicRaycaster 찾음");
            }
            
            // GraphicRaycaster 설정 확인
            Debug.Log($"GraphicRaycaster 설정:");
            Debug.Log($"  - Ignore Reversed Graphics: {graphicRaycaster.ignoreReversedGraphics}");
            Debug.Log($"  - Blocking Objects: {graphicRaycaster.blockingObjects}");
        }
        else
        {
            Debug.LogError("Canvas를 찾을 수 없습니다!");
        }
        
        // EventSystem 찾기
        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            Debug.LogWarning("EventSystem이 없습니다! 자동으로 생성합니다.");
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystem = eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();
            Debug.Log("✓ EventSystem 생성됨");
        }
        else
        {
            Debug.Log("✓ EventSystem 찾음");
        }
        
        if (blockMerger == null)
        {
            Debug.LogError("BlockMerger가 연결되지 않았습니다!");
        }
        else
        {
            Debug.Log("✓ BlockMerger 연결됨");
        }
    }
    
    private void Update()
    {
        // 터치 입력 처리
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleInput(touch.position);
            }
        }
        // PC 테스트용 마우스 입력
        else if (Input.GetMouseButtonDown(0))
        {
            HandleInput(Input.mousePosition);
        }
    }
    
    /// <summary>
    /// 입력 처리
    /// </summary>
    private void HandleInput(Vector2 screenPosition)
    {
        Debug.Log($"[InputHandler] 입력 감지: {screenPosition}");
        
        if (graphicRaycaster == null || eventSystem == null)
        {
            Debug.LogError("GraphicRaycaster 또는 EventSystem이 없습니다!");
            return;
        }
        
        // UI 버튼 등 클릭 무시
        if (IsPointerOverUIButton(screenPosition))
        {
            Debug.Log("UI 버튼 클릭됨 - 블록 선택 무시");
            return;
        }
        
        // 블록 찾기
        Block clickedBlock = GetBlockAtPosition(screenPosition);
        
        if (clickedBlock != null)
        {
            Debug.Log($"블록 클릭됨: 레벨 {clickedBlock.level}, 위치 {clickedBlock.gridPosition}");
            
            if (blockMerger != null)
            {
                // BlockMerger의 SelectBlock 함수 호출
                blockMerger.SelectBlock(clickedBlock);
            }
        }
        else
        {
            Debug.Log("블록이 클릭되지 않음");
        }
    }
    
    /// <summary>
    /// UI Raycast로 블록 찾기
    /// </summary>
    private Block GetBlockAtPosition(Vector2 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = screenPosition
        };
        
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerData, results);
        
        Debug.Log($"Raycast 결과: {results.Count}개 오브젝트");
        
        foreach (RaycastResult result in results)
        {
            Debug.Log($"- 히트: {result.gameObject.name}, Layer: {LayerMask.LayerToName(result.gameObject.layer)}");
            
            // Image 컴포넌트 확인
            Image img = result.gameObject.GetComponent<Image>();
            if (img != null)
            {
                Debug.Log($"  → Image 발견, raycastTarget: {img.raycastTarget}");
            }
            
            // Block 컴포넌트 찾기
            Block block = result.gameObject.GetComponent<Block>();
            if (block != null)
            {
                Debug.Log($"✓ Block 컴포넌트 찾음! 레벨: {block.level}");
                return block;
            }
            else
            {
                Debug.Log($"  → Block 컴포넌트 없음");
            }
        }
        
        return null;
    }
    
    /// <summary>
    /// UI 버튼 위에 있는지 확인
    /// </summary>
    private bool IsPointerOverUIButton(Vector2 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = screenPosition
        };
        
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerData, results);
        
        foreach (RaycastResult result in results)
        {
            // Button이나 Selectable 컴포넌트 확인
            if (result.gameObject.GetComponent<UnityEngine.UI.Button>() != null ||
                result.gameObject.GetComponent<UnityEngine.UI.Selectable>() != null)
            {
                return true;
            }
        }
        
        return false;
    }
}
