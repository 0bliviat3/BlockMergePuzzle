using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 블록 병합 로직 담당 클래스 - 콤보 시스템 개선
/// </summary>
public class BlockMerger : MonoBehaviour
{
    [Header("설정")]
    public int explodeLevel = 10;
    public int explodeRadius = 1;
    
    [Header("참조")]
    public Grid grid;
    public ScoreManager scoreManager;
    public EffectManager effectManager;
    
    private Block selectedBlock = null;
    private bool isMerging = false;
    
    // 콤보 시스템
    private float lastMergeTime = 0f;
    private float comboWindow = 3f; // 3초 안에 병합하면 콤보
    
    private void Start()
    {
        Debug.Log("=== BlockMerger Start ===");
        
        if (grid == null)
        {
            Debug.LogError("Grid가 연결되지 않았습니다!");
        }
        if (scoreManager == null)
        {
            Debug.LogWarning("ScoreManager가 연결되지 않았습니다.");
        }
        if (effectManager == null)
        {
            Debug.LogWarning("EffectManager가 연결되지 않았습니다.");
        }
    }
    
    /// <summary>
    /// 블록 선택
    /// </summary>
    public void SelectBlock(Block block)
    {
        if (isMerging)
        {
            Debug.Log("병합 중이라 선택 불가");
            return;
        }
        
        Debug.Log($"SelectBlock 호출: {block.gridPosition}");
        
        if (selectedBlock != null)
        {
            if (selectedBlock == block)
            {
                Debug.Log("같은 블록 다시 클릭 - 선택 취소");
                DeselectBlock();
                return;
            }
            
            if (CanMerge(selectedBlock, block))
            {
                Debug.Log($"병합 시도: {selectedBlock.gridPosition} + {block.gridPosition}");
                StartCoroutine(MergeBlocks(selectedBlock, block));
            }
            else
            {
                Debug.Log("병합 불가능 - 선택 변경");
                DeselectBlock();
                selectedBlock = block;
                HighlightBlock(block);
            }
        }
        else
        {
            Debug.Log("첫 번째 블록 선택");
            selectedBlock = block;
            HighlightBlock(block);
        }
    }
    
    private void DeselectBlock()
    {
        if (selectedBlock != null)
        {
            UnhighlightBlock(selectedBlock);
            selectedBlock = null;
        }
    }
    
    private void HighlightBlock(Block block)
    {
        if (block == null) return;
        
        Debug.Log($"블록 하이라이트: {block.gridPosition}");
        
        LeanTween.scale(block.gameObject, Vector3.one * 1.1f, 0.2f)
                .setEase(LeanTweenType.easeOutQuad);
    }
    
    private void UnhighlightBlock(Block block)
    {
        if (block == null) return;
        
        Debug.Log($"블록 하이라이트 해제: {block.gridPosition}");
        
        LeanTween.scale(block.gameObject, Vector3.one, 0.2f)
                .setEase(LeanTweenType.easeOutQuad);
    }
    
    private bool CanMerge(Block block1, Block block2)
    {
        if (block1 == null || block2 == null) return false;
        
        if (block1.level != block2.level)
        {
            Debug.Log($"레벨이 다름: {block1.level} != {block2.level}");
            return false;
        }
        
        Vector2Int pos1 = block1.gridPosition;
        Vector2Int pos2 = block2.gridPosition;
        
        int distance = Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
        bool adjacent = distance == 1;
        
        if (!adjacent)
        {
            Debug.Log($"인접하지 않음: 거리 = {distance}");
        }
        
        return adjacent;
    }
    
    /// <summary>
    /// 블록 병합 ⭐ 콤보 시스템 추가
    /// </summary>
    private IEnumerator MergeBlocks(Block block1, Block block2)
    {
        if (grid == null)
        {
            Debug.LogError("Grid가 null입니다!");
            yield break;
        }
        
        isMerging = true;
        Debug.Log($"=== 블록 병합 시작: {block1.gridPosition} + {block2.gridPosition} ===");
        
        // ⭐ 콤보 체크 (3초 안에 연속 병합)
        float currentTime = Time.time;
        bool isCombo = (currentTime - lastMergeTime) < comboWindow;
        
        if (isCombo && scoreManager != null)
        {
            scoreManager.AddCombo();
            Debug.Log("🔥 콤보 발동!");
        }
        
        lastMergeTime = currentTime;
        
        UnhighlightBlock(block1);
        
        Vector3 targetPos = grid.GetCellPosition(block1.gridPosition.x, block1.gridPosition.y);
        
        block2.PlayMergeAnimation(targetPos, () =>
        {
            grid.RemoveBlock(block2.gridPosition);
            Debug.Log($"block2 제거: {block2.gridPosition}");
            
            block1.LevelUp();
            Debug.Log($"block1 레벨업: {block1.level}");
            
            // 점수 추가 (콤보 배율 자동 적용됨)
            if (scoreManager != null)
            {
                int score = block1.GetBlockValue();
                scoreManager.AddScore(score);
                Debug.Log($"점수 추가: +{score}");
            }
            
            // 최고 레벨 업데이트
            if (GameManager.Instance != null)
            {
                GameManager.Instance.UpdateMaxBlockLevel(block1.level);
            }
            
            // 폭발 체크
            if (block1.level >= explodeLevel)
            {
                Debug.Log($"💥 폭발 조건 만족: 레벨 {block1.level} >= {explodeLevel}");
                StartCoroutine(ExplodeBlock(block1));
            }
            else
            {
                // 병합 효과
                if (effectManager != null)
                {
                    effectManager.PlayMergeEffect(block1.transform.position);
                }
                
                CheckForChainMerge(block1);
            }
        });
        
        yield return new WaitForSeconds(0.4f);
        
        selectedBlock = null;
        isMerging = false;
        
        // 새 블록 추가
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddNewBlock();
        }
        
        Debug.Log("=== 블록 병합 완료 ===");
    }
    
    private IEnumerator ExplodeBlock(Block block)
    {
        if (grid == null) yield break;
        
        Debug.Log($"=== 블록 폭발: {block.gridPosition}, 레벨 {block.level} ===");
        
        Vector2Int centerPos = block.gridPosition;
        
        if (effectManager != null)
        {
            effectManager.PlayExplodeEffect(block.transform.position);
        }
        
        // 폭발 보너스 점수
        if (scoreManager != null)
        {
            int bonusScore = block.GetBlockValue() * 2;
            scoreManager.AddScore(bonusScore);
            scoreManager.AddCombo(); // 폭발도 콤보 추가
            Debug.Log($"💥 폭발 보너스 점수: +{bonusScore}");
        }
        
        block.PlayExplodeAnimation(() =>
        {
            grid.RemoveBlock(centerPos);
        });
        
        yield return new WaitForSeconds(0.3f);
        
        List<Block> affectedBlocks = GetBlocksInRadius(centerPos, explodeRadius);
        Debug.Log($"영향받은 블록 수: {affectedBlocks.Count}");
        
        int removedBlockCount = 1; // 중심 블록
        
        foreach (Block affectedBlock in affectedBlocks)
        {
            if (affectedBlock.level <= 3)
            {
                affectedBlock.PlayExplodeAnimation(() =>
                {
                    grid.RemoveBlock(affectedBlock.gridPosition);
                });
                
                if (scoreManager != null)
                {
                    scoreManager.AddScore(affectedBlock.GetBlockValue());
                }
                
                removedBlockCount++; // 제거된 블록 수 카운트
                Debug.Log($"낮은 레벨 블록 제거: {affectedBlock.gridPosition}");
            }
            else
            {
                affectedBlock.level = Mathf.Max(1, affectedBlock.level - 2);
                affectedBlock.UpdateVisuals();
                Debug.Log($"블록 레벨 다운: {affectedBlock.gridPosition}, 새 레벨 {affectedBlock.level}");
                
                LeanTween.rotateZ(affectedBlock.gameObject, 10f, 0.1f)
                        .setLoopPingPong(2)
                        .setOnComplete(() =>
                        {
                            affectedBlock.transform.rotation = Quaternion.identity;
                        });
            }
            
            yield return new WaitForSeconds(0.05f);
        }
        
        yield return new WaitForSeconds(0.3f);
        
        // ⭐ 폭발 후 빈 칸 채우기 (중요!)
        Debug.Log($"💡 폭발로 {removedBlockCount}개 블록 제거됨 → 새 블록으로 채우기 시작");
        
        if (GameManager.Instance != null)
        {
            for (int i = 0; i < removedBlockCount; i++)
            {
                int level = GameManager.Instance.GetRandomBlockLevel();
                Block newBlock = grid.AddRandomBlock(level);
                
                if (newBlock != null)
                {
                    Debug.Log($"✓ 빈 칸 채움 {i + 1}/{removedBlockCount} - 위치: {newBlock.gridPosition}, 레벨: {level}");
                    yield return new WaitForSeconds(0.1f); // 시각적 효과
                }
                else
                {
                    Debug.LogWarning($"⚠️ 빈 칸 채우기 실패 {i + 1}/{removedBlockCount} - 더 이상 빈 칸이 없음");
                    break;
                }
            }
            
            // 빈 칸을 채운 후 게임오버 체크
            GameManager.Instance.CheckGameOverImmediate();
        }
        
        Debug.Log("=== 폭발 + 빈 칸 채우기 완료 ===");
    }
    
    private List<Block> GetBlocksInRadius(Vector2Int center, int radius)
    {
        if (grid == null) return new List<Block>();
        
        List<Block> blocksInRadius = new List<Block>();
        
        for (int dy = -radius; dy <= radius; dy++)
        {
            for (int dx = -radius; dx <= radius; dx++)
            {
                if (dx == 0 && dy == 0) continue;
                
                Vector2Int pos = new Vector2Int(center.x + dx, center.y + dy);
                Block block = grid.GetBlock(pos);
                
                if (block != null)
                {
                    blocksInRadius.Add(block);
                }
            }
        }
        
        return blocksInRadius;
    }
    
    private void CheckForChainMerge(Block block)
    {
        if (grid == null || effectManager == null) return;
        
        List<Block> adjacentBlocks = grid.GetAdjacentBlocks(block.gridPosition);
        
        foreach (Block adjacent in adjacentBlocks)
        {
            if (adjacent.level == block.level)
            {
                effectManager.PlayHintEffect(adjacent.transform.position);
                Debug.Log($"💡 연쇄 가능: {adjacent.gridPosition}");
            }
        }
    }
    
    public bool AutoMerge()
    {
        if (grid == null) return false;
        
        List<Block> allBlocks = grid.GetAllBlocks();
        
        foreach (Block block in allBlocks)
        {
            List<Block> adjacentBlocks = grid.GetAdjacentBlocks(block.gridPosition);
            
            foreach (Block adjacent in adjacentBlocks)
            {
                if (CanMerge(block, adjacent))
                {
                    StartCoroutine(MergeBlocks(block, adjacent));
                    return true;
                }
            }
        }
        
        return false;
    }
    
    public bool HasPossibleMerges()
    {
        if (grid == null) return false;
        
        List<Block> allBlocks = grid.GetAllBlocks();
        
        foreach (Block block in allBlocks)
        {
            List<Block> adjacentBlocks = grid.GetAdjacentBlocks(block.gridPosition);
            
            foreach (Block adjacent in adjacentBlocks)
            {
                if (CanMerge(block, adjacent))
                {
                    return true;
                }
            }
        }
        
        return false;
    }
}
