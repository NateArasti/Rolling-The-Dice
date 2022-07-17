using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void LoadGame() => SceneLoader.LoadScene(SceneLoader.Scene.Game);

    public void ExitGame() => Application.Quit();
}
