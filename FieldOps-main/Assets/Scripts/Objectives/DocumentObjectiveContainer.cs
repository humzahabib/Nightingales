using UnityEngine;

public class DocumentObjectiveContainer : MonoBehaviour
{
    ScriptableObjectEvent ObjectiveCompletedEvent = new ScriptableObjectEvent();




    [SerializeField]
    Objective objective;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddInvoker(SCRIPTABLEOBJECTSEVENTS.OBJECTIVECOMPLETEDEVENT, ObjectiveCompletedEvent);
    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            ObjectiveCompletedEvent.Invoke(objective);
        }
    }
}
