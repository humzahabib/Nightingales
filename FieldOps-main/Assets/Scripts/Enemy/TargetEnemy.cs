using UnityEngine;

public class TargetEnemy : Enemy
{
    ScriptableObjectEvent ObjectiveCompletedEvent = new ScriptableObjectEvent();

    [SerializeField]
    Objective objective;


    protected override void Start()
    {
        EventManager.AddInvoker(SCRIPTABLEOBJECTSEVENTS.OBJECTIVECOMPLETEDEVENT, ObjectiveCompletedEvent);
        base.Start();
    }



    protected override void Die()
    {
        ObjectiveCompletedEvent.Invoke(objective);
        base.Die();
    }


}
