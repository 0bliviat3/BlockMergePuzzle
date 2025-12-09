using UnityEngine;

/// <summary>
/// 오디오 관리 클래스
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("오디오 소스")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    
    [Header("오디오 클립")]
    public AudioClip bgmClip;
    public AudioClip mergeSound;
    public AudioClip explodeSound;
    public AudioClip gameOverSound;
    public AudioClip comboSound;        // ⭐ 추가
    public AudioClip clickSound;        // ⭐ 추가
    
    [Header("설정")]
    [Range(0f, 1f)]
    public float bgmVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.7f;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        InitializeAudioSources();
        LoadVolumeSettings();
    }
    
    private void Start()
    {
        PlayBGM();
    }
    
    /// <summary>
    /// 오디오 소스 초기화
    /// </summary>
    private void InitializeAudioSources()
    {
        // BGM Source
        if (bgmSource == null)
        {
            GameObject bgmObj = new GameObject("BGM_Source");
            bgmObj.transform.SetParent(transform);
            bgmSource = bgmObj.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
            Debug.Log("✓ BGM AudioSource 자동 생성");
        }
        
        // SFX Source
        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFX_Source");
            sfxObj.transform.SetParent(transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            Debug.Log("✓ SFX AudioSource 자동 생성");
        }
        
        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
    }
    
    /// <summary>
    /// BGM 재생
    /// </summary>
    public void PlayBGM()
    {
        if (bgmSource != null && bgmClip != null && !bgmSource.isPlaying)
        {
            bgmSource.clip = bgmClip;
            bgmSource.Play();
            Debug.Log("🎵 BGM 재생 시작");
        }
    }
    
    /// <summary>
    /// BGM 정지
    /// </summary>
    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
            Debug.Log("🎵 BGM 정지");
        }
    }
    
    /// <summary>
    /// SFX 재생 (디버그 로그 포함)
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null)
        {
            Debug.LogError("❌ SFX Source가 null입니다!");
            return;
        }
        
        if (clip == null)
        {
            Debug.LogWarning("⚠️ AudioClip이 null입니다! 사운드를 재생할 수 없습니다.");
            return;
        }
        
        sfxSource.PlayOneShot(clip, sfxVolume);
        Debug.Log($"🔊 SFX 재생: {clip.name} (볼륨: {sfxVolume * 100}%)");
    }
    
    /// <summary>
    /// 병합 사운드 재생
    /// </summary>
    public void PlayMergeSound()
    {
        if (mergeSound != null)
        {
            PlaySFX(mergeSound);
        }
        else
        {
            Debug.LogWarning("⚠️ Merge Sound가 연결되지 않았습니다!");
        }
    }
    
    /// <summary>
    /// 폭발 사운드 재생
    /// </summary>
    public void PlayExplodeSound()
    {
        if (explodeSound != null)
        {
            PlaySFX(explodeSound);
        }
        else
        {
            Debug.LogWarning("⚠️ Explode Sound가 연결되지 않았습니다!");
        }
    }
    
    /// <summary>
    /// 게임 오버 사운드 재생
    /// </summary>
    public void PlayGameOverSound()
    {
        if (gameOverSound != null)
        {
            PlaySFX(gameOverSound);
        }
        else
        {
            Debug.LogWarning("⚠️ Game Over Sound가 연결되지 않았습니다!");
        }
    }
    
    /// <summary>
    /// 콤보 사운드 재생 ⭐
    /// </summary>
    public void PlayComboSound()
    {
        if (comboSound != null)
        {
            PlaySFX(comboSound);
        }
        else
        {
            Debug.LogWarning("⚠️ Combo Sound가 연결되지 않았습니다!");
        }
    }
    
    /// <summary>
    /// 클릭 사운드 재생 ⭐
    /// </summary>
    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            PlaySFX(clickSound);
        }
        else
        {
            Debug.LogWarning("⚠️ Click Sound가 연결되지 않았습니다!");
        }
    }
    
    /// <summary>
    /// BGM 볼륨 설정
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null)
        {
            bgmSource.volume = bgmVolume;
        }
        SaveVolumeSettings();
        Debug.Log($"🎵 BGM 볼륨: {bgmVolume * 100}%");
    }
    
    /// <summary>
    /// SFX 볼륨 설정
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
        SaveVolumeSettings();
        Debug.Log($"🔊 SFX 볼륨: {sfxVolume * 100}%");
    }
    
    /// <summary>
    /// 볼륨 설정 저장
    /// </summary>
    private void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// 볼륨 설정 불러오기
    /// </summary>
    private void LoadVolumeSettings()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
        
        if (bgmSource != null) bgmSource.volume = bgmVolume;
        if (sfxSource != null) sfxSource.volume = sfxVolume;
        
        Debug.Log($"🎵 볼륨 로드 - BGM: {bgmVolume * 100}%, SFX: {sfxVolume * 100}%");
    }
    
    /// <summary>
    /// BGM 볼륨 반환
    /// </summary>
    public float GetBGMVolume()
    {
        return bgmVolume;
    }
    
    /// <summary>
    /// SFX 볼륨 반환
    /// </summary>
    public float GetSFXVolume()
    {
        return sfxVolume;
    }
}
