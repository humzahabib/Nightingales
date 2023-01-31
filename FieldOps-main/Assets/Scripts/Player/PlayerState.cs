using UnityEngine;

#region Delegates

public delegate void WeaponSwitched(float input);


#endregion

#region Base Player State

public class PlayerState : StatesBase
{

    #region EVENTS


    public event WeaponSwitched WeaponSwitchedEvent;

    FloatVectorEvent noiseProducedEvent = new FloatVectorEvent();
    ScriptableObjectEvent gunFiredEvent = new ScriptableObjectEvent();
    ScriptableObjectEvent gunStatusUpdatedEvent = new ScriptableObjectEvent();


    #endregion

    #region Fields

    #region Components Used

    protected Animator anim;
    Rigidbody2D rb2d;

    #endregion

    #region State Specific Fields



    protected GameObject bullet;
    protected new WEAPONSTATE name;
    protected string animTrigger;
    protected Transform bulletPosition;

    #endregion

    #region Status


    public GunSpecs gunSpecs;
    protected GameObject player;

    #endregion


    #endregion

    #region Constructor


    public PlayerState(GameObject _player, Animator _anim,
        float _walkSpeed, GameObject _bullet, string _animTrigger,
         Transform _bulletPosition, Rigidbody2D _rb2d, GunSpecs _gunSpecs)
    {
        player = _player;
        anim = _anim;
        walkSpeed = _walkSpeed;
        bullet = _bullet;
        animTrigger = _animTrigger;
        bulletPosition = _bulletPosition;
        rb2d = _rb2d;
        gunSpecs = _gunSpecs;

    }


    #endregion

    #region Functions

    #region Private Methods

    public void Process()
    {
        if (stage == EVENT.ENTER) { Enter(); }
        if (stage == EVENT.UPDATE) { Update(); }
        if (stage == EVENT.EXIT) { Exit(); }
    }
    #endregion

    #region PrivateMethods


    protected override void Enter()
    {
        EventManager.AddInvoker(FLOATVECTOR3EVENTS.NOISEPRODUCEDEVENT, noiseProducedEvent);
        EventManager.AddInvoker(SCRIPTABLEOBJECTSEVENTS.GUNFIREDEVENT, gunFiredEvent);
        EventManager.AddInvoker(SCRIPTABLEOBJECTSEVENTS.GUNSTATUSUPDATEDEVENT, gunStatusUpdatedEvent);
        anim.SetTrigger(animTrigger);
        base.Enter();
    }

    protected override void Update()
    {
        if (gunSpecs.reloading)
            HandleReload();
        HandleCoolDown();
        base.Update();
    }

    protected override void Exit()
    {
        anim.ResetTrigger(animTrigger);
        stage = EVENT.ENTER;
        EventManager.RemoveInvoker(FLOATVECTOR3EVENTS.NOISEPRODUCEDEVENT, noiseProducedEvent);
        EventManager.RemoveInvoker(SCRIPTABLEOBJECTSEVENTS.GUNFIREDEVENT, gunFiredEvent);
        EventManager.RemoveInvoker(SCRIPTABLEOBJECTSEVENTS.GUNSTATUSUPDATEDEVENT, gunStatusUpdatedEvent);
    }

    #endregion

    #region StuffDoing

    protected void GetMouseWheel()
    {
        bool input = Input.GetButtonDown("WeaponSwitch");
        if (input)
        {
            Exit();
            if (WeaponSwitchedEvent != null)
                WeaponSwitchedEvent(Input.GetAxis("WeaponSwitch"));

        }
    }

    protected virtual void Fire()
    {
        if (gunSpecs.remainingBulletsInMag > 0)
        {
            if (!gunSpecs.automatic)
            {
                if (Input.GetButtonDown("Fire1") && !gunSpecs.reloading && gunSpecs.cooledDown)
                {
                    FireBulletWithNoise();
                }
            }
            else if (gunSpecs.automatic)
            {
                if (Input.GetAxisRaw("Fire1") != 0 && gunSpecs.cooledDown)
                {
                    FireBulletWithNoise();
                }
            }

        }
        else if (gunSpecs.remainingBulletsInMag <= 0)
        {
            Reload();
        }
    }

    protected virtual void FireBulletWithNoise()
    {
        Vector2 bulletRandomOffset = new Vector2(Random.Range(-0.1f, 0.1f), 0);
        bulletRandomOffset = bulletPosition.transform.InverseTransformVector(bulletRandomOffset);

        gunSpecs.remainingBulletsInMag--;
        GameObject.Instantiate(bullet, (Vector2)bulletPosition.transform.position + bulletRandomOffset, player.transform.rotation);
        gunFiredEvent.Invoke(gunSpecs);
        gunStatusUpdatedEvent.Invoke(gunSpecs);
        noiseProducedEvent.Invoke(gunSpecs.noise, player.transform.position);
        gunSpecs.cooledDown = false;
    }

    void Reload()
    {
        gunSpecs.reloading = true;
    }


    public void HandleReload()
    {
        if (gunSpecs.remainingBulletsInPocket > 0)
        {
            if (!gunSpecs.cooledDown)
            {
                gunSpecs.cooledDown = true;
                gunSpecs.elapsedSeconds = 0;
            }
            gunSpecs.elapsedSeconds += Time.fixedDeltaTime;
            gunStatusUpdatedEvent.Invoke(gunSpecs);
            if (gunSpecs.elapsedSeconds >= gunSpecs.reloadTime)
            {
                gunSpecs.elapsedSeconds = 0;
                int emptyShells = gunSpecs.magSize - gunSpecs.remainingBulletsInMag;
                gunSpecs.remainingBulletsInMag = Mathf.Min(emptyShells, gunSpecs.remainingBulletsInPocket);
                gunSpecs.remainingBulletsInPocket -= emptyShells;
                gunSpecs.reloading = false;
                gunStatusUpdatedEvent.Invoke(gunSpecs);
            }
        }
        else
        {
            gunSpecs.reloading = false;
            gunStatusUpdatedEvent.Invoke(gunSpecs);
        }
        return;



    }

    protected virtual void HandleCoolDown()
    {
        if (!gunSpecs.cooledDown)
        {
            gunSpecs.elapsedSeconds += Time.fixedDeltaTime;

            if (gunSpecs.elapsedSeconds >= gunSpecs.coolDownSeconds)
            {
                gunSpecs.elapsedSeconds = 0;
                gunSpecs.cooledDown = true;
            }
        }
    }


    protected void Move()
    {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        rb2d.MovePosition((Vector2)player.transform.position + move.normalized * Time.fixedDeltaTime * walkSpeed);
    }

    #endregion

    #endregion

}

#endregion



#region Silenced State

public class Silenced : PlayerState
{


    #region Constructor

    public Silenced(GameObject _player, Animator _anim, float _walkSpeed,
        GameObject _bullet, string animTrigger, Transform _bulletPosition,
         Rigidbody2D _rb2d, GunSpecs _gunSpecs)
        : base(_player, _anim, _walkSpeed, _bullet, animTrigger, _bulletPosition, _rb2d, _gunSpecs)
    {
        gunSpecs = _gunSpecs;
        name = WEAPONSTATE.SILENCED;
    }

    #endregion


    #region ProcessMethods

    protected override void Enter()
    {
        base.Enter();
    }

    protected override void Update()
    {

        base.Update();
        Move();
        if (Input.GetButtonDown("Fire1"))
            Fire();
        GetMouseWheel();
    }

    #endregion
}

#endregion

#region MachineGun State

public class MachineGun : PlayerState
{
    #region Fields

    string reloadAnimTrigger;


    #endregion


    #region Constructor

    public MachineGun(GameObject _player, Animator _anim, float _walkSpeed,
        GameObject _bullet, string animTrigger, string _reloadAnimTrigger,
        Transform bulletPosition, Rigidbody2D _rb2d, GunSpecs _gunSpecs)
        : base(_player, _anim, _walkSpeed, _bullet, animTrigger, bulletPosition, _rb2d, _gunSpecs)

    {
        name = WEAPONSTATE.MACHINEGUN;
        reloadAnimTrigger = _reloadAnimTrigger;
    }

    #endregion


    #region ProcessMethods

    protected override void Update()
    {

        base.Update();
        GetMouseWheel();
        Move();
        if (Input.GetAxis("Fire1") != 0)
            Fire();
    }


    #endregion


    #region Owned Methods




    #endregion

    #region OverRidden Methods


    #endregion
}

#endregion

