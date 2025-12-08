using UnityEngine;
using System.Collections;

/// <summary>
/// 이펙트 관리 클래스
/// </summary>
public class EffectManager : MonoBehaviour
{
    [Header("파티클 프리팹")]
    public GameObject mergeEffectPrefab;
    public GameObject explodeEffectPrefab;
    public GameObject hintEffectPrefab;
    public GameObject milestoneEffectPrefab;
    
    [Header("설정")]
    public float effectLifetime = 1f;
    
    [Header("오디오")]
    public AudioSource audioSource;
    public AudioClip mergeSound;
    public AudioClip explodeSound;
    public AudioClip milestoneSound;
    
    /// <summary>
    /// 병합 이펙트 재생
    /// </summary>
    public void PlayMergeEffect(Vector3 position)
    {
        if (mergeEffectPrefab != null)
        {
            GameObject effect = Instantiate(mergeEffectPrefab, position, Quaternion.identity);
            Destroy(effect, effectLifetime);
        }
        
        // 사운드 재생
        PlaySound(mergeSound);
        
        // 간단한 파티클 효과 (프리팹이 없는 경우)
        CreateSimpleParticles(position, Color.yellow, 10);
    }
    
    /// <summary>
    /// 폭발 이펙트 재생
    /// </summary>
    public void PlayExplodeEffect(Vector3 position)
    {
        if (explodeEffectPrefab != null)
        {
            GameObject effect = Instantiate(explodeEffectPrefab, position, Quaternion.identity);
            Destroy(effect, effectLifetime);
        }
        
        // 사운드 재생
        PlaySound(explodeSound);
        
        // 간단한 파티클 효과
        CreateSimpleParticles(position, Color.red, 20);
        
        // 화면 흔들림
        StartCoroutine(ScreenShake(0.3f, 0.2f));
    }
    
    /// <summary>
    /// 힌트 이펙트 재생
    /// </summary>
    public void PlayHintEffect(Vector3 position)
    {
        if (hintEffectPrefab != null)
        {
            GameObject effect = Instantiate(hintEffectPrefab, position, Quaternion.identity);
            Destroy(effect, effectLifetime);
        }
        
        // 간단한 반짝임 효과
        CreatePulseEffect(position);
    }
    
    /// <summary>
    /// 마일스톤 이펙트 재생
    /// </summary>
    public void PlayMilestoneEffect()
    {
        // 사운드 재생
        PlaySound(milestoneSound);
        
        // 화면 전체 이펙트
        StartCoroutine(ScreenFlash(Color.yellow, 0.5f));
    }
    
    /// <summary>
    /// 사운드 재생
    /// </summary>
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    
    /// <summary>
    /// 간단한 파티클 생성
    /// </summary>
    private void CreateSimpleParticles(Vector3 position, Color color, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject particle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            particle.transform.position = position;
            particle.transform.localScale = Vector3.one * 5f;
            
            Renderer renderer = particle.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
            
            // 랜덤 방향으로 날리기
            Vector3 randomDirection = Random.insideUnitSphere;
            float speed = Random.Range(50f, 150f);
            
            LeanTween.move(particle, position + randomDirection * speed, 0.5f)
                .setEase(LeanTweenType.easeOutQuad);
            
            LeanTween.scale(particle, Vector3.zero, 0.5f)
                .setEase(LeanTweenType.easeInQuad)
                .setOnComplete(() =>
                {
                    Destroy(particle);
                });
        }
    }
    
    /// <summary>
    /// 펄스 효과 생성
    /// </summary>
    private void CreatePulseEffect(Vector3 position)
    {
        GameObject pulse = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pulse.transform.position = position;
        pulse.transform.localScale = Vector3.one * 50f;
        
        Renderer renderer = pulse.GetComponent<Renderer>();
        if (renderer != null)
        {
            Color color = Color.cyan;
            color.a = 0.5f;
            renderer.material.color = color;
        }
        
        // 확대 및 투명화
        LeanTween.scale(pulse, Vector3.one * 120f, 0.8f)
            .setEase(LeanTweenType.easeOutQuad);
        
        LeanTween.alpha(pulse, 0f, 0.8f)
            .setOnComplete(() =>
            {
                Destroy(pulse);
            });
    }
    
    /// <summary>
    /// 화면 흔들림
    /// </summary>
    private IEnumerator ScreenShake(float duration, float magnitude)
    {
        Camera cam = Camera.main;
        if (cam == null) yield break;
        
        Vector3 originalPosition = cam.transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            
            cam.transform.position = originalPosition + new Vector3(x, y, 0);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        cam.transform.position = originalPosition;
    }
    
    /// <summary>
    /// 화면 플래시
    /// </summary>
    private IEnumerator ScreenFlash(Color color, float duration)
    {
        // TODO: UI 패널을 사용한 플래시 효과 구현
        yield return new WaitForSeconds(duration);
    }
}
