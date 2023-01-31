using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{

    [SerializeField]
    GameObject loadingScreen;

    AsyncOperation operation;

    [SerializeField]
    Slider loadingBar;

    [SerializeField]
    TextMeshProUGUI loadingText;

    bool completed = false;

    [SerializeField]
    TextMeshProUGUI anyButtonText;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        EventManager.AddListener(ASYNCOPERATIONEVENTS.LOADINGSTARTEVENT, LoadingStartedEventhandler);

    }


    void LoadingStartedEventhandler(AsyncOperation _operation)
    {
        loadingScreen.SetActive(true);
        operation = _operation;
        operation.allowSceneActivation = false;
        operation.completed += OnSceneActivation;
        StartCoroutine(ProgressLoading());
    }


    IEnumerator ProgressLoading()
    {
        yield return null;

        while (!operation.isDone)
        {

            loadingBar.value = operation.progress;
            loadingText.text = Mathf.Round((operation.progress * 100)).ToString() + "%";

            if (operation.progress >= 0.9f)
            {

                loadingText.gameObject.SetActive(false);
                loadingBar.gameObject.SetActive(false);
                anyButtonText.gameObject.SetActive(true);

                if (Input.anyKey)
                {
                    operation.allowSceneActivation = true;
                }

            }

            yield return null;
        }


    }

    void OnSceneActivation(AsyncOperation _unused)
    {
        loadingScreen.SetActive(false);
    }


}
