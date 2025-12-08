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
        
        // GraphicRaycaster 찾기
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
            if (graphicRaycaster == null)
            {
                Debug.LogError("Canvas에 GraphicRaycaster가 없습니다!");
            }
            else
            {
                Debug.Log("✓ GraphicRaycaster 찾음");
            }
        }
        
        // EventSystem 찾기
        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            Debug.LogError("EventSystem이 없습니다!");
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
            
            // Block 컴포넌트 찾기
            Block block = result.gameObject.GetComponent<Block>();
            if (block != null)
            {
                Debug.Log($"✓ Block 컴포넌트 찾음!");
                return block;
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
