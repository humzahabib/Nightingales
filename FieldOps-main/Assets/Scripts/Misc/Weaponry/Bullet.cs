using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    int DamagePoints = 5;
    Vector2 mL;
    public int speed;

    EmptyEvent BulletNotHitEvent = new EmptyEvent();

    Rigidbody2D rb;
    GameObjectIntEvent EnemyHitEvent = new GameObjectIntEvent();
    IntEvent PlayerHitEvent = new IntEvent();

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddInvoker(GAMEOBJECTINTEVENTS.ENEMYHITEVENT, EnemyHitEvent);
        EventManager.AddInvoker(INTEVENTS.PLAYERHITEVENT, PlayerHitEvent);
        EventManager.AddInvoker(EMPTYEVENTS.BULLETNOTHITEVENT, BulletNotHitEvent);

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed * Time.deltaTime;
        Invoke("DestroyBullet", 10);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void DestroyBullet()
    {
        BulletNotHitEvent.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyHitEvent.Invoke(collision.gameObject, DamagePoints);
        }
        else if (collision.gameObject.tag == "Player")
            PlayerHitEvent.Invoke(DamagePoints);

        if (!collision.gameObject.tag.Equals(transform.tag))
            DestroyBullet();
    }
}
