
using UnityEngine;
using Pathfinding;




#region Enemy Base State Class

public class EnemyStates : StatesBase
{

    #region Events



    #region Fields

    #region Animator Fields

    protected Animator anim;
    protected string NormalAnimTrigger = "Normal";
    protected string ShootAnimTrigger = "Shoot";

    #endregion

    #region TimerFields


    protected float elapsedSeconds;
    protected float totalSeconds;

    #endregion

    #region Components Refrences

    protected AudioSource audioSource;
    protected Transform bulletPosition;
    protected GameObject bullet;

    protected FOW fow;

    #endregion

    #region Navigation And Walking Fields

    protected Transform[] wayPoints;

    protected AIPath agent;
    protected AIDestinationSetter destinationSetter;

    #endregion

    #region Detection Fields

    protected Transform extraTransform;
    protected Transform player;
    protected Vector2 playerDir;

    #endregion

    #region State Tracking Fields

    protected EnemyStates nextState;

    #endregion

    #endregion

    #region Constructor

    public EnemyStates(Animator _anim,
        AudioSource _audioSource, AIDestinationSetter _aIDestinationSetter, AIPath _agent,
        Transform _bulletPosition, GameObject _bullet, Transform _extraTransform, Transform[] _wayPoints, FOW _fow)
    {

        anim = _anim;
        audioSource = _audioSource;
        destinationSetter = _aIDestinationSetter;
        agent = _agent;
        bulletPosition = _bulletPosition;
        bullet = _bullet;
        extraTransform = _extraTransform;
        wayPoints = _wayPoints;
        fow = _fow;
        stage = EVENT.ENTER;
    }

    #endregion

    #region Overridden Functions

    protected override void Enter()
    {
        fow.PlayerDetectedEvent.AddListener(PlayerDetectedEventHandler);
        fow.SuspectDetectedEvent.AddListener(SuspectDetectedEventHandler);
        fow.PlayerInRangeEvent.AddListener(PlayerInRangeEventHandler);
        EventManager.AddListener(FLOATVECTOR3EVENTS.NOISEPRODUCEDEVENT, NoiseProducedEventHandler);
        EventManager.AddListener(FLOATVECTOR3EVENTS.NOISEPRODUCEDEVENT, NoiseProducedEventHandler); ;
        base.Enter();
    }
    protected override void Exit()
    {
        EventManager.RemoveListener(FLOATVECTOR3EVENTS.NOISEPRODUCEDEVENT, NoiseProducedEventHandler);
        fow.PlayerDetectedEvent.RemoveListener(PlayerDetectedEventHandler);
        fow.SuspectDetectedEvent.RemoveListener(SuspectDetectedEventHandler);
        EventManager.RemoveListener(FLOATVECTOR3EVENTS.NOISEPRODUCEDEVENT, NoiseProducedEventHandler);
        base.Exit();
    }

    #endregion

    #region Public Functions

    public EnemyStates Process()
    {
        if (stage == EVENT.ENTER) Enter();
        else if (stage == EVENT.UPDATE) Update();
        else if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }


    #endregion

    #region Stuff Doing

    #region SeePlayer

    protected virtual void PlayerInRangeEventHandler(Transform player)
    {
        nextState = new Shoot(anim, player, audioSource, destinationSetter, agent,
        bulletPosition, bullet, extraTransform, wayPoints, fow);
        stage = EVENT.EXIT;
    }


    protected virtual void PlayerDetectedEventHandler(Transform player)
    {
        stage = EVENT.EXIT;
        nextState = new Chase(anim, player, audioSource, destinationSetter,
            agent, bulletPosition, bullet, extraTransform, wayPoints, fow);
    }

    protected virtual void SuspectDetectedEventHandler(Vector2 suspect)
    {
        stage = EVENT.EXIT;
        nextState = new Wander(anim, suspect, audioSource, destinationSetter, agent,
         bulletPosition, bullet, extraTransform, wayPoints, fow);
    }

    #endregion

    #region Wander

    protected float wanderRadius = 1f;

    protected virtual void WanderAround(Vector2 wanderTarget)
    {
        wanderRadius = Random.Range(1, 3);
        Vector2 wanderCircle = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        wanderCircle.Normalize();
        wanderCircle *= wanderRadius;
        wanderTarget += wanderCircle;
        extraTransform.position = wanderTarget;
        destinationSetter.target = extraTransform;
    }

    #endregion

    protected virtual void NoiseProducedEventHandler(float intensity, Vector3 origin)
    {
        if (Vector2.Distance(agent.transform.position, origin) <= intensity)
        {
            nextState = new Wander(anim, (Vector2)origin, audioSource, destinationSetter, agent,
                bulletPosition, bullet, extraTransform, wayPoints, fow);
            stage = EVENT.EXIT;
        }
    }

    #endregion

}

#endregion

#region Idle State

public class Idle : EnemyStates
{

    #region Constructor

    public Idle(Animator _anim, AudioSource _audioSource,
        AIDestinationSetter _aIDestinationSetter, AIPath _agent, Transform _bulletPosition,
        GameObject _bullet, Transform _extraTransform, Transform[] _wayPoints, FOW _fow)
        : base(_anim, _audioSource, _aIDestinationSetter, _agent,
            _bulletPosition, _bullet, _extraTransform, _wayPoints, _fow)
    {
        name = ENEMYSTATES.IDLE;
    }

    #endregion

    #region OverRidden Functions

    protected override void Enter()
    {
        anim.SetTrigger(NormalAnimTrigger);
        destinationSetter.target = null;
        base.Enter();
        destinationSetter.target = wayPoints[1];
    }

    protected override void Update()
    {
        if (Random.Range(0, 101f) < 90f)
        {
            nextState = new Patrol(anim, audioSource,
                destinationSetter, agent, bulletPosition, bullet, extraTransform, wayPoints, fow);
            stage = EVENT.EXIT;
        }

    }

    protected override void Exit()
    {
        anim.ResetTrigger(NormalAnimTrigger);
        destinationSetter.target = null;
        base.Exit();
    }

    #endregion

}

#endregion

#region Patrol State

public class Patrol : EnemyStates
{
    #region Fields

    protected int currentWPIndex;

    #endregion

    #region Constructor

    public Patrol(Animator _anim, AudioSource _audioSource,
        AIDestinationSetter _aIDestinationSetter, AIPath _agent, Transform _bulletPosition,
        GameObject _bullet, Transform _extraTransform, Transform[] _wayPoints, FOW _fow)
        : base(_anim, _audioSource, _aIDestinationSetter,
            _agent, _bulletPosition, _bullet, _extraTransform, _wayPoints, _fow)
    {
        name = ENEMYSTATES.PATROL;
        currentWPIndex = 0;
    }

    #endregion

    #region Overridden Functions

    protected override void Enter()
    {
        anim.SetTrigger(NormalAnimTrigger);
        destinationSetter.target = wayPoints[currentWPIndex].transform;
        base.Enter();
    }

    protected override void Update()
    {
        Patroll();

    }

    protected override void Exit()
    {
        anim.ResetTrigger(NormalAnimTrigger);
        destinationSetter.target = null;
        base.Exit();
    }

    #endregion

    #region Private Functions

    protected void Patroll()
    {
        if (currentWPIndex >= wayPoints.Length)
            currentWPIndex = 0;
        if (agent.reachedDestination)
        {
            destinationSetter.target = wayPoints[currentWPIndex].transform;
            currentWPIndex++;
        }
    }

    #endregion

}

#endregion

#region Chase State

public class Chase : EnemyStates
{
    #region Fields

    readonly float secondsToChaseBlindly = 3f;

    new Transform player;
    Vector2 playerDirection;

    #endregion

    #region Constructor

    public Chase(Animator _anim, Transform _player, AudioSource _audioSource,
        AIDestinationSetter _aIDestinationSetter, AIPath _agent, Transform _bulletPosition,
        GameObject _bullet, Transform _extraTransform, Transform[] _wayPoints, FOW _fow)
        : base(_anim, _audioSource, _aIDestinationSetter, _agent,
            _bulletPosition, _bullet, _extraTransform, _wayPoints, _fow)
    {
        name = ENEMYSTATES.CHASE;
        player = _player;

    }

    #endregion

    #region OverRidden Functions

    protected override void Enter()
    {
        fow.StateIconChangedEvent.Invoke(name);
        anim.SetTrigger(ShootAnimTrigger);
        destinationSetter.target = player.transform;

        base.Enter();
    }

    protected override void Update()
    {
        elapsedSeconds += Time.deltaTime;
        if (elapsedSeconds >= secondsToChaseBlindly)
        {
            stage = EVENT.EXIT;
            extraTransform.position = agent.transform.position;
            nextState = new Wander(anim, extraTransform.position, audioSource, destinationSetter, agent,
                bulletPosition, bullet, extraTransform, wayPoints, fow);
        }


    }

    protected override void Exit()
    {
        fow.StateIconChangedEvent.Invoke(nextState.name);
        anim.SetTrigger(ShootAnimTrigger);
        destinationSetter.target = null;
        base.Exit();
    }

    protected override void PlayerDetectedEventHandler(Transform player)
    {
        if (elapsedSeconds > 1)
            elapsedSeconds = 0;
        return;
    }

    protected override void SuspectDetectedEventHandler(Vector2 suspect)
    {
        return;
    }

    protected override void NoiseProducedEventHandler(float intensity, Vector3 origin)
    {
        return;
    }

    #endregion

}

#endregion


#endregion

#region Wander State

public class Wander : EnemyStates
{

    #region Fields

    Vector2 wanderAroundTarget;
    readonly float nervousSeconds = 30f;
    float inNervousStateTime;

    #endregion

    #region Constructor

    public Wander(Animator _anim, Vector2 _wanderAroundTarget, AudioSource _audioSource,
        AIDestinationSetter _aIDestinationSetter, AIPath _agent, Transform _bulletPosition,
        GameObject _bullet, Transform _extraTransform, Transform[] _wayPoints, FOW _fow)
        : base(_anim, _audioSource, _aIDestinationSetter, _agent,
            _bulletPosition, _bullet, _extraTransform, _wayPoints, _fow)
    {
        name = ENEMYSTATES.WANDER;
        wanderAroundTarget = _wanderAroundTarget;
    }

    #endregion

    #region Overridden Functions

    protected override void Enter()
    {
        fow.StateIconChangedEvent.Invoke(name);
        anim.SetTrigger(ShootAnimTrigger);
        base.Enter();
        WanderAround(wanderAroundTarget);
    }

    protected override void Update()
    {
        if (agent.reachedEndOfPath || !agent.hasPath)
        {
            WanderAround(wanderAroundTarget);
        }
        inNervousStateTime += Time.deltaTime;

        if (inNervousStateTime >= nervousSeconds)
        {
            stage = EVENT.EXIT;
            nextState = new Idle(anim, audioSource, destinationSetter, agent,
             bulletPosition, bullet, extraTransform, wayPoints, fow);
        }


    }



    protected override void Exit()
    {
        fow.StateIconChangedEvent.Invoke(nextState.name);
        anim.ResetTrigger(ShootAnimTrigger);
        destinationSetter.target = null;
        base.Exit();
    }

    #endregion

}

#endregion

#region Attack State

public class Shoot : EnemyStates
{
    #region Fields

    int clockWise = 1;
    bool cooledDown = true;
    protected float coolDownSeconds = .2f;

    float gunAngle = 1f;

    float timeToRotate = .5f;

    #endregion

    #region Constructor

    public Shoot(Animator _anim, Transform _player, AudioSource _audioSource,
        AIDestinationSetter _aIDestinationSetter, AIPath _agent, Transform _bulletPosition,
        GameObject _bullet, Transform _extraTransform, Transform[] _wayPoints, FOW _fow)
        : base(_anim, _audioSource, _aIDestinationSetter, _agent,
            _bulletPosition, _bullet, _extraTransform, _wayPoints, _fow)
    {

        name = ENEMYSTATES.SHOOT;
        player = _player;
    }

    #endregion

    #region Overridden Functions

    protected override void Enter()
    {
        destinationSetter.target = null;
        agent.isStopped = true;
        anim.SetTrigger(ShootAnimTrigger);
        base.Enter();
        EventManager.AddListener(INTEVENTS.GAMEOVEREVENT, GameOverEventHandler);
    }

    protected override void Update()
    {
        if (player != null)
        {
            RotateToPlayer();
            base.Update();

            if (!cooledDown)
                ManageCoolDown();

            if (!CanSee(player))
            {
                nextState = new Chase(anim, player, audioSource, destinationSetter, agent
                , bulletPosition, bullet, extraTransform, wayPoints, fow);
                stage = EVENT.EXIT;
            }
        }



    }

    protected override void Exit()
    {
        EventManager.RemoveListener(INTEVENTS.GAMEOVEREVENT, GameOverEventHandler);
        agent.isStopped = false;
        anim.ResetTrigger(ShootAnimTrigger);
        base.Exit();
    }


    void GameOverEventHandler(int unused)
    {
        nextState = new Patrol(anim, audioSource, destinationSetter, agent,
        bulletPosition, bullet, extraTransform, wayPoints, fow);
        stage = EVENT.EXIT;
    }


    protected override void PlayerInRangeEventHandler(Transform player)
    {
        return;
    }

    protected override void SuspectDetectedEventHandler(Vector2 suspect)
    {
        return;
    }

    protected override void NoiseProducedEventHandler(float intensity, Vector3 origin)
    {
        return;
    }

    #endregion

    #region Private Funcrions

    void RotateToPlayer()
    {
        playerDir = player.position - agent.position;
        playerDir.Normalize();
        float angle = Vector3.Angle(agent.transform.up, playerDir);
        if (MyMath.CrossProduct(agent.transform.up, playerDir).z < 0)
            clockWise = -1;
        else
            clockWise = 1;
        if (angle > gunAngle)
            agent.transform.Rotate(new Vector3(0, 0, clockWise * angle * Time.deltaTime * 5));
        if (angle < gunAngle)
        {
            Fire();
        }
        else if (angle < gunAngle * 8)
        {
            Fire();
        }

    }



    void Fire()
    {
        if (cooledDown)
        {
            GameObject.Instantiate(bullet, bulletPosition.transform.position, bulletPosition.transform.rotation);
            cooledDown = false;
        }
    }

    bool CanSee(Transform gameObject)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(agent.position, playerDir, fow.viewRadius);
        if (hit2D.transform == gameObject.transform)
            return true;
        else
            return false;
    }


    void ManageCoolDown()
    {
        elapsedSeconds += Time.deltaTime;
        if (elapsedSeconds > coolDownSeconds)
        {
            elapsedSeconds = 0f;
            cooledDown = true;
        }
    }


}

#endregion

#endregion

