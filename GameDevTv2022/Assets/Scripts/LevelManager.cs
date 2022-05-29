using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Singleton Stuff
    static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogErrorFormat("[Singleton] Trying to instantiate a second instance of singleton class {0} from {1}", GetType().Name, this.gameObject.name);
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
    #endregion

    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void ReloadLevel()
    {
        player.RefreshPlayer();
        //player.isActive = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    public void RefreshLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.LogWarning(SceneManager.sceneCountInBuildSettings);
    }

    public void LoadNextScene()
    {
        if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.LogWarning("HELLO?");
            SceneManager.LoadScene(0);
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
