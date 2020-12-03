using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainGameStart : MonoBehaviour
{
    public void newGameUI()
    {
        SceneManager.LoadScene("Levels");
    }

    public void instructionsUI() => SceneManager.LoadScene("InstructionScreen");

    public void ReturnUI()
    {
        SceneManager.LoadScene("StartGameScreen");
    }

    public void quitUI()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE)
                Application.Quit();
        #endif
    }

    public void level1()
    {
        SceneManager.LoadScene("Lvl_1");
    }

    public void level2()
    {
        SceneManager.LoadScene("Lvl_2");
    }
}

