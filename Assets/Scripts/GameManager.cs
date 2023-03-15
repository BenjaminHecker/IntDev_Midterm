using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float parallaxIncrement = 0.1f;
    [SerializeField] private float alignmentThreshold = 0.3f;
    [SerializeField] private float velocityThreshold = 0.01f;

    public static float ParallaxIncrement { get { return instance.parallaxIncrement; } }
    public static float AlignmentThreshold { get { return instance.alignmentThreshold; } }

    [SerializeField] private float successDelay = 1f;
    private float successTimer = 0f;

    [SerializeField] private Level[] levels;

    private int levelIndex = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        foreach (Level level in levels)
            level.gameObject.SetActive(false);

        ShowLevel(levelIndex);
    }

    private void Update()
    {
        if (levels[levelIndex].IsAligned() && Movement.velocity.magnitude < velocityThreshold)
        {
            if (successTimer >= successDelay)
                NextLevel();
            else
                successTimer += Time.deltaTime;
        }
        else
            successTimer = 0f;
    }

    private void NextLevel()
    {
        HideLevel(levelIndex);

        levelIndex++;
        if (levelIndex >= levels.Length)
            levelIndex = 0;

        ShowLevel(levelIndex);
    }

    private void HideLevel(int index)
    {
        Level level = levels[index];
        level.gameObject.SetActive(false);
    }

    private void ShowLevel(int index)
    {
        Level level = levels[index];
        level.gameObject.SetActive(true);
        level.Setup();
        level.RandomizeOffsets();
    }
}
