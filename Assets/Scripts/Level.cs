using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private float parallaxIncrement = 0.1f;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private Sprite[] segments;

    private List<GameObject> layers = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            GameObject go = Instantiate(prefab, transform);
            go.GetComponent<SpriteRenderer>().sprite = segments[i];
            go.GetComponent<Parallax>().Setup((i + 1) * parallaxIncrement, cellSize);
            layers.Add(go);
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cellSize);
    }

#endif
}
