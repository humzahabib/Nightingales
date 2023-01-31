using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Asset", menuName = "Level/Level")]
public class Level : ScriptableObject
{

    ScriptableObjectEvent ObjectiveAddedEvent = new ScriptableObjectEvent();

    public new LEVELNAMES name;

    [SerializeField, TextArea]
    public string discription;


    [SerializeField]
    public List<Objective> objectives = new List<Objective>();

    bool allObjectivesFinished;



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
    }


    public void Initialize()
    {
        if (objectives.Count <= 0)
            objectives.AddRange(Resources.LoadAll<Objective>(@"Objectives/" + name.ToString()));

        EventManager.AddInvoker(SCRIPTABLEOBJECTSEVENTS.OBJECTIVEADDEDEVENT, ObjectiveAddedEvent);
        EventManager.AddListener(SCRIPTABLEOBJECTSEVENTS.OBJECTIVECOMPLETEDEVENT, ObjectiveCompletedEventHandler);
        for (int i = objectives.Count - 1; i >= 0; i--)
        {
            ObjectiveAddedEvent.Invoke(objectives[i]);
        }
    }

    public bool CheckObjectivesCompleted()
    {
        foreach (Objective objective in objectives)
        {
            if (!objective.finished)
            {
                return false;
            }
        }
        
        return true;
    }




    void ObjectiveCompletedEventHandler(ScriptableObject objective)
    {
        SetObjectiveFinished((Objective)objective);
    }


    public virtual void SetObjectiveFinished(Objective _objective)
    {
        Objective objective = _objective;
        objective.finished = true;
        if (objective.daughterObjective != null)
        {
            objectives.Add(objective);
            ObjectiveAddedEvent.Invoke(objective.daughterObjective);
        }
    }

}



public enum LEVELNAMES
{
    SomeOneSmarter,
}
