using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{


    AsyncOperationEvent LoadingStartedEvent = new AsyncOperationEvent();

    int selectedIndex;

    [SerializeField]
    Button playButton;


    [SerializeField]
    string levelNames;

    [SerializeField]
    Dropdown levelMenu;


    [SerializeField]
    TextMeshProUGUI levelDiscription;


    [SerializeField]
    Level[] levels;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        EventManager.AddInvoker(ASYNCOPERATIONEVENTS.LOADINGSTARTEVENT, LoadingStartedEvent);
        levelMenu.onValueChanged.AddListener(LevelMenuValueChangedHandler);

        foreach (Level level in levels)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = level.name.ToString();
            levelMenu.options.Add(optionData);
        }
    }

    void LevelMenuValueChangedHandler(int value)
    {
        selectedIndex = value;
        levelDiscription.text = levels[selectedIndex].discription;

    }

    public void OnPlayButtonClicked()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levels[selectedIndex].name.ToString());
        LoadingStartedEvent.Invoke(operation);
    }

    public void OnBackButtonClicked()
    {
        LoadingStartedEvent.Invoke(SceneManager.LoadSceneAsync("MainMenu"));
    }





}
