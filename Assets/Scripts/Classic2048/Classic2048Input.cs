using UnityEngine;

/// <summary>
/// 2048 입력 처리 클래스
/// </summary>
public class Classic2048Input : MonoBehaviour
{
    [Header("스와이프 설정")]
    public float minSwipeDistance = 50f;
    
    [Header("디버그")]
    public bool enableKeyboardDebug = false; // 키보드 디버그 활성화 (기본: 비활성)
    
    private Vector2 touchStartPos;
    private bool isSwiping = false;
    
    private void Update()
    {
        HandleInput();
    }
    
    /// <summary>
    /// 입력 처리
    /// </summary>
    private void HandleInput()
    {
        // 터치 입력 (모바일)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                ProcessSwipe(touchStartPos, touch.position);
                isSwiping = false;
            }
        }
        // 마우스 입력 (PC 스와이프)
        else if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            ProcessSwipe(touchStartPos, Input.mousePosition);
            isSwiping = false;
        }
        
        // 키보드 입력 (디버그 전용, 기본 비활성)
        if (enableKeyboardDebug)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                OnSwipe(Vector2Int.up);
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                OnSwipe(Vector2Int.down);
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                OnSwipe(Vector2Int.left);
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                OnSwipe(Vector2Int.right);
        }
    }
    
    /// <summary>
    /// 스와이프 처리
    /// </summary>
    private void ProcessSwipe(Vector2 startPos, Vector2 endPos)
    {
        Vector2 swipe = endPos - startPos;
        
        if (swipe.magnitude < minSwipeDistance)
            return;
        
        swipe.Normalize();
        
        // 수평/수직 방향 결정
        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            // 수평
            OnSwipe(swipe.x > 0 ? Vector2Int.right : Vector2Int.left);
        }
        else
        {
            // 수직
            OnSwipe(swipe.y > 0 ? Vector2Int.up : Vector2Int.down);
        }
    }
    
    /// <summary>
    /// 스와이프 이벤트
    /// </summary>
    private void OnSwipe(Vector2Int direction)
    {
        string directionName = direction == Vector2Int.up ? "위" :
                              direction == Vector2Int.down ? "아래" :
                              direction == Vector2Int.left ? "왼쪽" : "오른쪽";
        
        Debug.Log($"👆 스와이프: {directionName}");
        
        if (Classic2048Manager.Instance != null)
        {
            Classic2048Manager.Instance.OnSwipe(direction);
        }
    }
}
