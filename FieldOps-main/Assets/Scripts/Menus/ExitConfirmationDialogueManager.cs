using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitConfirmationDialogueManager : MonoBehaviour
{
    BoolEvent MainMenuDisplayEvent = new BoolEvent();

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        EventManager.AddInvoker(BOOLEVENTS.MAINMENUDISPLAYEVENT, MainMenuDisplayEvent);
    }

    void OnDestroy()
    {
        EventManager.RemoveInvoker(BOOLEVENTS.MAINMENUDISPLAYEVENT, MainMenuDisplayEvent);
    }

    public void OnYesButtonClicked()
    {
        Application.Quit();
    }

    public void OnNoButtonClicked()
    {

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            MainMenuDisplayEvent.Invoke(true);
        }
        Destroy(gameObject);
    }
}
