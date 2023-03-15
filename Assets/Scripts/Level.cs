using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private float parallaxIncrement = 0.1f;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private Sprite[] segments;

    class Layer
    {
        public List<GameObject> items = new List<GameObject>();

        public Vector2 GetOffset()
        {
            if (items.Count > 0)
                return items[0].GetComponent<Parallax>().OffsetPosition();
            else
                return Vector2.zero;
        }
    }
    private List<Layer> layers = new List<Layer>();

    private void Awake()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = height * Camera.main.aspect;

        float startX = (int) (-width / 2f / cellSize.x - 1) * cellSize.x;
        float startY = (int) (-height / 2f / cellSize.y - 1) * cellSize.y;

        float endX = width / 2f + cellSize.x;
        float endY = height / 2f + cellSize.y;

        for (int i = 0; i < segments.Length; i++) {
            Layer layer = new Layer();

            for (float x = startX; x < endX; x += cellSize.x)
            {
                for (float y = startY; y < endY; y += cellSize.y)
                {
                    GameObject go = Instantiate(prefab, transform.position + new Vector3(x, y), Quaternion.identity, transform);
                    go.GetComponent<SpriteRenderer>().sprite = segments[i];
                    go.GetComponent<Parallax>().Setup((i + 1) * parallaxIncrement, cellSize);
                    layer.items.Add(go);
                }
            }

            layers.Add(layer);
        }
    }

    public bool IsAligned()
    {
        foreach (Layer layer in layers)
        {
            Vector2 offset = layer.GetOffset();

            if (offset.magnitude > 0.3f)
                return false;
        }

        return true;
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cellSize);
    }

#endif
}
