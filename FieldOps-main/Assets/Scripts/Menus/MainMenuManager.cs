using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    static BoolEvent mainMenuDisplayEvent = new BoolEvent();


    [SerializeField]
    Object levelMenu;

    [SerializeField]
    Object credits;

    [SerializeField]
    Object exitConfirmationDialogue;

    [SerializeField]
    Object resetConfirmationDialogue;

    [SerializeField]
    Object optionsMenu;


    [SerializeField]
    GameObject mainMenu;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        EventManager.AddInvoker(BOOLEVENTS.MAINMENUDISPLAYEVENT, mainMenuDisplayEvent);
        EventManager.AddListener(BOOLEVENTS.MAINMENUDISPLAYEVENT, MainMenuDisplayEventHandler);
    }

    void MainMenuDisplayEventHandler(bool value)
    {
        mainMenu.SetActive(value);
    }


    public void OnPlayButtonPressed()
    {
        Object.Instantiate(levelMenu, transform);
        mainMenuDisplayEvent.Invoke(false);
    }

    public void OnOptionsButtonPressed()
    {
        mainMenuDisplayEvent.Invoke(false);
    }

    public void OnCreditsButtonPressed()
    {
        mainMenuDisplayEvent.Invoke(false);
    }

    public void OnQuitButtonPressed()
    {
        Object.Instantiate(exitConfirmationDialogue, transform);
        mainMenuDisplayEvent.Invoke(false);
    }

    public void OnResetButtonPressed()
    {
        Object.Instantiate(resetConfirmationDialogue, transform);

    }

}
