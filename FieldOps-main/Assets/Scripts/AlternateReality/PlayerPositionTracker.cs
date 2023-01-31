using UnityEngine;
using Cinemachine;

public class PlayerPositionTracker : MonoBehaviour
{
    Transform player;
    Vector2 offSetPlayer;

    IntEvent UniversalStatsEvent = new IntEvent();


    bool inReality = true;

    CinemachineVirtualCamera realityCamera;
    CinemachineVirtualCamera alternateRealityCamera;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offSetPlayer = transform.position - player.position;
        realityCamera = GameObject.FindGameObjectWithTag("RealCamera")
        .GetComponent<CinemachineVirtualCamera>();
        alternateRealityCamera = GameObject.FindGameObjectWithTag("AlternateCamera")
        .GetComponent<CinemachineVirtualCamera>();
        EventManager.AddListener(INTEVENTS.REALITYCHANGEDEVENT, RealityChangedEventHandler);
        EventManager.AddListener(INTEVENTS.GAMEOVEREVENT, GameOverEventHandler);
        EventManager.AddInvoker(INTEVENTS.UNIVERSALSTATSEVENT, UniversalStatsEvent);

    }


    void LateUpdate()
    {
        transform.position = (Vector2)player.position + offSetPlayer;
    }


    void OnDisable()
    {
        EventManager.RemoveListener(INTEVENTS.REALITYCHANGEDEVENT, RealityChangedEventHandler);
        EventManager.RemoveListener(INTEVENTS.GAMEOVEREVENT, GameOverEventHandler);
        EventManager.RemoveInvoker(INTEVENTS.UNIVERSALSTATSEVENT, UniversalStatsEvent);
    }

    void RealityChangedEventHandler(int unused)
    {
        Vector2 playerPosition = player.position;
        player.position = transform.position;
        transform.position = playerPosition;
        offSetPlayer = transform.position - player.position;
        if (inReality)
        {
            inReality = false;
            alternateRealityCamera.ForceCameraPosition(player.position, realityCamera.transform.rotation);
            alternateRealityCamera.MoveToTopOfPrioritySubqueue();
            alternateRealityCamera.Follow = player;
            realityCamera.Follow = gameObject.transform;

        }
        else
        {
            inReality = true;
            realityCamera.ForceCameraPosition(player.position, realityCamera.transform.rotation);
            realityCamera.MoveToTopOfPrioritySubqueue();
            alternateRealityCamera.Follow = gameObject.transform;
            realityCamera.Follow = player;
        }
        UniversalStatsEvent.Invoke(inReality == true ? 1 : -1);
    }

    void GameOverEventHandler(int unused)
    {
        Destroy(gameObject);
    }

}
