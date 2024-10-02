using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField]
    private float _scoreGoal;
    // Start is called before the first frame update
    void Start()
    {
        _scoreGoal = _scoreGoal + EnvironmentalProps.Instance.GetScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnvironmentalProps.Instance.GetScore() >= _scoreGoal)
        {
            SoundManager.Instance.StopAlarm();
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
    }
}
