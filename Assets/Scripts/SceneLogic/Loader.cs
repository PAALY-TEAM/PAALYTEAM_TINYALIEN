using System;
using UnityEngine.SceneManagement;

public static class Loader {
    public enum Scene {
        MainMenu2,
        GameScene,
        Settings,
        HelpScene,
        LoadingScene
    }
    
    private static Scene _targetScene;
    
    public static void Load(Scene targetScene) {
        Loader._targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback() {
        SceneManager.LoadScene(_targetScene.ToString());
    }

}