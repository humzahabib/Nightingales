using UnityEngine;

public class PlayerMover : MonoBehaviour
{

    #region Events

    ScriptableObjectEvent weaponSwitchToHudEvent;

    #endregion





    #region Fields

    #region Refrences For States
    [SerializeField]
    Animator anim;

    Rigidbody2D rb2d;

    #endregion

    #region State List

    PlayerState[] playerStates = new PlayerState[2];

    #endregion

    #region State Switching Maintainance

    int currentStateNumber = 0;

    #endregion


    #region State Specific Variables To Use As Arguments

    #region GunSpecs

    [SerializeField]
    GunSpecs machineGunSpecs;


    [SerializeField]
    GunSpecs silencedPistolSpecs;

    GunSpecs silencedStateConfig;
    GunSpecs machineGunStateConfig;

    #endregion


    public Transform bulletPosition;

    #region WalkSpeed For States

    public float HoldWalkSpeed;
    public float PistolWalkSpeed;
    public float MachineGunWalkSpeed;

    #endregion

    #region Bullets For States

    public GameObject Bullet;

    #endregion

    #region Animator String Fields

    string holdAnimTrigger = "Hold";
    string pistolAnimTrigger = "Pistol";
    string silencedAnimTrigger = "Silenced";
    string machineGunAnimTrigger = "MachineGun";
    string reloadAnimTrigger = "Reload";

    #endregion

    #endregion

    #endregion

    #region StartUp

    void Awake()
    {
        silencedStateConfig = ScriptableObject.CreateInstance<GunSpecs>();
        machineGunStateConfig = ScriptableObject.CreateInstance<GunSpecs>();

        silencedStateConfig.automatic = silencedPistolSpecs.automatic;
        silencedStateConfig.icon = silencedPistolSpecs.icon;
        silencedStateConfig.maximumBulletsInPocket = silencedPistolSpecs.maximumBulletsInPocket;
        silencedStateConfig.magSize = silencedPistolSpecs.magSize;
        silencedStateConfig.remainingBulletsInMag = silencedPistolSpecs.remainingBulletsInMag;
        silencedStateConfig.remainingBulletsInPocket = silencedPistolSpecs.remainingBulletsInPocket;
        silencedStateConfig.reloadTime = silencedPistolSpecs.reloadTime;
        silencedStateConfig.noise = silencedPistolSpecs.noise;
        silencedStateConfig.coolDownSeconds = silencedPistolSpecs.coolDownSeconds;

        machineGunStateConfig.icon = machineGunSpecs.icon;
        machineGunStateConfig.automatic = machineGunSpecs.automatic;
        machineGunStateConfig.maximumBulletsInPocket = machineGunSpecs.maximumBulletsInPocket;
        machineGunStateConfig.magSize = machineGunSpecs.magSize;
        machineGunStateConfig.remainingBulletsInMag = machineGunSpecs.remainingBulletsInMag;
        machineGunStateConfig.remainingBulletsInPocket = machineGunSpecs.remainingBulletsInPocket;
        machineGunStateConfig.reloadTime = machineGunSpecs.reloadTime;
        machineGunStateConfig.noise = machineGunSpecs.noise;
        machineGunStateConfig.coolDownSeconds = machineGunSpecs.coolDownSeconds;
    }


    private void Start()
    {
        #region RefrencingComponents


        rb2d = GetComponent<Rigidbody2D>();

        #endregion

        #region Player States Implementation



        playerStates[0] = new Silenced(this.gameObject, anim, PistolWalkSpeed, Bullet,
         silencedAnimTrigger, bulletPosition, rb2d, silencedStateConfig);
        playerStates[1] = new MachineGun(this.gameObject, anim, MachineGunWalkSpeed, Bullet,
         machineGunAnimTrigger, reloadAnimTrigger, bulletPosition, rb2d, machineGunStateConfig);

        #endregion

        #region EventsRegistry


        foreach (PlayerState state in playerStates)
        {
            state.WeaponSwitchedEvent += SwitchState;
        }


        weaponSwitchToHudEvent = new ScriptableObjectEvent();
        EventManager.AddInvoker(SCRIPTABLEOBJECTSEVENTS.WEAPONSWITCHEDTOHUDEVENT,
         weaponSwitchToHudEvent);


        weaponSwitchToHudEvent.Invoke(playerStates[currentStateNumber].gunSpecs);

        #endregion

    }


    #endregion

    private void FixedUpdate()
    {
        playerStates[currentStateNumber].Process();
    }

    private void OnDestroy()
    {
        EventManager.RemoveInvoker(SCRIPTABLEOBJECTSEVENTS.WEAPONSWITCHEDTOHUDEVENT, weaponSwitchToHudEvent);
        foreach (PlayerState state in playerStates)
        {
            state.WeaponSwitchedEvent -= SwitchState;
        }
    }

    #region Private Functions

    void SwitchState(float input)
    {
        if (currentStateNumber + input > playerStates.Length - 1)
        {
            currentStateNumber = 0;
        }
        else if (currentStateNumber + input < 0)
        {
            currentStateNumber = playerStates.Length - 1;
        }
        else
        {
            currentStateNumber += (int)input;
        }
        weaponSwitchToHudEvent.Invoke(playerStates[currentStateNumber].gunSpecs);
    }

    #endregion
}
