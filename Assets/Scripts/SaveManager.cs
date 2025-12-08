using UnityEngine;
using System;

/// <summary>
/// 게임 데이터 저장/로드 관리
/// </summary>
public class SaveManager : MonoBehaviour
{
    private const string SAVE_KEY = "BlockMergeSave";
    
    /// <summary>
    /// 게임 데이터 저장
    /// </summary>
    public static void SaveGame(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
        
        Debug.Log("게임 저장 완료");
    }
    
    /// <summary>
    /// 게임 데이터 로드
    /// </summary>
    public static GameData LoadGame()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("게임 로드 완료");
            return data;
        }
        
        Debug.Log("저장된 게임 없음");
        return new GameData();
    }
    
    /// <summary>
    /// 저장 데이터 존재 여부
    /// </summary>
    public static bool HasSaveData()
    {
        return PlayerPrefs.HasKey(SAVE_KEY);
    }
    
    /// <summary>
    /// 저장 데이터 삭제
    /// </summary>
    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();
        Debug.Log("저장 데이터 삭제됨");
    }
    
    /// <summary>
    /// 통계 저장
    /// </summary>
    public static void SaveStatistics(GameStatistics stats)
    {
        PlayerPrefs.SetInt("TotalGames", stats.totalGames);
        PlayerPrefs.SetInt("TotalScore", stats.totalScore);
        PlayerPrefs.SetInt("HighestBlock", stats.highestBlockLevel);
        PlayerPrefs.SetInt("TotalMerges", stats.totalMerges);
        PlayerPrefs.SetInt("TotalExplodes", stats.totalExplodes);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// 통계 로드
    /// </summary>
    public static GameStatistics LoadStatistics()
    {
        return new GameStatistics
        {
            totalGames = PlayerPrefs.GetInt("TotalGames", 0),
            totalScore = PlayerPrefs.GetInt("TotalScore", 0),
            highestBlockLevel = PlayerPrefs.GetInt("HighestBlock", 1),
            totalMerges = PlayerPrefs.GetInt("TotalMerges", 0),
            totalExplodes = PlayerPrefs.GetInt("TotalExplodes", 0)
        };
    }
}

/// <summary>
/// 저장할 게임 데이터
/// </summary>
[Serializable]
public class GameData
{
    public int currentScore;
    public int highScore;
    public int maxBlockLevel;
    public BlockData[] blocks;
    public string lastPlayedDate;
    
    public GameData()
    {
        currentScore = 0;
        highScore = 0;
        maxBlockLevel = 1;
        blocks = new BlockData[0];
        lastPlayedDate = DateTime.Now.ToString();
    }
}

/// <summary>
/// 블록 데이터
/// </summary>
[Serializable]
public class BlockData
{
    public int level;
    public int gridX;
    public int gridY;
}

/// <summary>
/// 게임 통계
/// </summary>
[Serializable]
public class GameStatistics
{
    public int totalGames;
    public int totalScore;
    public int highestBlockLevel;
    public int totalMerges;
    public int totalExplodes;
    
    public float averageScore
    {
        get { return totalGames > 0 ? (float)totalScore / totalGames : 0; }
    }
}
