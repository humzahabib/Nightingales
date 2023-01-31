using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField]
    GameObject optionsMenu;


    AsyncOperationEvent levelLoadingEvent = new AsyncOperationEvent();

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        Time.timeScale = 0f;
        EventManager.AddInvoker(ASYNCOPERATIONEVENTS.LOADINGSTARTEVENT, levelLoadingEvent);

    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void OnResumeButtonClicked()
    {
        Destroy(gameObject);
    }

    public void OnRestartButtonClicked()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        levelLoadingEvent.Invoke(SceneManager.LoadSceneAsync(activeSceneIndex));
    }

    public void OnOptionsButtonClicked()
    {
        Object.Instantiate(optionsMenu, transform.parent);
        Destroy(gameObject);
    }

    public void OnQuitToMenuButtonClicked()
    {
        levelLoadingEvent.Invoke(SceneManager.LoadSceneAsync("Main Menu"));
    }


    public void OnQuitToDesktopButtonClicked()
    {
        Application.Quit();
    }
}
