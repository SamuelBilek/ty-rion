using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        // switch levels if N is pressed (released actually)
        #if UNITY_EDITOR
            if (Input.GetKeyUp(KeyCode.N))
            {
                int sceneIndex = SceneManager.GetActiveScene().buildIndex;
                if (sceneIndex >= SceneManager.sceneCountInBuildSettings - 1)
                {
                    Debug.LogWarning("No more levels");
                }
                else
                {
                    SceneManager.LoadScene(sceneIndex + 1);
                }
            }
        #endif
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu();
        }
    }

    public static void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public static void LevelSelect()
    {
        SceneManager.LoadScene("Level Select");
    }

    public static void DeathScreen()
    {
        SceneManager.LoadScene("Death Screen");
    }

    public static void Options()
    {
        SceneManager.LoadScene("Options");
    }

    public static void Controls()
    {
        SceneManager.LoadScene("Controls");
    }

    public static void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public static void Victory()
    {
        SceneManager.LoadScene("Win Screen");
    }

    public static void Exit()
    {
        Application.Quit();
    }

    public static void Level1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public static void Level2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public static void Level3()
    {
        SceneManager.LoadScene("Level 3");
    }

}
