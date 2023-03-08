using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxEffect;

    //private float width, height;
    //private Vector3 startPos;

    void Start()
    {
        //startPos = transform.position;
        //width = GetComponent<SpriteRenderer>().bounds.size.x;
        //height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        transform.position = Movement.origin * parallaxEffect;

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
