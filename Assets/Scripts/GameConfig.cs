using UnityEngine;

/// <summary>
/// 게임 설정 및 상수 관리
/// </summary>
public static class GameConfig
{
    // 그리드 설정
    public const int DEFAULT_GRID_SIZE = 5;
    public const float DEFAULT_CELL_SIZE = 100f;
    public const float DEFAULT_CELL_SPACING = 10f;
    
    // 블록 설정
    public const int STARTING_BLOCKS = 3;
    public const int EXPLODE_LEVEL = 10;  // 1024 블록에서 폭발
    public const int EXPLODE_RADIUS = 1;
    
    // 확률 설정
    public const float LEVEL_1_PROBABILITY = 0.90f;  // 90% 확률로 2 블록
    public const float LEVEL_2_PROBABILITY = 0.10f;  // 10% 확률로 4 블록
    
    // 점수 설정
    public const float COMBO_TIME_LIMIT = 3f;
    public const float COMBO_MULTIPLIER = 1.5f;
    
    // 애니메이션 설정
    public const float MERGE_ANIMATION_DURATION = 0.3f;
    public const float SPAWN_ANIMATION_DURATION = 0.2f;
    public const float EXPLODE_ANIMATION_DURATION = 0.4f;
    public const float MOVE_ANIMATION_DURATION = 0.2f;
    
    // 효과 설정
    public const float SCREEN_SHAKE_DURATION = 0.3f;
    public const float SCREEN_SHAKE_MAGNITUDE = 0.2f;
    
    // 레벨별 색상 (HSV 기반으로 자동 생성 가능)
    public static Color GetColorForLevel(int level)
    {
        // 레벨에 따라 색상 변화 (무지개 스펙트럼)
        float hue = (level * 0.1f) % 1f;
        return Color.HSVToRGB(hue, 0.7f, 0.9f);
    }
    
    // 블록 값 계산
    public static int GetBlockValue(int level)
    {
        return (int)Mathf.Pow(2, level);
    }
    
    // 레벨에서 점수 계산
    public static int GetScoreForLevel(int level)
    {
        return GetBlockValue(level);
    }
    
    // 폭발 보너스 점수
    public static int GetExplodeBonus(int level)
    {
        return GetBlockValue(level) * 2;
    }
    
    // 목표 점수 (업적용)
    public static readonly int[] SCORE_MILESTONES = new int[]
    {
        1000,    // 브론즈
        5000,    // 실버
        10000,   // 골드
        50000,   // 플래티넘
        100000   // 다이아몬드
    };
    
    // 목표 블록 레벨 (업적용)
    public static readonly int[] BLOCK_MILESTONES = new int[]
    {
        8,   // 256
        10,  // 1024
        12,  // 4096
        14,  // 16384
        16   // 65536
    };
}

/// <summary>
/// 게임 난이도 설정
/// </summary>
public enum Difficulty
{
    Easy,
    Normal,
    Hard
}

/// <summary>
/// 난이도별 설정
/// </summary>
public static class DifficultySettings
{
    public static GameDifficulty GetSettings(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return new GameDifficulty
                {
                    gridSize = 6,
                    startingBlocks = 2,
                    explodeLevel = 9,
                    level2Probability = 0.15f
                };
            
            case Difficulty.Normal:
                return new GameDifficulty
                {
                    gridSize = 5,
                    startingBlocks = 3,
                    explodeLevel = 10,
                    level2Probability = 0.10f
                };
            
            case Difficulty.Hard:
                return new GameDifficulty
                {
                    gridSize = 4,
                    startingBlocks = 4,
                    explodeLevel = 11,
                    level2Probability = 0.05f
                };
            
            default:
                return GetSettings(Difficulty.Normal);
        }
    }
}

/// <summary>
/// 난이도 데이터 구조
/// </summary>
[System.Serializable]
public struct GameDifficulty
{
    public int gridSize;
    public int startingBlocks;
    public int explodeLevel;
    public float level2Probability;
}
