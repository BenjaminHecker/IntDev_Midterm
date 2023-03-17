using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
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

    public void Setup()
    {
        if (layers.Count > 0)
            return;

        float height = Camera.main.orthographicSize * 2;
        float width = height * Camera.main.aspect;

        float startX = (int) (-width / 2f / cellSize.x) * cellSize.x;
        float startY = (int) (-height / 2f / cellSize.y) * cellSize.y;

        float endX = width / 2f + cellSize.x * 0;
        float endY = height / 2f + cellSize.y * 0;

        for (int i = 0; i < segments.Length; i++) {
            Layer layer = new Layer();

            for (float x = startX; x < endX; x += cellSize.x)
            {
                for (float y = startY; y < endY; y += cellSize.y)
                {
                    GameObject go = Instantiate(prefab, transform.position + new Vector3(x, y), Quaternion.identity, transform);
                    go.GetComponent<Parallax>().Setup(segments[i], (i + 1) * GameManager.ParallaxIncrement, cellSize);
                    layer.items.Add(go);
                }
            }

            layers.Add(layer);
        }
    }

    public void RandomizeOffsets()
    {
        float range = segments.Length * 100f;
        Vector2 randomOffset = new Vector2(Random.Range(-range, range), Random.Range(-range, range));

        foreach (Layer layer in layers)
        {
            foreach (GameObject go in layer.items)
            {
                Parallax parallax = go.GetComponent<Parallax>();
                parallax.offset = randomOffset * parallax.parallaxEffect;
                parallax.AdjustOffset();
            }
        }

        if (IsAligned())
            RandomizeOffsets();
    }

    public bool IsAligned()
    {
        foreach (Layer layer in layers)
        {
            Vector2 offset = layer.GetOffset();

            if (offset.magnitude > GameManager.AlignmentThreshold)
                return false;
        }

        return true;
    }

    public float TriggerSuccess()
    {
        float successDelay = 0f;

        foreach (Layer layer in layers)
        {
            foreach (GameObject go in layer.items)
            {
                float delay = go.GetComponent<Parallax>().TriggerSuccess();
                if (delay > successDelay)
                    successDelay = delay;
            }
        }

        return successDelay;
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cellSize);
    }

#endif
}
