using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 2048 그리드 관리 클래스
/// </summary>
public class Classic2048Grid : MonoBehaviour
{
    [Header("그리드 설정")]
    public int gridSize = 4;
    public float cellSize = 140f;
    public float cellSpacing = 15f;
    
    [Header("타일 프리팹")]
    public GameObject tilePrefab;
    
    private Classic2048Tile[,] tiles;
    private List<Classic2048Tile> allTiles = new List<Classic2048Tile>();
    
    /// <summary>
    /// 그리드 초기화
    /// </summary>
    public void Initialize()
    {
        Debug.Log("=== 그리드 초기화 ===");
        
        tiles = new Classic2048Tile[gridSize, gridSize];
        allTiles.Clear();
        
        // 타일 프리팹이 없으면 자동 생성
        if (tilePrefab == null)
        {
            CreateTilePrefab();
        }
    }
    
    /// <summary>
    /// 타일 프리팹 자동 생성
    /// </summary>
    private void CreateTilePrefab()
    {
        tilePrefab = new GameObject("TilePrefab");
        
        RectTransform rect = tilePrefab.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(cellSize, cellSize);
        
        tilePrefab.AddComponent<Classic2048Tile>();
        tilePrefab.SetActive(false);
        tilePrefab.transform.SetParent(transform);
        
        Debug.Log("✓ 타일 프리팹 자동 생성");
    }
    
    /// <summary>
    /// 랜덤 타일 추가
    /// </summary>
    public void AddRandomTile()
    {
        // 빈 칸 찾기
        List<Vector2Int> emptyPositions = new List<Vector2Int>();
        
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (tiles[x, y] == null)
                {
                    emptyPositions.Add(new Vector2Int(x, y));
                }
            }
        }
        
        if (emptyPositions.Count == 0)
        {
            Debug.LogWarning("빈 칸이 없습니다!");
            return;
        }
        
        // 랜덤 위치 선택
        Vector2Int randomPos = emptyPositions[Random.Range(0, emptyPositions.Count)];
        
        // 2 또는 4 생성 (90% 확률로 2)
        int value = Random.value < 0.9f ? 2 : 4;
        
        CreateTile(value, randomPos);
    }
    
    /// <summary>
    /// 타일 생성
    /// </summary>
    private void CreateTile(int value, Vector2Int gridPos)
    {
        GameObject tileObj = Instantiate(tilePrefab, transform);
        tileObj.SetActive(true);
        tileObj.name = $"Tile_{value}_{gridPos.x}_{gridPos.y}";
        
        Classic2048Tile tile = tileObj.GetComponent<Classic2048Tile>();
        tile.Initialize(value, gridPos);
        
        // 위치 설정
        RectTransform rect = tileObj.GetComponent<RectTransform>();
        rect.anchoredPosition = GetWorldPosition(gridPos);
        
        tiles[gridPos.x, gridPos.y] = tile;
        allTiles.Add(tile);
        
        Debug.Log($"✓ 타일 생성: 위치 ({gridPos.x}, {gridPos.y}), 값 {value}");
    }
    
    /// <summary>
    /// 그리드 위치를 월드 위치로 변환
    /// </summary>
    private Vector2 GetWorldPosition(Vector2Int gridPos)
    {
        float totalSize = gridSize * cellSize + (gridSize - 1) * cellSpacing;
        float startX = -totalSize / 2f + cellSize / 2f;
        float startY = -totalSize / 2f + cellSize / 2f;
        
        float x = startX + gridPos.x * (cellSize + cellSpacing);
        float y = startY + gridPos.y * (cellSize + cellSpacing);
        
        return new Vector2(x, y);
    }
    
    /// <summary>
    /// 타일 이동
    /// </summary>
    public bool MoveTiles(Vector2Int direction)
    {
        bool moved = false;
        
        // 병합 플래그 초기화
        foreach (var tile in allTiles)
        {
            if (tile != null)
                tile.hasMerged = false;
        }
        
        // 이동 순서 결정
        if (direction == Vector2Int.up)
        {
            // 위로 이동: 아래부터 위로
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = gridSize - 2; y >= 0; y--)
                {
                    if (MoveTile(new Vector2Int(x, y), direction))
                        moved = true;
                }
            }
        }
        else if (direction == Vector2Int.down)
        {
            // 아래로 이동: 위부터 아래로
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 1; y < gridSize; y++)
                {
                    if (MoveTile(new Vector2Int(x, y), direction))
                        moved = true;
                }
            }
        }
        else if (direction == Vector2Int.left)
        {
            // 왼쪽 이동: 왼쪽부터 오른쪽으로
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 1; x < gridSize; x++)
                {
                    if (MoveTile(new Vector2Int(x, y), direction))
                        moved = true;
                }
            }
        }
        else if (direction == Vector2Int.right)
        {
            // 오른쪽 이동: 오른쪽부터 왼쪽으로
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = gridSize - 2; x >= 0; x--)
                {
                    if (MoveTile(new Vector2Int(x, y), direction))
                        moved = true;
                }
            }
        }
        
        return moved;
    }
    
    /// <summary>
    /// 개별 타일 이동
    /// </summary>
    private bool MoveTile(Vector2Int from, Vector2Int direction)
    {
        if (tiles[from.x, from.y] == null)
            return false;
        
        Classic2048Tile tile = tiles[from.x, from.y];
        Vector2Int targetPos = from;
        
        // 가장 먼 위치 찾기
        while (true)
        {
            Vector2Int nextPos = targetPos + direction;
            
            if (nextPos.x < 0 || nextPos.x >= gridSize || 
                nextPos.y < 0 || nextPos.y >= gridSize)
                break;
            
            Classic2048Tile targetTile = tiles[nextPos.x, nextPos.y];
            
            if (targetTile == null)
            {
                targetPos = nextPos;
            }
            else if (targetTile.value == tile.value && !targetTile.hasMerged && !tile.hasMerged)
            {
                // 병합 가능 - 즉시 그리드 상태 업데이트
                tiles[from.x, from.y] = null;
                MergeTiles(tile, targetTile, nextPos);
                return true;
            }
            else
            {
                break;
            }
        }
        
        // 이동
        if (targetPos != from)
        {
            tiles[from.x, from.y] = null;
            tiles[targetPos.x, targetPos.y] = tile;
            tile.gridPosition = targetPos;
            tile.MoveTo(GetWorldPosition(targetPos));
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// 타일 병합
    /// </summary>
    private void MergeTiles(Classic2048Tile movingTile, Classic2048Tile targetTile, Vector2Int targetPos)
    {
        // 이동 타일 제거
        tiles[movingTile.gridPosition.x, movingTile.gridPosition.y] = null;
        allTiles.Remove(movingTile);
        
        // 애니메이션 후 제거
        movingTile.MoveTo(GetWorldPosition(targetPos));
        Destroy(movingTile.gameObject, 0.15f);
        
        // 타겟 타일 값 증가
        int newValue = targetTile.value * 2;
        targetTile.SetValue(newValue);
        targetTile.hasMerged = true;
        targetTile.PlayMergeAnimation();
        
        // 점수 추가
        if (Classic2048Manager.Instance != null)
        {
            Classic2048Manager.Instance.AddScore(newValue);
        }
        
        // 병합 사운드
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMergeSound();
        }
        
        Debug.Log($"🔀 병합: {targetTile.value / 2} + {targetTile.value / 2} = {targetTile.value}");
    }
    
    /// <summary>
    /// 이동 가능 여부
    /// </summary>
    public bool CanMove()
    {
        // 빈 칸이 있으면 이동 가능
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (tiles[x, y] == null)
                    return true;
            }
        }
        
        // 인접한 같은 값이 있으면 이동 가능
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (tiles[x, y] == null) continue;
                
                int value = tiles[x, y].value;
                
                // 우측 체크
                if (x < gridSize - 1 && tiles[x + 1, y] != null && tiles[x + 1, y].value == value)
                    return true;
                
                // 하단 체크
                if (y < gridSize - 1 && tiles[x, y + 1] != null && tiles[x, y + 1].value == value)
                    return true;
            }
        }
        
        return false;
    }
    
    /// <summary>
    /// 2048 타일 존재 여부
    /// </summary>
    public bool Has2048Tile()
    {
        return allTiles.Any(tile => tile != null && tile.value >= 2048);
    }
    
    /// <summary>
    /// 최대 타일 값 반환
    /// </summary>
    public int GetMaxTileValue()
    {
        int maxValue = 0;
        foreach (var tile in allTiles)
        {
            if (tile != null && tile.value > maxValue)
                maxValue = tile.value;
        }
        return maxValue;
    }
    
    /// <summary>
    /// 그리드 초기화
    /// </summary>
    public void Clear()
    {
        foreach (var tile in allTiles)
        {
            if (tile != null)
                Destroy(tile.gameObject);
        }
        
        allTiles.Clear();
        tiles = new Classic2048Tile[gridSize, gridSize];
        
        Debug.Log("✓ 그리드 초기화 완료");
    }
}
