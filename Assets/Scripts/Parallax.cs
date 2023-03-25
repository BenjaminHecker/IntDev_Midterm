using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Movement")]
    public float parallaxEffect;

    [HideInInspector] public Vector2 offset = Vector2.zero;
    private Vector2 startPos = Vector2.zero;
    private Vector2 cellSize;

    [Header("Animations")]
    [SerializeField] private float animationDelayFactor = 0.5f;

    [Space]
    [SerializeField] private float stretchMagnitude = 0.2f;
    [SerializeField] private float stretchAnimationTime = 0.8f;
    [SerializeField] private AnimationCurve stretchAnimationCurve;
    [SerializeField] private float snapAnimationTime = 0.2f;
    [SerializeField] private AnimationCurve snapAnimationCurve;

    [Space]
    [SerializeField] private float hideSlideMagnitude = 1f;
    [SerializeField] private float hideAnimationTime = 0.5f;
    [SerializeField] private AnimationCurve hideAnimationCurve;

    [Space]
    [SerializeField] private float revealSlideMagnitude = 1f;
    [SerializeField] private float revealAnimationTime = 0.5f;
    [SerializeField] private AnimationCurve revealAnimationCurve;

    [Space]
    [SerializeField] private float shakeDuration = 0.05f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem trailParticles;

    private SpriteRenderer sRender;

    private void Awake()
    {
        sRender = GetComponent<SpriteRenderer>();
    }

    public void Setup(Sprite sprite, float parallaxEffect, Vector2 cellSize)
    {
        startPos = transform.position;

        sRender.sprite = sprite;

        ParticleSystemRenderer trailRenderer = trailParticles.GetComponent<ParticleSystemRenderer>();
        Material trailMat = new Material(trailRenderer.material);
        trailMat.mainTexture = sprite.texture;
        trailRenderer.material = trailMat;

        this.parallaxEffect = parallaxEffect;
        this.cellSize = cellSize;
    }

    private void Update()
    {
        if (Movement.run)
            AdjustOffset();
    }

    public void AdjustOffset()
    {
        bool withinX = false, withinY = false;
        while (!withinX || !withinY)
        {
            if (transform.position.x > startPos.x + cellSize.x / 2f)
                offset.x -= cellSize.x;
            else if (transform.position.x < startPos.x - cellSize.x / 2f)
                offset.x += cellSize.x;
            else
                withinX = true;

            if (transform.position.y > startPos.y + cellSize.y / 2f)
                offset.y -= cellSize.y;
            else if (transform.position.y < startPos.y - cellSize.y / 2f)
                offset.y += cellSize.y;
            else
                withinY = true;

            transform.position = Movement.origin * parallaxEffect + offset + startPos;
        }
    }
    
    public Vector2 OffsetPosition()
    {
        return Movement.origin * parallaxEffect + offset;
    }

    public float TriggerSuccess()
    {
        float delay = startPos.magnitude * animationDelayFactor;
        StartCoroutine(SuccessRoutine(delay));
        return delay + stretchAnimationTime + snapAnimationTime + shakeDuration;
    }

    private IEnumerator SuccessRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        float successAnimTimer = 0f;

        Vector2 originalPos = transform.position;
        Vector2 targetPos = startPos;
        Vector2 stretchPos = targetPos + (originalPos - targetPos).normalized * stretchMagnitude * parallaxEffect;

        while (successAnimTimer < stretchAnimationTime)
        {
            float ratio = stretchAnimationCurve.Evaluate(successAnimTimer / stretchAnimationTime);

            transform.position = Vector2.Lerp(originalPos, stretchPos, ratio);

            successAnimTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        successAnimTimer = 0f;
        while (successAnimTimer < snapAnimationTime)
        {
            float ratio = snapAnimationCurve.Evaluate(successAnimTimer / snapAnimationTime);

            transform.position = Vector2.Lerp(stretchPos, targetPos, ratio);

            successAnimTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetPos;

        ScreenShake.TriggerShake(shakeDuration, shakeMagnitude);
    }

    public float TriggerHide()
    {
        float delay = startPos.magnitude * animationDelayFactor;
        StartCoroutine(HideRoutine(delay));
        return delay + hideAnimationTime;
    }

    private IEnumerator HideRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        trailParticles.Stop();

        float hideAnimTimer = 0f;

        Vector2 originalPos = transform.position;
        Vector2 targetPos = originalPos + Random.insideUnitCircle * hideSlideMagnitude * parallaxEffect;

        while (hideAnimTimer < hideAnimationTime)
        {
            float ratio = hideAnimationCurve.Evaluate(hideAnimTimer / hideAnimationTime);

            transform.position = Vector2.Lerp(originalPos, targetPos, ratio);
            sRender.color = Color.Lerp(Color.white, Color.clear, ratio);

            hideAnimTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetPos;
        sRender.color = Color.clear;
    }

    public float TriggerReveal()
    {
        float delay = startPos.magnitude * animationDelayFactor;
        StartCoroutine(RevealRoutine(delay));
        return delay + revealAnimationTime;
    }

    private IEnumerator RevealRoutine(float delay)
    {
        yield return new WaitForSeconds(0f);
    }
}
