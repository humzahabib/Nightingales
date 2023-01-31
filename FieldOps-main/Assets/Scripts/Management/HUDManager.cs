using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{

    #region Player Possession HUD

    [Header("Player Possession Hud")]

    [SerializeField]
    Image gunIcon;

    [SerializeField]
    TextMeshProUGUI gunBulletText;

    [SerializeField]
    Slider playerHealthBar;

    [SerializeField]
    Image gunReloadFillingImage;


    #endregion

    [Space]

    #region AlternateRalityHUD

    [SerializeField]
    Image universalFlash;

    bool isFlashing = false;

    [SerializeField]
    Image verseWatchDialImage;

    #endregion

    #region Dialogue

    [Header("Dialogue Hud")]

    [SerializeField]
    TextMeshProUGUI dialogueText;

    [SerializeField]
    Image dialogueNarratorImage;

    [Space]

    #endregion


    #region ObjectiveDisplay

    [Header("Objective Hud")]

    int objectivesCount;

    [SerializeField]
    TextMeshProUGUI objectiveText;

    [Space]


    #endregion

    #region GameOverHud

    [Header("GameOver Hud")]

    [SerializeField]
    RectTransform gameOverPanel;

    [SerializeField]
    TextMeshProUGUI gameOverPanelTitleText;

    [SerializeField]
    TextMeshProUGUI statsText;

    [SerializeField]
    TextMeshProUGUI statsLabelText;

    [SerializeField]
    Image statsGraphImage;


    #endregion


    // Start is called before the first frame update
    void Start()
    {

        EventManager.AddListener(SCRIPTABLEOBJECTSEVENTS.WEAPONSWITCHEDTOHUDEVENT, PlayerWeaponSwitchedEventHandler);
        EventManager.AddListener(SCRIPTABLEOBJECTSEVENTS.GUNSTATUSUPDATEDEVENT, PlayerGunStatusUpdatedEventHandler);
        EventManager.AddListener(FLOATEVENTS.SCREENFLASHEVENT, ScreenFlashEventHandler);
        EventManager.AddListener(SCRIPTABLEOBJECTSEVENTS.DIALOGUEDISPLAYEVENT, DialogueDisplayEventHandler);
        EventManager.AddListener(SCRIPTABLEOBJECTSEVENTS.OBJECTIVEADDEDEVENT, ObjectiveAddedEventHandler);
        EventManager.AddListener(SCRIPTABLEOBJECTSEVENTS.OBJECTIVECOMPLETEDEVENT, ObjectiveCompletedEventHandler);
        EventManager.AddListener(INTEVENTS.GAMEOVEREVENT, GameOverEventHandler);
        EventManager.AddListener(INTEVENTS.PlayerHealthChangedEvent, PlayerHealthChangedEventHandler);
        EventManager.AddListener(FLOATEVENTS.VERSEWATCHPOWERCHANGEDEVENT, VerseWatchEnergeChangedEventHandler);
        EventManager.AddListener(SCRIPTABLEOBJECTSEVENTS.PLAYERSPECSTOHUDEVENT, PlayerSpecsToHUDEventHandler);
    }


    private void OnDestroy()
    {
        EventManager.RemoveListener(SCRIPTABLEOBJECTSEVENTS.WEAPONSWITCHEDTOHUDEVENT, PlayerGunStatusUpdatedEventHandler);
        EventManager.RemoveListener(SCRIPTABLEOBJECTSEVENTS.GUNSTATUSUPDATEDEVENT, PlayerGunStatusUpdatedEventHandler);
        EventManager.RemoveListener(FLOATEVENTS.SCREENFLASHEVENT, ScreenFlashEventHandler);
        EventManager.RemoveListener(SCRIPTABLEOBJECTSEVENTS.DIALOGUEDISPLAYEVENT, DialogueDisplayEventHandler);
        EventManager.RemoveListener(SCRIPTABLEOBJECTSEVENTS.OBJECTIVEADDEDEVENT, ObjectiveAddedEventHandler);
        EventManager.RemoveListener(SCRIPTABLEOBJECTSEVENTS.OBJECTIVECOMPLETEDEVENT, ObjectiveCompletedEventHandler);
        EventManager.RemoveListener(INTEVENTS.GAMEOVEREVENT, GameOverEventHandler);
        EventManager.RemoveListener(INTEVENTS.PlayerHealthChangedEvent, PlayerHealthChangedEventHandler);
        EventManager.RemoveListener(FLOATEVENTS.VERSEWATCHPOWERCHANGEDEVENT, VerseWatchEnergeChangedEventHandler);
    }



    void PlayerWeaponSwitchedEventHandler(ScriptableObject gunSpecs)
    {
        GunSpecs currentGun = (GunSpecs)gunSpecs;
        gunIcon.sprite = currentGun.icon;
        gunBulletText.text = (currentGun.remainingBulletsInMag + "/" + currentGun.remainingBulletsInPocket);

    }


    void PlayerGunStatusUpdatedEventHandler(ScriptableObject gunSpecs)
    {
        GunSpecs currentGun = (GunSpecs)gunSpecs;
        gunIcon.sprite = currentGun.icon;
        gunBulletText.text = (currentGun.remainingBulletsInMag + "/" + currentGun.remainingBulletsInPocket);
        gunReloadFillingImage.fillAmount = currentGun.elapsedSeconds / currentGun.reloadTime;
        if (currentGun.reloading)
            gunReloadFillingImage.gameObject.SetActive(true);
        else
            gunReloadFillingImage.gameObject.SetActive(false);
    }



    void GameOverEventHandler(int deathType)
    {
        gameOverPanel.gameObject.SetActive(true);

        switch (deathType)
        {
            case (int)FAILURETYPES.BYPLAYERDEATH:
                {
                    gameOverPanelTitleText.text = "You are dead..!!!";
                    break;
                }
        }
    }

    void PlayerSpecsToHUDEventHandler(ScriptableObject playerSpecs)
    {
        PlayerSpecs specs = (PlayerSpecs)playerSpecs;
        statsLabelText.text += "Time Played" + endLine;
        statsText.text += specs.timePlayed + endLine;
        statsLabelText.text += "Accuracy" + endLine;
        statsText.text += specs.accuracy.ToString() + endLine;
        statsLabelText.text += "Witness" + endLine;
        statsText.text += specs.witnesses.ToString() + endLine;
        statsLabelText.text += "Enemies Killed" + endLine;
        statsText.text += specs.enemiesKilled.ToString() + endLine;
        statsLabelText.text += "Shots Fired" + endLine;
        statsText.text += specs.shotsFired.ToString() + endLine;
        statsLabelText.text += "Noise" + endLine;
        statsText.text += specs.noiseProduced + endLine;

    }

    void GameOverContinueButtonPressed()
    {
        gameOverPanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }


    void PlayerHealthChangedEventHandler(int health)
    {
        playerHealthBar.value = health;
    }






    void MessageDisplayEventHandler(ScriptableObject dialogue)
    {

        StartCoroutine(DialogueProcess((Dialogue)dialogue));
    }

    void ScreenFlashEventHandler(float delay)
    {
        if (!isFlashing)
            StartCoroutine(AlphaLerpImageColor(delay, universalFlash));
        else
            return;
    }

    IEnumerator<WaitForSeconds> AlphaLerpImageColor(float delay, Image image)
    {
        isFlashing = true;
        while (image.color.a <= 1f)
        {
            Color imageColor = image.color;
            imageColor.a += 1 / delay * Time.deltaTime;
            image.color = imageColor;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Mathf.Clamp(image.color.a, 0, 1);
        yield return new WaitForSeconds(delay / 20);
        while (image.color.a >= 0f)
        {
            Color imageColor = image.color;
            imageColor.a -= 1 / delay * Time.deltaTime;
            image.color = imageColor;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Mathf.Clamp(image.color.a, 0, 1);
        isFlashing = false;
        yield break;
    }


    void VerseWatchEnergeChangedEventHandler(float currentValue)
    {
        verseWatchDialImage.fillAmount = currentValue / 100;
    }


    #region DialogueHandling

    bool isProcessingDialogue = false;

    void DialogueDisplayEventHandler(ScriptableObject dialogue)
    {
        if (isProcessingDialogue)
            StopCoroutine("DialogueProcess");
        StartCoroutine(DialogueProcess((Dialogue)dialogue));
    }

    IEnumerator<WaitForSeconds> DialogueProcess(Dialogue dialogue)
    {
        dialogueNarratorImage.gameObject.SetActive(true);

        dialogueNarratorImage.sprite = dialogue.narratorImage;
        isProcessingDialogue = true;
        for (int i = 0; i <= dialogue.message.Length - 1; i++)
        {
            dialogueText.text = dialogue.message.Substring(0, i);

            yield return new WaitForSeconds(.05f);
        }

        yield return new WaitForSeconds(2f);

        if (dialogue.daughterDialogue != null)
        {
            DialogueDisplayEventHandler(dialogue.daughterDialogue);
        }
        else
        {
            dialogueText.text = null;
            dialogueNarratorImage.sprite = null;
            isProcessingDialogue = false;
            dialogueNarratorImage.gameObject.SetActive(false);
        }

    }

    IEnumerator<WaitForSeconds> UploadDaughterDialogue(Dialogue _daughterDialogue)
    {
        yield return new WaitForSeconds(1f);
    }

    #endregion


    string endLine = "\n";
    string cutStart = "<s>";

    string cutEnd = "</s>";


    #region ObjectiveHandling

    void ObjectiveAddedEventHandler(ScriptableObject _objective)
    {
        Objective objective = (Objective)_objective;

        objectivesCount++;


        objectiveText.text += objective.name.ToString() + endLine;


    }

    void ObjectiveCompletedEventHandler(ScriptableObject _objective)
    {
        Objective objective = (Objective)_objective;

        objectivesCount -= 1;
        string objectiveName = objective.name.ToString();
        int startIndex = objectiveText.text.IndexOf(objectiveName);
        int endIndex = startIndex + objectiveName.Length;

        string beforeString = objectiveText.text.Substring(0, startIndex);
        string afterString = objectiveText.text.Substring(endIndex + 1);

        objectiveText.text = beforeString + cutStart + objectiveName + cutEnd + afterString;

    }

    #endregion


}
