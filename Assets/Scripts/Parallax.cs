using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxEffect;

    [SerializeField] private float successDelay = 1f;

    [HideInInspector] public Vector2 offset = Vector2.zero;
    private Vector2 startOffset = Vector2.zero;
    private Vector2 cellSize;

    public void Setup(float parallaxEffect, Vector2 cellSize)
    {
        startOffset = transform.position;

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
            if (transform.position.x > startOffset.x + cellSize.x / 2f)
                offset.x -= cellSize.x;
            else if (transform.position.x < startOffset.x - cellSize.x / 2f)
                offset.x += cellSize.x;
            else
                withinX = true;

            if (transform.position.y > startOffset.y + cellSize.y / 2f)
                offset.y -= cellSize.y;
            else if (transform.position.y < startOffset.y - cellSize.y / 2f)
                offset.y += cellSize.y;
            else
                withinY = true;

            transform.position = Movement.origin * parallaxEffect + offset + startOffset;
        }
    }
    
    public Vector2 OffsetPosition()
    {
        return Movement.origin * parallaxEffect + offset;
    }

    public void TriggerSuccess()
    {
        StartCoroutine(SuccessRoutine());
    }

    private IEnumerator SuccessRoutine()
    {
        float successTimer = 0f;
        Vector2 originalOffset = offset;
        Vector2 targetOffset = -Movement.origin * parallaxEffect;

        while (successTimer < successDelay)
        {
            float ratio = Mathf.Pow(successTimer / successDelay, 10);
            offset = Vector2.Lerp(originalOffset, targetOffset, ratio);
            transform.position = Movement.origin * parallaxEffect + offset + startOffset;

            successTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
