using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private static LevelLoader _instance;

    [SerializeField] private GameObject[] _levels;
    private GameObject _currentLevelGameObject;
    private int _currentLevelIndex = -1;

    public static int TotalLevelCount => _instance._levels.Length;
    public static int CurrentLevel => _instance._currentLevelIndex;

    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        SwitchToLevel(0);
    }

    public static void SwitchToLevel(int index)
    {
        if(_instance._currentLevelGameObject) Destroy(_instance._currentLevelGameObject);
        _instance._currentLevelGameObject = Instantiate(_instance._levels[index]);
        _instance._currentLevelIndex = index;
    }

    public static void SwitchToNextLevel() => SwitchToLevel(_instance._currentLevelIndex + 1);

    public static void ReloadLevel() => SwitchToLevel(_instance._currentLevelIndex);
}
