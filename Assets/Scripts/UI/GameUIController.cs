using UnityEngine;
using UnityEngine.Events;

public class GameUIController : MonoBehaviour
{
    private const string VolumeKey = "Volume";
    private const string LastPassedLevelKey = "LevelProgress";

    public static bool Paused { get; private set; }

    [Header("Events")]
    [SerializeField] private UnityEvent _onSettingsOpenEvent;
    [SerializeField] private UnityEvent _onLevelChooseOpenEvent;
    [SerializeField] private UnityEvent _onLevelPassed;
    [SerializeField] private UnityEvent _onLastLevelPassed;
    [SerializeField] private UnityEvent _onLevelFailed;
    [SerializeField] private UnityEvent _closeMenuEvent;
    [Header("Level Choose Params")]
    [SerializeField] private Transform _levelGridPivot;
    [SerializeField] private LevelChooseIcon _levelChoosePrefab;
    [SerializeField] private LevelChooseIcon _lastLevelChoosePrefab;

    private void Start()
    {
        AudioListener.volume = Mathf.Clamp01(PlayerPrefs.GetFloat(VolumeKey, 0.5f));
        GridController.OnCurrentGridFinished.AddListener(HandleLevelPass);
        GridController.OnCurrentGridFailed.AddListener(() => _onLevelFailed.Invoke());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenSettings();
        }
    }

    private void HandleLevelPass()
    {
        if (LevelLoader.CurrentLevel == LevelLoader.TotalLevelCount - 1)
        {
            _onLastLevelPassed.Invoke();
        }
        else
        {
            _onLevelPassed.Invoke();
        }
    }

    public void ExitToMainMenu()
    {
        Paused = false;
        SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
    }

    public void CloseMenu()
    {
        Paused = false;
        _closeMenuEvent.Invoke();
    }

    public void OpenSettings()
    {
        Paused = true;
        _onSettingsOpenEvent.Invoke();
    }
    public void OpenLevelChoose()
    {
        Paused = true;
        _onLevelChooseOpenEvent.Invoke();
    }

    public void GenerateLevelChooseUI()
    {
        foreach (Transform child in _levelGridPivot)
        {
            Destroy(child.gameObject);
        }

        var lastPassedLevel = PlayerPrefs.GetInt(LastPassedLevelKey, 0);
        var i = 0;
        for (; i <= lastPassedLevel && i < LevelLoader.TotalLevelCount; i++)
        {
            if(i == LevelLoader.TotalLevelCount - 1)
                Instantiate(_lastLevelChoosePrefab, _levelGridPivot).SetLevelData(i, false, HandleLevelChoose);
            else
                Instantiate(_levelChoosePrefab, _levelGridPivot).SetLevelData(i, false, HandleLevelChoose);
        }
        for (; i < LevelLoader.TotalLevelCount; i++)
        {
            if (i == LevelLoader.TotalLevelCount - 1)
                Instantiate(_lastLevelChoosePrefab, _levelGridPivot).SetLevelData(i, true, HandleLevelChoose);
            else
                Instantiate(_levelChoosePrefab, _levelGridPivot).SetLevelData(i, true, HandleLevelChoose);
        }
    }

    public void HandleLevelChoose(int index)
    {
        LevelLoader.SwitchToLevel(index);
        CloseMenu();
    }

    public void HandleVolumeChange(float volume)
    {
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(VolumeKey, volume);
        AudioListener.volume = volume;
        PlayerPrefs.Save();
    }

    public void LoadNextLevel()
    {
        LevelLoader.SwitchToNextLevel();
        if(PlayerPrefs.GetInt(LastPassedLevelKey, 0) < LevelLoader.CurrentLevel)
        {
            PlayerPrefs.SetInt(LastPassedLevelKey, LevelLoader.CurrentLevel);
            PlayerPrefs.Save();
        }
    }

    public void RestartLevel()
    {
        LevelLoader.ReloadLevel();
        CloseMenu();
    }
}
