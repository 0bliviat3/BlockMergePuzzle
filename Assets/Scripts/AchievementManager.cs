using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 업적 시스템
/// </summary>
public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }
    
    private List<Achievement> achievements = new List<Achievement>();
    
    // 업적 달성 이벤트
    public event Action<Achievement> OnAchievementUnlocked;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAchievements();
            LoadAchievements();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 업적 초기화
    /// </summary>
    private void InitializeAchievements()
    {
        achievements = new List<Achievement>
        {
            // 점수 업적
            new Achievement("first_score", "첫 걸음", "1,000점 달성", AchievementType.Score, 1000),
            new Achievement("score_5k", "실력자", "5,000점 달성", AchievementType.Score, 5000),
            new Achievement("score_10k", "전문가", "10,000점 달성", AchievementType.Score, 10000),
            new Achievement("score_50k", "마스터", "50,000점 달성", AchievementType.Score, 50000),
            new Achievement("score_100k", "그랜드마스터", "100,000점 달성", AchievementType.Score, 100000),
            
            // 블록 레벨 업적
            new Achievement("block_256", "256 달성", "256 블록 생성", AchievementType.BlockLevel, 8),
            new Achievement("block_1024", "1024 달성", "1024 블록 생성", AchievementType.BlockLevel, 10),
            new Achievement("block_4096", "4096 달성", "4096 블록 생성", AchievementType.BlockLevel, 12),
            new Achievement("block_16384", "16384 달성", "16384 블록 생성", AchievementType.BlockLevel, 14),
            
            // 병합 업적
            new Achievement("merge_10", "초보 병합사", "10회 병합", AchievementType.Merge, 10),
            new Achievement("merge_100", "숙련 병합사", "100회 병합", AchievementType.Merge, 100),
            new Achievement("merge_1000", "전설의 병합사", "1,000회 병합", AchievementType.Merge, 1000),
            
            // 폭발 업적
            new Achievement("explode_1", "첫 폭발", "첫 블록 폭발", AchievementType.Explode, 1),
            new Achievement("explode_10", "폭발 전문가", "10회 폭발", AchievementType.Explode, 10),
            new Achievement("explode_50", "폭발의 달인", "50회 폭발", AchievementType.Explode, 50),
            
            // 콤보 업적
            new Achievement("combo_5", "콤보 시작", "5 콤보 달성", AchievementType.Combo, 5),
            new Achievement("combo_10", "콤보 마스터", "10 콤보 달성", AchievementType.Combo, 10),
            
            // 게임 플레이 업적
            new Achievement("game_10", "단골 플레이어", "10게임 플레이", AchievementType.Games, 10),
            new Achievement("game_50", "열정적인 플레이어", "50게임 플레이", AchievementType.Games, 50),
            new Achievement("game_100", "중독된 플레이어", "100게임 플레이", AchievementType.Games, 100),
        };
    }
    
    /// <summary>
    /// 업적 체크 및 해제
    /// </summary>
    public void CheckAchievement(AchievementType type, int value)
    {
        foreach (Achievement achievement in achievements)
        {
            if (achievement.type == type && !achievement.isUnlocked)
            {
                if (value >= achievement.targetValue)
                {
                    UnlockAchievement(achievement);
                }
            }
        }
    }
    
    /// <summary>
    /// 업적 해제
    /// </summary>
    private void UnlockAchievement(Achievement achievement)
    {
        achievement.isUnlocked = true;
        achievement.unlockedDate = DateTime.Now.ToString();
        
        SaveAchievements();
        
        // 이벤트 발생
        OnAchievementUnlocked?.Invoke(achievement);
        
        // UI 표시
        ShowAchievementPopup(achievement);
        
        Debug.Log($"업적 달성: {achievement.name}");
    }
    
    /// <summary>
    /// 업적 팝업 표시
    /// </summary>
    private void ShowAchievementPopup(Achievement achievement)
    {
        // TODO: UI 팝업 구현
        Debug.Log($"🏆 업적 달성!\n{achievement.name}\n{achievement.description}");
    }
    
    /// <summary>
    /// 업적 저장
    /// </summary>
    private void SaveAchievements()
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            Achievement achievement = achievements[i];
            PlayerPrefs.SetInt($"Achievement_{achievement.id}_Unlocked", achievement.isUnlocked ? 1 : 0);
            PlayerPrefs.SetString($"Achievement_{achievement.id}_Date", achievement.unlockedDate);
        }
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// 업적 로드
    /// </summary>
    private void LoadAchievements()
    {
        foreach (Achievement achievement in achievements)
        {
            achievement.isUnlocked = PlayerPrefs.GetInt($"Achievement_{achievement.id}_Unlocked", 0) == 1;
            achievement.unlockedDate = PlayerPrefs.GetString($"Achievement_{achievement.id}_Date", "");
        }
    }
    
    /// <summary>
    /// 모든 업적 가져오기
    /// </summary>
    public List<Achievement> GetAllAchievements()
    {
        return new List<Achievement>(achievements);
    }
    
    /// <summary>
    /// 달성한 업적 개수
    /// </summary>
    public int GetUnlockedCount()
    {
        int count = 0;
        foreach (Achievement achievement in achievements)
        {
            if (achievement.isUnlocked) count++;
        }
        return count;
    }
    
    /// <summary>
    /// 전체 업적 개수
    /// </summary>
    public int GetTotalCount()
    {
        return achievements.Count;
    }
    
    /// <summary>
    /// 업적 진행률 (백분율)
    /// </summary>
    public float GetCompletionPercentage()
    {
        return (float)GetUnlockedCount() / GetTotalCount() * 100f;
    }
}

/// <summary>
/// 업적 타입
/// </summary>
public enum AchievementType
{
    Score,      // 점수
    BlockLevel, // 블록 레벨
    Merge,      // 병합 횟수
    Explode,    // 폭발 횟수
    Combo,      // 콤보
    Games       // 게임 플레이 횟수
}

/// <summary>
/// 업적 클래스
/// </summary>
[Serializable]
public class Achievement
{
    public string id;
    public string name;
    public string description;
    public AchievementType type;
    public int targetValue;
    public bool isUnlocked;
    public string unlockedDate;
    
    public Achievement(string id, string name, string description, AchievementType type, int targetValue)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.type = type;
        this.targetValue = targetValue;
        this.isUnlocked = false;
        this.unlockedDate = "";
    }
}
