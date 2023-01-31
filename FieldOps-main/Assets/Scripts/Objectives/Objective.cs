using UnityEngine;


[CreateAssetMenu(fileName = "NewObjective", menuName = "Level/Objective")]
public class Objective : ScriptableObject
{


    public new OBJECTIVENAME name;

    [TextArea]
    public string discription;

    public bool finished;

    [TextArea]
    public string congratulationMessage;

    public Objective daughterObjective;


}

