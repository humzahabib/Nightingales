using UnityEngine;

public class PlayerPossession : MonoBehaviour
{
    int health = 100;
    IntEvent PlayerHealthChangedEvent = new IntEvent();
    IntEvent GameOverEvent = new IntEvent();

    private void Start()
    {
        EventManager.AddListener(INTEVENTS.PLAYERHITEVENT, PlayerHitEventHandler);
        EventManager.AddInvoker(INTEVENTS.PlayerHealthChangedEvent, PlayerHealthChangedEvent);
        EventManager.AddInvoker(INTEVENTS.GAMEOVEREVENT, GameOverEvent);
    }

    private void Update()
    {
    }

    void OnDisable()
    {
        EventManager.RemoveListener(INTEVENTS.PLAYERHITEVENT, PlayerHitEventHandler);
        EventManager.RemoveInvoker(INTEVENTS.PlayerHealthChangedEvent, PlayerHealthChangedEvent);
        EventManager.RemoveInvoker(INTEVENTS.GAMEOVEREVENT, GameOverEvent);

    }

    private void PlayerHitEventHandler(int damagePoints)
    {
        health = Mathf.Max(0, health -= damagePoints);
        PlayerHealthChangedEvent.Invoke(health);
        if (health <= 0)
        {
            GameOverEvent.Invoke((int)FAILURETYPES.BYPLAYERDEATH);
            Destroy(gameObject);
        }
    }

}

