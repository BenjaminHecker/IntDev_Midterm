using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxEffect;

    [HideInInspector] public Vector2 offset = Vector2.zero;
    private Vector2 startOffset = Vector2.zero;
    private Vector2 cellSize;

    private void Start()
    {
        startOffset = transform.position;
    }

    public void Setup(float parallaxEffect, Vector2 cellSize)
    {
        this.parallaxEffect = parallaxEffect;
        this.cellSize = cellSize;
    }

    private void Update()
    {
        if (transform.position.x > startOffset.x + cellSize.x / 2f)
            offset.x -= cellSize.x;
        else if (transform.position.x < startOffset.x - cellSize.x / 2f)
            offset.x += cellSize.x;

        if (transform.position.y > startOffset.y + cellSize.y / 2f)
            offset.y -= cellSize.y;
        else if (transform.position.y < startOffset.y - cellSize.y / 2f)
            offset.y += cellSize.y;

        transform.position = Movement.origin * parallaxEffect + offset + startOffset;
    }
    
    public Vector2 OffsetPosition()
    {
        return Movement.origin * parallaxEffect + offset;
    }
}
