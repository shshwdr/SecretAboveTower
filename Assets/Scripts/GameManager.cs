using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        CSVLoader.Instance.Init();
        ResourceManager.Instance.Init();
        TimerManager.Instance.Init();
        //TimerManager.Instance.CheckAllTimerShouldStart();
    }

    // Update is called once per frame

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
