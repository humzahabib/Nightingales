using UnityEngine;
using System;
using Pathfinding;


public class Enemy : MonoBehaviour, IEndGameObserver
{

    GameObjectEvent WitnessRemovedEvent = new GameObjectEvent();


    #region Events

    FloatIntEvent enemyHitEvent;

    #endregion

    #region Fields 

    #region Health Fields

    [SerializeField]
    protected float maxHealth = 20f;


    protected float currentHealth;

    float alertHealth = 1;
    float unAlertHealth = 1;

    #endregion

    #endregion

    #region Components Refrenced

    [SerializeField]
    FOW fow;


    Animator anim;

    #endregion

    #region State Related Fields

    bool alert = false;

    [SerializeField]
    GameObject bullet;

    [SerializeField]
    Transform bulletPosition;


    [SerializeField]
    Transform player;

    EnemyStates currentState;

    [SerializeField]
    AIDestinationSetter destinationSetter;

    [SerializeField]
    AIPath agent;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    Transform[] wayPoints;

    [SerializeField]
    GameObject extraTransform;

    #endregion

    #region Patrol State Fields

    #endregion


    // Start is called before the first frame update
    protected virtual void Start()
    {

        #region ComponentsGetting

        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMover>().transform;
        destinationSetter = GetComponent<AIDestinationSetter>();
        agent = GetComponent<AIPath>();
        fow = GetComponent<FOW>();

        #endregion

        #region Event Registration

        EventManager.AddInvoker(GAMEOBJECTEVENTS.WITNESSREMOVEDEVENT, WitnessRemovedEvent);
        EventManager.AddListener(GAMEOBJECTINTEVENTS.ENEMYHITEVENT, EnemyHitEventHandler);

        #endregion

        currentHealth = maxHealth;

        #region Finite States Implementation

        currentState = new Idle(anim, audioSource,
            destinationSetter, agent, bulletPosition, bullet, extraTransform.transform, wayPoints, fow);

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
            currentState = currentState.Process();
    }


    protected virtual void EnemyHitEventHandler(GameObject enemy, int damagePoints)
    {
        if (enemy == this.gameObject)
        {
            currentHealth -= damagePoints;
            if (currentHealth <= 0f)
            {
                Die();
            }
        }
    }

    protected virtual void Die()
    {
        WitnessRemovedEvent.Invoke(this.gameObject);
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }

}
