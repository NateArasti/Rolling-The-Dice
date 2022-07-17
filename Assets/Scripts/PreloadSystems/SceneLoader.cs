using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum Scene
    {
        MainMenu,
        Game
    }

    private static SceneLoader _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("SceneLoader already exists!");
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public static void LoadScene(Scene scene)
    {
        SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Single);
    }

}
