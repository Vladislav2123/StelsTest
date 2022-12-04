using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Zenject;

public class LevelsManager : MonoBehaviour
{
    [Header("Fading")]
    [SerializeField] private float _fadeDuration = 1;
    [SerializeField] private float _restartDelay = 0.5f;

    [Inject] private UIFade _uiFade;

    private const string LEVEL_NUMBER_KEY = "LevelNumber";

    public int LevelNumber
    {
        get => PlayerPrefs.GetInt(LEVEL_NUMBER_KEY, 1);
        set => PlayerPrefs.SetInt(LEVEL_NUMBER_KEY, value);
    }

    public void CompleteLevel()
    {
        LevelNumber++;
    }

    public void Restart()
    {
        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        _uiFade.FadeIn(_fadeDuration);

        yield return new WaitForSeconds(_fadeDuration + _restartDelay);

        RestartScene();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
