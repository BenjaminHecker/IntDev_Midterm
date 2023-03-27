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
    [SerializeField] private float stretchSnapDelayFactor = 0.5f;
    [SerializeField] private float stretchMagnitude = 0.2f;
    [SerializeField] private float stretchAnimationTime = 0.8f;
    [SerializeField] private AnimationCurve stretchAnimationCurve;
    [SerializeField] private float snapAnimationTime = 0.2f;
    [SerializeField] private AnimationCurve snapAnimationCurve;

    [Space]
    [SerializeField] private float hideDelayFactor = 0.01f;
    [SerializeField] private float hideSlideMagnitude = 1f;
    [SerializeField] private float hideAnimationTime = 0.5f;
    [SerializeField] private AnimationCurve hideAnimationCurve;

    [Space]
    [SerializeField] private float revealDelayFactor = 0.01f;
    [SerializeField] private float revealSlideMagnitude = 1f;
    [SerializeField] private float revealAnimationTime = 0.5f;
    [SerializeField] private AnimationCurve revealAnimationCurve;

    [Space]
    [SerializeField] private float shakeDuration = 0.05f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem trailParticles;
    [SerializeField] private Color trailColor;

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
        float delay = transform.position.magnitude * stretchSnapDelayFactor;
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
        float delay = transform.position.magnitude * hideDelayFactor;
        StartCoroutine(HideRoutine(delay));
        return delay + hideAnimationTime;
    }

    private IEnumerator HideRoutine(float delay)
    {
        ParticleSystem.MainModule trail = trailParticles.main;

        yield return new WaitForSeconds(delay);

        float hideAnimTimer = 0f;

        Vector2 originalPos = transform.position;
        Vector2 targetPos = originalPos + Random.insideUnitCircle.normalized * hideSlideMagnitude * parallaxEffect;

        while (hideAnimTimer < hideAnimationTime)
        {
            float ratio = hideAnimationCurve.Evaluate(hideAnimTimer / hideAnimationTime);

            transform.position = Vector2.Lerp(originalPos, targetPos, ratio);
            sRender.color = Color.Lerp(Color.white, Color.clear, ratio);
            trail.startColor = Color.Lerp(trailColor, Color.clear, ratio);

            hideAnimTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetPos;
        sRender.color = Color.clear;
        trail.startColor = Color.clear;
    }

    public float TriggerReveal()
    {
        AdjustOffset();
        float delay = transform.position.magnitude * revealDelayFactor;
        StartCoroutine(RevealRoutine(delay));
        return delay + revealAnimationTime;
    }

    private IEnumerator RevealRoutine(float delay)
    {
        ParticleSystem.MainModule trail = trailParticles.main;

        sRender.color = Color.clear;
        trail.startColor = Color.clear;

        yield return new WaitForSeconds(delay);

        float revealAnimTimer = 0f;

        Vector2 targetPos = transform.position;
        Vector2 initialPos = targetPos + Random.insideUnitCircle.normalized * revealSlideMagnitude * parallaxEffect;

        while (revealAnimTimer < revealAnimationTime)
        {
            float ratio = revealAnimationCurve.Evaluate(revealAnimTimer / revealAnimationTime);

            transform.position = Vector2.Lerp(initialPos, targetPos, ratio);
            sRender.color = Color.Lerp(Color.clear, Color.white, ratio);
            trail.startColor = Color.Lerp(Color.clear, trailColor, ratio);

            revealAnimTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetPos;
        sRender.color = Color.white;
        trail.startColor = trailColor;
    }
}
