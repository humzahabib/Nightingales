using UnityEngine;
using UnityEngine.UI;

public class EnemyHUDManager : MonoBehaviour
{

    FOW fow;

    [SerializeField]
    Image botStateIcon;

    [SerializeField]
    Sprite chaseIcon;

    [SerializeField]
    Sprite wanderIcon;

    [SerializeField]
    Vector2 stateIconOffset;

    // Start is called before the first frame update
    void Start()
    {
        fow = GetComponent<FOW>();
        fow.StateIconChangedEvent += StateIconChangedEventHandler;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        botStateIcon.transform.position = Camera.main.WorldToScreenPoint((Vector2)transform.position + stateIconOffset);
    }

    void StateIconChangedEventHandler(ENEMYSTATES state)
    {
        if (state != ENEMYSTATES.CHASE && state != ENEMYSTATES.WANDER && state != ENEMYSTATES.SHOOT)
        {
            botStateIcon.gameObject.SetActive(false);
        }
        switch (state)
        {
            case ENEMYSTATES.CHASE:
                botStateIcon.gameObject.SetActive(true);
                botStateIcon.sprite = chaseIcon;
                break;
            case ENEMYSTATES.WANDER:
                botStateIcon.gameObject.SetActive(true);
                botStateIcon.sprite = wanderIcon;
                break;
            case ENEMYSTATES.SHOOT:
                botStateIcon.gameObject.SetActive(true);
                botStateIcon.sprite = chaseIcon;
                break;



        }
    }
}
