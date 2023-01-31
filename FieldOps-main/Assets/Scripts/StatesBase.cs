using UnityEngine;

#region ENEMY STATES STATES

public enum ENEMYSTATES
{
    #region GENERAL STATES

    IDLE, PATROL,

    #endregion

    #region AGRESSIVE STATES
    SHOOT,

    #endregion

    #region PLAYER DISTRACTION STATES

    WANDER, CHASE,

    #endregion
}

#endregion

#region Finite State Machine Enum

public enum WEAPONSTATE { SILENCED, MACHINEGUN, DEAD, }

public enum EVENT { ENTER, UPDATE, EXIT }

#endregion

public class StatesBase
{

    #region Fields


    #region Refrences For Components

    AudioSource audioSource;

    #endregion

    protected float walkSpeed;

    #region State Specific Fields

    public ENEMYSTATES name;
    protected EVENT stage;

    #endregion

    #endregion

    #region State Processing Methods

    protected virtual void Enter() { stage = EVENT.UPDATE; }
    protected virtual void Update() { stage = EVENT.UPDATE; }
    protected virtual void Exit() { stage = EVENT.EXIT; }

    #endregion

}
