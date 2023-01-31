using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    bool locked;
    bool playerLocked;
    [SerializeField]
    Lock bolt;

    IntEvent lockSendEvent = new IntEvent();

    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.LogWarning("Pressed E");
                lockSendEvent.Invoke(bolt.key);
            }
        }
    }


    public void Open()
    {
        anim.SetBool("isOpen", true);
    }

    public void Close()
    {
        anim.SetBool("isOpen", false);
    }

    public void KeySendEventHandler(int key)
    {
        if (key == bolt.key)
        {
            Open();
        }
    }
}
