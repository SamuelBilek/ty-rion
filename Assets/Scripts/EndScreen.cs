using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    private float _timerSeconds = 1f;
    private bool _canSwitch = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (_canSwitch && Input.anyKeyDown)
        {
            EnvironmentalProps.Instance.SetScore(0);
            SceneManager.LoadScene("Main Menu");
        }
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(_timerSeconds);
        _canSwitch = true;
    }
}
