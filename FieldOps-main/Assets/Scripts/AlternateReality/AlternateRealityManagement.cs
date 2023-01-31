using UnityEngine;

public class AlternateRealityManagement : MonoBehaviour
{


    float powerConsumptionSpeed = 10;

    float powerRefillSpeed = 3;


    float currentVerseWatchPower = 100;

    float MaxVerseWatchPower = 100;

    FloatEvent VerseWatchPowerChangedEvent = new FloatEvent();

    bool inReality = true;

    IntEvent RealityChangedEvent = new IntEvent();
    FloatEvent ScreenFlashEvent = new FloatEvent();


    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddInvoker(FLOATEVENTS.SCREENFLASHEVENT, ScreenFlashEvent);
        EventManager.AddInvoker(INTEVENTS.REALITYCHANGEDEVENT, RealityChangedEvent);
        EventManager.AddInvoker(FLOATEVENTS.VERSEWATCHPOWERCHANGEDEVENT, VerseWatchPowerChangedEvent);
        EventManager.AddListener(INTEVENTS.UNIVERSALSTATSEVENT, UniversalStatsEventHandler);
    }

    private void OnDisable()
    {
        EventManager.RemoveInvoker(FLOATEVENTS.SCREENFLASHEVENT, ScreenFlashEvent);
        EventManager.RemoveInvoker(INTEVENTS.REALITYCHANGEDEVENT, RealityChangedEvent);
        EventManager.RemoveInvoker(FLOATEVENTS.VERSEWATCHPOWERCHANGEDEVENT, VerseWatchPowerChangedEvent);
        EventManager.RemoveListener(INTEVENTS.UNIVERSALSTATSEVENT, UniversalStatsEventHandler);
    }

    // Update is called once per frame
    void Update()
    {
        if (inReality && currentVerseWatchPower < MaxVerseWatchPower)
        {
            currentVerseWatchPower = Mathf.Min(100, currentVerseWatchPower + Time.deltaTime * powerRefillSpeed);
            VerseWatchPowerChangedEvent.Invoke(currentVerseWatchPower);
        }
        else if (!inReality && currentVerseWatchPower > 0)
        {
            currentVerseWatchPower = Mathf.Max(0, currentVerseWatchPower - Time.deltaTime * powerConsumptionSpeed);
            VerseWatchPowerChangedEvent.Invoke(currentVerseWatchPower);
        }
        else if (!inReality && currentVerseWatchPower <= 0)
        {
            RealityChangedEvent.Invoke(0);
            ScreenFlashEvent.Invoke(0.05f);
            inReality = !inReality;
        }

        if (Input.GetButtonDown("RealityShift") && (currentVerseWatchPower > MaxVerseWatchPower / 2 || !inReality))
        {
            RealityChangedEvent.Invoke(0);
            ScreenFlashEvent.Invoke(.05f);
            inReality = !inReality;
            currentVerseWatchPower -= 20;
        }
    }




    void UniversalStatsEventHandler(int realitystats)
    {
        //if (realitystats > 1)
        //    inReality = true;
        //else
        //    inReality = false;
    }
}
