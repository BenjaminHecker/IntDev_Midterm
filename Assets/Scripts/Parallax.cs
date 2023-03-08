using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxEffect;

    private Vector2 offset = Vector2.zero;
    private Vector2 cellSize;

    public void Setup(float parallaxEffect, Vector2 cellSize)
    {
        this.parallaxEffect = parallaxEffect;
        this.cellSize = cellSize;
    }

    void Update()
    {
        if (transform.position.x > cellSize.x / 2f)
            offset.x -= cellSize.x;

        if (transform.position.y > cellSize.y / 2f)
            offset.y -= cellSize.y;


        //Vector2 temp = Movement.origin * (1 - parallaxEffect);

        //if (temp.x > offset.x + cellSize.x)
        //    offset.x += cellSize.x;
        //else if (temp.x < offset.x - cellSize.x)
        //    offset.x -= cellSize.x;

        //if (temp.y > offset.y + cellSize.y)
        //    offset.y += cellSize.y;
        //else if (temp.y < offset.y - cellSize.y)
        //    offset.y -= cellSize.y;

        transform.position = Movement.origin * parallaxEffect + offset;

        //Vector3 temp = Movement.origin * (1 - parallaxEffect);
        //Vector3 dist = Movement.origin * parallaxEffect;

        //if (temp.x > startPos.x + width)
        //    startPos.x += width;
        //else if (temp.x < startPos.x - width)
        //    startPos.x -= width;

        //if (temp.y > startPos.y + height)
        //    startPos.y += height;
        //else if (temp.y < startPos.y - height)
        //    startPos.y -= height;

        //transform.position = new Vector3(startPos.x + dist.x, startPos.y + dist.y, transform.position.z);
    }
}
