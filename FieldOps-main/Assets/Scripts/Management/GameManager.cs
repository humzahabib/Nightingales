using System.Runtime.InteropServices;
using Pathfinding.Util;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Events


    IntEvent GameOverEvent = new IntEvent();


    #endregion


    [SerializeField]
    Level level;

    [SerializeField]
    GameObject pauseMenu;


    GameObject canvas;


    Object currentlyActivePauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("HUD");
        if (level != null)
            level.Initialize();
        EventManager.AddListener(BOOLEVENTS.MAINMENUDISPLAYEVENT, MainMenuDisplayEventHandler);
        EventManager.AddListener(INTEVENTS.GAMEOVEREVENT, GameOverEventHandler);
        EventManager.AddInvoker(INTEVENTS.GAMEOVEREVENT, GameOverEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (level.CheckObjectivesCompleted())
            {
                Debug.LogWarning("LevelCompleted");

            }
        }

        if (level != null)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                currentlyActivePauseMenu = Object.Instantiate(pauseMenu, canvas.transform);
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameOverEvent.Invoke((int)FAILURETYPES.BYPLAYERDEATH);
            
        }

    }


    void ObjectiveCompleteEventHandler(int objectiveNumber)
    {
        foreach (Objective objective in level.objectives)
        {
            if ((int)objective.name == objectiveNumber)
            {
                objective.finished = true;
            }
        }
        Debug.LogWarning(level.objectives[objectiveNumber].congratulationMessage);
    }

    void GameOverEventHandler(int failureType)
    {
        Time.timeScale = 0f;
    }

    void MainMenuDisplayEventHandler(bool value)
    {
        if (value == false)
            Destroy(currentlyActivePauseMenu);

    }

}
