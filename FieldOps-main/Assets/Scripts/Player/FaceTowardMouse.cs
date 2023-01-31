using UnityEngine;

public class FaceTowardMouse : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 ml = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (Vector2)rb2d.transform.position - ml;
        float angle = Mathf.Atan2(dir.y, dir.x);
        if (angle - 90 < -2 || angle + 90 > 2)
            rb2d.MoveRotation(angle * Mathf.Rad2Deg + 90f);
    }

}
