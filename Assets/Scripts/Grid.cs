using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// 게임 그리드 관리 클래스 - 수정 버전 (로그 강화)
/// </summary>
public class Grid : MonoBehaviour
{
    [Header("그리드 설정")]
    public int gridSize = 5;
    public float cellSize = 140f;  // 100f → 140f (모바일 터치 개선)
    public float cellSpacing = 12f;  // 10f → 12f
    
    [Header("프리팹 - 반드시 연결!")]
    public GameObject blockPrefab;
    public GameObject cellPrefab;
    
    [Header("컨테이너 - 반드시 연결!")]
    public RectTransform gridContainer;
    public RectTransform blocksContainer;
    
    private Block[,] blocks;
    private GameObject[,] cells;
    
    private void Awake()
    {
        Debug.Log("=== Grid Awake ===");
        blocks = new Block[gridSize, gridSize];
        cells = new GameObject[gridSize, gridSize];
        
        // 참조 체크
        CheckReferences();
    }
    
    /// <summary>
    /// 참조 체크
    /// </summary>
    private void CheckReferences()
    {
        Debug.Log("[Grid 참조 체크]");
        
        if (blockPrefab == null)
        {
            Debug.LogError("!!! blockPrefab이 연결되지 않았습니다!");
        }
        else
        {
            Debug.Log($"✓ blockPrefab: {blockPrefab.name}");
        }
        
        if (cellPrefab == null)
        {
            Debug.LogError("!!! cellPrefab이 연결되지 않았습니다!");
        }
        else
        {
            Debug.Log($"✓ cellPrefab: {cellPrefab.name}");
        }
        
        if (gridContainer == null)
        {
            Debug.LogError("!!! gridContainer가 연결되지 않았습니다!");
        }
        else
        {
            Debug.Log($"✓ gridContainer: {gridContainer.name}");
        }
        
        if (blocksContainer == null)
        {
            Debug.LogError("!!! blocksContainer가 연결되지 않았습니다!");
        }
        else
        {
            Debug.Log($"✓ blocksContainer: {blocksContainer.name}");
        }
    }
    
    /// <summary>
    /// 그리드 초기화
    /// </summary>
    public void Initialize()
    {
        Debug.Log("=== Grid Initialize 시작 ===");
        
        if (gridContainer == null || cellPrefab == null)
        {
            Debug.LogError("gridContainer 또는 cellPrefab이 null입니다!");
            return;
        }
        
        // Canvas 계층 구조 확인
        CheckCanvasHierarchy();
        
        CreateGrid();
        Debug.Log("=== Grid Initialize 완료 ===");
    }
    
    /// <summary>
    /// Canvas 계층 구조 확인
    /// </summary>
    private void CheckCanvasHierarchy()
    {
        Debug.Log("=== Canvas 계층 구조 확인 ===");
        
        // gridContainer의 Canvas 찾기
        Canvas gridCanvas = gridContainer.GetComponentInParent<Canvas>();
        if (gridCanvas != null)
        {
            Debug.Log($"✓ gridContainer의 Canvas: {gridCanvas.name}");
            Debug.Log($"  - Render Mode: {gridCanvas.renderMode}");
            Debug.Log($"  - Sort Order: {gridCanvas.sortingOrder}");
        }
        else
        {
            Debug.LogError("❌ gridContainer가 Canvas의 자식이 아닙니다!");
        }
        
        // blocksContainer의 Canvas 찾기
        Canvas blocksCanvas = blocksContainer.GetComponentInParent<Canvas>();
        if (blocksCanvas != null)
        {
            Debug.Log($"✓ blocksContainer의 Canvas: {blocksCanvas.name}");
            Debug.Log($"  - Render Mode: {blocksCanvas.renderMode}");
            Debug.Log($"  - Sort Order: {blocksCanvas.sortingOrder}");
            
            // GraphicRaycaster 확인
            GraphicRaycaster raycaster = blocksCanvas.GetComponent<GraphicRaycaster>();
            if (raycaster != null)
            {
                Debug.Log($"✓ Canvas에 GraphicRaycaster 있음");
            }
            else
            {
                Debug.LogWarning($"⚠️ Canvas에 GraphicRaycaster가 없습니다!");
            }
        }
        else
        {
            Debug.LogError("❌ blocksContainer가 Canvas의 자식이 아닙니다!");
        }
        
        // 같은 Canvas인지 확인
        if (gridCanvas != null && blocksCanvas != null)
        {
            if (gridCanvas == blocksCanvas)
            {
                Debug.Log("✓ gridContainer와 blocksContainer가 같은 Canvas 안에 있음");
            }
            else
            {
                Debug.LogWarning("⚠️ gridContainer와 blocksContainer가 다른 Canvas에 있습니다!");
            }
        }
    }
    
    /// <summary>
    /// 그리드 셀 생성
    /// </summary>
    private void CreateGrid()
    {
        Debug.Log($"[CreateGrid] {gridSize}x{gridSize} 그리드 생성 시작");
        
        // 그리드 컨테이너 크기 조정
        float totalSize = (gridSize * cellSize) + ((gridSize - 1) * cellSpacing);
        gridContainer.sizeDelta = new Vector2(totalSize, totalSize);
        Debug.Log($"그리드 컨테이너 크기: {totalSize}x{totalSize}");
        
        // 셀 생성
        int cellCount = 0;
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                try
                {
                    GameObject cell = Instantiate(cellPrefab, gridContainer);
                    RectTransform cellRect = cell.GetComponent<RectTransform>();
                    
                    cellRect.sizeDelta = new Vector2(cellSize, cellSize);
                    cellRect.anchoredPosition = GetCellPosition(x, y);
                    
                    cells[x, y] = cell;
                    cellCount++;
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"셀 [{x},{y}] 생성 실패: {e.Message}");
                }
            }
        }
        
        Debug.Log($"✓ 셀 생성 완료: {cellCount}개");
    }
    
    /// <summary>
    /// 특정 그리드 위치의 월드 좌표 반환
    /// </summary>
    public Vector3 GetCellPosition(int x, int y)
    {
        float startX = -(gridSize - 1) * (cellSize + cellSpacing) / 2f;
        float startY = (gridSize - 1) * (cellSize + cellSpacing) / 2f;
        
        float posX = startX + x * (cellSize + cellSpacing);
        float posY = startY - y * (cellSize + cellSpacing);
        
        return new Vector3(posX, posY, 0);
    }
    
    /// <summary>
    /// 특정 위치에 블록 추가
    /// </summary>
    public Block AddBlock(int level, Vector2Int position)
    {
        Debug.Log($"[AddBlock] 레벨 {level}, 위치 {position}");
        
        if (!IsValidPosition(position))
        {
            Debug.LogError($"유효하지 않은 위치: {position}");
            return null;
        }
        
        if (blocks[position.x, position.y] != null)
        {
            Debug.LogError($"위치 {position}에 이미 블록이 있습니다!");
            return null;
        }
        
        if (blockPrefab == null)
        {
            Debug.LogError("blockPrefab이 null입니다!");
            return null;
        }
        
        if (blocksContainer == null)
        {
            Debug.LogError("blocksContainer가 null입니다!");
            return null;
        }
        
        try
        {
            // 블록 생성
            GameObject blockObj = Instantiate(blockPrefab, blocksContainer);
            Debug.Log($"✓ 블록 오브젝트 생성: {blockObj.name}");
            
            // 블록의 Canvas 확인
            Canvas blockCanvas = blockObj.GetComponentInParent<Canvas>();
            if (blockCanvas != null)
            {
                Debug.Log($"✓ 생성된 블록의 Canvas: {blockCanvas.name}");
            }
            else
            {
                Debug.LogError($"❌ 생성된 블록이 Canvas 안에 없습니다!");
                Debug.LogError($"   Parent: {blockObj.transform.parent?.name}");
            }
            
            Block block = blockObj.GetComponent<Block>();
            if (block == null)
            {
                Debug.LogError("생성된 블록에 Block 컴포넌트가 없습니다!");
                Destroy(blockObj);
                return null;
            }
            
            // RectTransform 설정
            RectTransform blockRect = blockObj.GetComponent<RectTransform>();
            blockRect.sizeDelta = new Vector2(cellSize, cellSize);
            blockRect.anchoredPosition = GetCellPosition(position.x, position.y);
            Debug.Log($"블록 위치 설정: {blockRect.anchoredPosition}");
            
            // Image raycastTarget 확인
            UnityEngine.UI.Image img = blockObj.GetComponent<UnityEngine.UI.Image>();
            if (img != null)
            {
                Debug.Log($"블록 Image raycastTarget: {img.raycastTarget}");
            }
            
            // 블록 초기화
            block.Initialize(level, position);
            blocks[position.x, position.y] = block;
            
            Debug.Log($"✓ 블록 추가 완료: 레벨 {level}, 위치 {position}");
            return block;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"블록 생성 중 예외 발생: {e.Message}\n{e.StackTrace}");
            return null;
        }
    }
    
    /// <summary>
    /// 랜덤 빈 위치에 블록 추가
    /// </summary>
    public Block AddRandomBlock(int level = 1)
    {
        Debug.Log($"[AddRandomBlock] 레벨 {level} 블록 추가 시도");
        
        List<Vector2Int> emptyPositions = GetEmptyPositions();
        Debug.Log($"빈 공간 수: {emptyPositions.Count}");
        
        if (emptyPositions.Count == 0)
        {
            Debug.LogWarning("빈 공간이 없습니다!");
            return null;
        }
        
        Vector2Int randomPos = emptyPositions[Random.Range(0, emptyPositions.Count)];
        Debug.Log($"랜덤 위치 선택: {randomPos}");
        
        return AddBlock(level, randomPos);
    }
    
    /// <summary>
    /// 블록 제거
    /// </summary>
    public void RemoveBlock(Vector2Int position)
    {
        if (!IsValidPosition(position))
            return;
        
        Block block = blocks[position.x, position.y];
        if (block != null)
        {
            Debug.Log($"블록 제거: 위치 {position}");
            Destroy(block.gameObject);
            blocks[position.x, position.y] = null;
        }
    }
    
    /// <summary>
    /// 블록 이동
    /// </summary>
    public void MoveBlock(Vector2Int from, Vector2Int to)
    {
        if (!IsValidPosition(from) || !IsValidPosition(to))
            return;
        
        Block block = blocks[from.x, from.y];
        if (block == null)
            return;
        
        blocks[to.x, to.y] = block;
        blocks[from.x, from.y] = null;
        block.gridPosition = to;
        
        Vector3 targetPos = GetCellPosition(to.x, to.y);
        block.MoveTo(targetPos);
        
        Debug.Log($"블록 이동: {from} → {to}");
    }
    
    /// <summary>
    /// 특정 위치의 블록 가져오기
    /// </summary>
    public Block GetBlock(Vector2Int position)
    {
        if (!IsValidPosition(position))
            return null;
        
        return blocks[position.x, position.y];
    }
    
    /// <summary>
    /// 유효한 위치인지 확인
    /// </summary>
    public bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridSize &&
               position.y >= 0 && position.y < gridSize;
    }
    
    /// <summary>
    /// 빈 위치인지 확인
    /// </summary>
    public bool IsEmptyPosition(Vector2Int position)
    {
        return IsValidPosition(position) && blocks[position.x, position.y] == null;
    }
    
    /// <summary>
    /// 빈 위치 목록 반환
    /// </summary>
    public List<Vector2Int> GetEmptyPositions()
    {
        List<Vector2Int> emptyPositions = new List<Vector2Int>();
        
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (blocks[x, y] == null)
                {
                    emptyPositions.Add(new Vector2Int(x, y));
                }
            }
        }
        
        return emptyPositions;
    }
    
    /// <summary>
    /// 인접한 블록 가져오기
    /// </summary>
    public List<Block> GetAdjacentBlocks(Vector2Int position)
    {
        List<Block> adjacentBlocks = new List<Block>();
        
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, -1),  // 위
            new Vector2Int(0, 1),   // 아래
            new Vector2Int(-1, 0),  // 왼쪽
            new Vector2Int(1, 0)    // 오른쪽
        };
        
        foreach (Vector2Int dir in directions)
        {
            Vector2Int adjacentPos = position + dir;
            Block block = GetBlock(adjacentPos);
            if (block != null)
            {
                adjacentBlocks.Add(block);
            }
        }
        
        return adjacentBlocks;
    }
    
    /// <summary>
    /// 모든 블록 가져오기
    /// </summary>
    public List<Block> GetAllBlocks()
    {
        List<Block> allBlocks = new List<Block>();
        
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (blocks[x, y] != null)
                {
                    allBlocks.Add(blocks[x, y]);
                }
            }
        }
        
        return allBlocks;
    }
    
    /// <summary>
    /// 그리드가 가득 찼는지 확인
    /// </summary>
    public bool IsFull()
    {
        return GetEmptyPositions().Count == 0;
    }
    
    /// <summary>
    /// 모든 블록 제거
    /// </summary>
    public void ClearAllBlocks()
    {
        Debug.Log("모든 블록 제거");
        int count = 0;
        
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (blocks[x, y] != null)
                {
                    Destroy(blocks[x, y].gameObject);
                    blocks[x, y] = null;
                    count++;
                }
            }
        }
        
        Debug.Log($"✓ {count}개 블록 제거 완료");
    }
}
