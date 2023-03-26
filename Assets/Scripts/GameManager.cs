using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float parallaxIncrement = 0.1f;
    [SerializeField] private float alignmentThreshold = 0.3f;
    [SerializeField] private float velocityThreshold = 0.01f;
    [SerializeField] private float randomOffsetMin = 20f;
    [SerializeField] private float randomOffsetMax = 100f;

    public static float ParallaxIncrement { get { return instance.parallaxIncrement; } }
    public static float AlignmentThreshold { get { return instance.alignmentThreshold; } }
    public static float RandomOffsetMin { get { return instance.randomOffsetMin; } }
    public static float RandomOffsetMax { get { return instance.randomOffsetMax; } }

    [SerializeField] private float alignmentDelay = 1f;
    private float alignmentTimer = 0f;

    [SerializeField] private float levelTransitionDelay = 1f;

    [SerializeField] private Level[] levels;

    private int levelIndex = 0;
    private IEnumerator transitionRoutine;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Cursor.visible = false;

        foreach (Level level in levels)
            level.gameObject.SetActive(false);

        StartCoroutine(StartTutorial());
    }

    private IEnumerator StartTutorial()
    {
        transitionRoutine = ShowLevel(levelIndex);
        yield return new WaitForSeconds(levelTransitionDelay);

        Movement.Startup();
        StartCoroutine(transitionRoutine);
        transitionRoutine = null;
    }

    private void Update()
    {
        if (transitionRoutine != null)
            return;

        if (levels[levelIndex].IsAligned() && Movement.velocity.magnitude < velocityThreshold)
        {
            if (alignmentTimer >= alignmentDelay)
            {
                transitionRoutine = NextLevel();
                StartCoroutine(transitionRoutine);
            }
            else
                alignmentTimer += Time.deltaTime;
        }
        else
            alignmentTimer = 0f;
    }

    private IEnumerator NextLevel()
    {
        yield return TriggerSuccess(levelIndex);

        yield return new WaitForSeconds(levelTransitionDelay);

        yield return HideLevel(levelIndex);

        levelIndex++;
        if (levelIndex >= levels.Length)
            levelIndex = 1;

        yield return ShowLevel(levelIndex);

        transitionRoutine = null;
    }

    private IEnumerator TriggerSuccess(int index)
    {
        Movement.Stop();

        Level level = levels[index];
        float delay = level.TriggerSuccess();
        yield return new WaitForSeconds(delay);
    }

    private IEnumerator HideLevel(int index)
    {
        Movement.Stop();

        Level level = levels[index];
        float delay = level.Hide();
        yield return new WaitForSeconds(delay);

        level.gameObject.SetActive(false);
    }

    private IEnumerator ShowLevel(int index)
    {
        Movement.Stop();

        Level level = levels[index];
        level.gameObject.SetActive(true);
        float delay = level.Reveal();
        yield return new WaitForSeconds(delay);

        Movement.Startup();
    }
}
