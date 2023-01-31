using System.Collections.Generic;
using UnityEngine;


public class PlayerStatsKeeper : MonoBehaviour
{
    PlayerSpecs currentPlayerSpecs;


    ScriptableObjectEvent playerSpecsToHUDEvent = new ScriptableObjectEvent();

    int bulletsHit;


    List<GameObject> witnesses = new List<GameObject>();

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        currentPlayerSpecs = new PlayerSpecs();
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddInvoker(SCRIPTABLEOBJECTSEVENTS.PLAYERSPECSTOHUDEVENT, playerSpecsToHUDEvent);


        EventManager.AddListener(SCRIPTABLEOBJECTSEVENTS.GUNFIREDEVENT, GunFiredEventHandler);
        EventManager.AddListener(FLOATVECTOR3EVENTS.NOISEPRODUCEDEVENT, NoiseProducedEventHandler);
        EventManager.AddListener(EMPTYEVENTS.ENEMYKILLEDEVENT, EnemyKilledEventHandler);
        EventManager.AddListener(EMPTYEVENTS.BULLETNOTHITEVENT, BulletNotHitEventHandler);
        EventManager.AddListener(GAMEOBJECTEVENTS.WITNESSADDEDEVENT, WitnessAddedEventHandler);
        EventManager.AddListener(GAMEOBJECTEVENTS.WITNESSREMOVEDEVENT, WitnessRemovedEventHandler);
        EventManager.AddListener(INTEVENTS.GAMEOVEREVENT, GameOverEventHandler);
    }

    // Update is called once per frame
    void Update()
    {
    }




    void GunFiredEventHandler(ScriptableObject _gunSpecs)
    {
        GunSpecs gunSpecs = (GunSpecs)_gunSpecs;

        currentPlayerSpecs.shotsFired++;

        bulletsHit++;

        currentPlayerSpecs.noiseProduced += gunSpecs.noise;

    }

    void NoiseProducedEventHandler(float intensity, Vector3 unused)
    {
        currentPlayerSpecs.noiseProduced += intensity;
    }

    void EnemyKilledEventHandler()
    {
        currentPlayerSpecs.enemiesKilled++;
    }

    void BulletNotHitEventHandler()
    {
        bulletsHit--;
    }


    void GameOverEventHandler(int unused)
    {
        currentPlayerSpecs.timePlayed = MyMath.GetTimeString(Time.timeSinceLevelLoad);
        if (currentPlayerSpecs.shotsFired != 0)
            currentPlayerSpecs.accuracy = (bulletsHit * 100) / currentPlayerSpecs.shotsFired;
        else
            currentPlayerSpecs.accuracy = 0;
        currentPlayerSpecs.witnesses = witnesses.Count;

        playerSpecsToHUDEvent.Invoke(currentPlayerSpecs);

    }

    void WitnessAddedEventHandler(GameObject witness)
    {
        if (witnesses.Contains(witness))
            return;
        else
            witnesses.Add(witness);

    }

    void WitnessRemovedEventHandler(GameObject witness)
    {
        if (witnesses.Contains(witness))
            witnesses.Remove(witness);
        else
            return;
    }

}