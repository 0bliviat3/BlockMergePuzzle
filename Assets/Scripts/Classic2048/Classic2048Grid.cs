using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 2048 그리드 관리 클래스 - 완전히 재작성된 버전
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
    /// 타일 이동 - 완전히 재작성
    /// </summary>
    public bool MoveTiles(Vector2Int direction)
    {
        bool moved = false;
        
        // 1단계: 병합 플래그 초기화
        foreach (var tile in allTiles)
        {
            if (tile != null)
                tile.hasMerged = false;
        }
        
        // 2단계: 방향별 처리
        if (direction == Vector2Int.up)
        {
            moved = ProcessVertical(true);
        }
        else if (direction == Vector2Int.down)
        {
            moved = ProcessVertical(false);
        }
        else if (direction == Vector2Int.left)
        {
            moved = ProcessHorizontal(true);
        }
        else if (direction == Vector2Int.right)
        {
            moved = ProcessHorizontal(false);
        }
        
        return moved;
    }
    
    /// <summary>
    /// 수직 이동 처리 (위/아래)
    /// </summary>
    private bool ProcessVertical(bool isUp)
    {
        bool moved = false;
        
        for (int x = 0; x < gridSize; x++)
        {
            // 타일 수집
            List<Classic2048Tile> column = new List<Classic2048Tile>();
            
            if (isUp)
            {
                for (int y = gridSize - 1; y >= 0; y--)
                {
                    if (tiles[x, y] != null)
                        column.Add(tiles[x, y]);
                }
            }
            else
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (tiles[x, y] != null)
                        column.Add(tiles[x, y]);
                }
            }
            
            // 병합 처리
            List<Classic2048Tile> merged = MergeList(column);
            
            // 새 위치에 배치
            for (int i = 0; i < gridSize; i++)
            {
                int y = isUp ? (gridSize - 1 - i) : i;
                
                if (i < merged.Count)
                {
                    Classic2048Tile tile = merged[i];
                    if (tile.gridPosition.x != x || tile.gridPosition.y != y)
                    {
                        // 위치 변경됨
                        tiles[tile.gridPosition.x, tile.gridPosition.y] = null;
                        tiles[x, y] = tile;
                        tile.gridPosition = new Vector2Int(x, y);
                        tile.MoveTo(GetWorldPosition(new Vector2Int(x, y)));
                        moved = true;
                    }
                }
                else
                {
                    tiles[x, y] = null;
                }
            }
        }
        
        return moved;
    }
    
    /// <summary>
    /// 수평 이동 처리 (좌/우)
    /// </summary>
    private bool ProcessHorizontal(bool isLeft)
    {
        bool moved = false;
        
        for (int y = 0; y < gridSize; y++)
        {
            // 타일 수집
            List<Classic2048Tile> row = new List<Classic2048Tile>();
            
            if (isLeft)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    if (tiles[x, y] != null)
                        row.Add(tiles[x, y]);
                }
            }
            else
            {
                for (int x = gridSize - 1; x >= 0; x--)
                {
                    if (tiles[x, y] != null)
                        row.Add(tiles[x, y]);
                }
            }
            
            // 병합 처리
            List<Classic2048Tile> merged = MergeList(row);
            
            // 새 위치에 배치
            for (int i = 0; i < gridSize; i++)
            {
                int x = isLeft ? i : (gridSize - 1 - i);
                
                if (i < merged.Count)
                {
                    Classic2048Tile tile = merged[i];
                    if (tile.gridPosition.x != x || tile.gridPosition.y != y)
                    {
                        // 위치 변경됨
                        tiles[tile.gridPosition.x, tile.gridPosition.y] = null;
                        tiles[x, y] = tile;
                        tile.gridPosition = new Vector2Int(x, y);
                        tile.MoveTo(GetWorldPosition(new Vector2Int(x, y)));
                        moved = true;
                    }
                }
                else
                {
                    tiles[x, y] = null;
                }
            }
        }
        
        return moved;
    }
    
    /// <summary>
    /// 타일 리스트 병합 처리
    /// </summary>
    private List<Classic2048Tile> MergeList(List<Classic2048Tile> tiles)
    {
        List<Classic2048Tile> result = new List<Classic2048Tile>();
        
        for (int i = 0; i < tiles.Count; i++)
        {
            if (i + 1 < tiles.Count && 
                tiles[i].value == tiles[i + 1].value && 
                !tiles[i].hasMerged)
            {
                // 병합
                Classic2048Tile keepTile = tiles[i];
                Classic2048Tile removeTile = tiles[i + 1];
                
                int newValue = keepTile.value * 2;
                keepTile.SetValue(newValue);
                keepTile.hasMerged = true;
                keepTile.PlayMergeAnimation();
                
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
                
                // 제거할 타일 즉시 삭제
                allTiles.Remove(removeTile);
                Destroy(removeTile.gameObject);
                
                Debug.Log($"🔀 병합: {newValue / 2} + {newValue / 2} = {newValue}");
                
                result.Add(keepTile);
                i++; // 다음 타일 건너뛰기
            }
            else
            {
                result.Add(tiles[i]);
            }
        }
        
        return result;
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
