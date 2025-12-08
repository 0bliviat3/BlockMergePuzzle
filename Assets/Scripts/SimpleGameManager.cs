using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 간소화된 테스트용 GameManager
/// 문제 진단을 위한 최소 기능만 구현
/// </summary>
public class SimpleGameManager : MonoBehaviour
{
    [Header("필수 컴포넌트 (이것만 연결하면 됨)")]
    public Grid grid;
    
    [Header("디버그")]
    public TextMeshProUGUI debugText;
    
    private void Start()
    {
        Debug.Log("=== SimpleGameManager 시작 ===");
        
        if (debugText != null)
        {
            debugText.text = "SimpleGameManager 시작...\n";
        }
        
        // 1. Grid 확인
        if (grid == null)
        {
            LogError("Grid가 연결되지 않았습니다!");
            return;
        }
        
        LogSuccess("Grid 확인 완료");
        
        // 2. Grid 초기화
        try
        {
            grid.Initialize();
            LogSuccess("Grid 초기화 완료");
        }
        catch (System.Exception e)
        {
            LogError($"Grid 초기화 실패: {e.Message}");
            return;
        }
        
        // 3. 테스트 블록 생성
        for (int i = 0; i < 5; i++)
        {
            Block block = grid.AddRandomBlock(1);
            if (block != null)
            {
                LogSuccess($"블록 {i + 1} 생성 완료");
                
                // 블록 클릭 테스트 리스너 추가
                BoxCollider2D collider = block.GetComponent<BoxCollider2D>();
                if (collider != null)
                {
                    LogSuccess("블록에 Collider 있음");
                }
                else
                {
                    LogError("블록에 Collider 없음!");
                }
            }
            else
            {
                LogError($"블록 {i + 1} 생성 실패");
            }
        }
        
        LogSuccess("=== 초기화 완료 ===");
        
        // 4. 터치 테스트 시작
        InvokeRepeating("CheckTouch", 1f, 0.5f);
    }
    
    private void CheckTouch()
    {
        // 터치 감지
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                LogSuccess($"터치 감지! 위치: {touch.position}");
                
                // Raycast 테스트
                TestRaycast(touch.position);
            }
        }
        // PC 테스트용
        else if (Input.GetMouseButtonDown(0))
        {
            LogSuccess($"마우스 클릭 감지! 위치: {Input.mousePosition}");
            TestRaycast(Input.mousePosition);
        }
    }
    
    private void TestRaycast(Vector2 screenPos)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            LogError("Main Camera가 없습니다!");
            return;
        }
        
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        
        if (hit.collider != null)
        {
            LogSuccess($"Raycast 히트! 오브젝트: {hit.collider.gameObject.name}");
            
            Block block = hit.collider.GetComponent<Block>();
            if (block != null)
            {
                LogSuccess($"블록 클릭됨! 레벨: {block.level}");
                
                // 테스트: 블록 색상 변경
                if (block.blockImage != null)
                {
                    block.blockImage.color = Color.red;
                    LogSuccess("블록 색상 변경됨 (빨간색)");
                }
            }
        }
        else
        {
            LogError("Raycast에 아무것도 히트되지 않음!");
            
            // 레이어 마스크 확인
            int blockLayer = LayerMask.NameToLayer("BlockLayer");
            if (blockLayer == -1)
            {
                LogError("BlockLayer가 없습니다! Tags and Layers에서 추가하세요.");
            }
            else
            {
                LogSuccess($"BlockLayer 존재함: {blockLayer}");
            }
        }
    }
    
    private void LogSuccess(string message)
    {
        Debug.Log($"✓ {message}");
        if (debugText != null)
        {
            debugText.text += $"✓ {message}\n";
        }
    }
    
    private void LogError(string message)
    {
        Debug.LogError($"✗ {message}");
        if (debugText != null)
        {
            debugText.text += $"✗ {message}\n";
        }
    }
    
    private void Update()
    {
        // 화면에 현재 상태 표시
        if (debugText != null && grid != null)
        {
            int blockCount = grid.GetAllBlocks().Count;
            debugText.text = $"현재 블록 수: {blockCount}\n";
            debugText.text += $"터치 수: {Input.touchCount}\n";
            debugText.text += $"마우스 위치: {Input.mousePosition}\n";
            debugText.text += "\n화면을 터치해보세요!\n";
        }
    }
}
