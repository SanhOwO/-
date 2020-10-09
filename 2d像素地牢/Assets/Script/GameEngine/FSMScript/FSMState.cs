using UnityEngine;
using System.Collections.Generic;
using GameEngine.Instance;

/// <summary>
/// Place the labels for the Transitions in this enum. —— 在此枚举中放置转换的标签。
/// Don't change the first label, NullTransition as FSMSystem class uses it. —— 不要改变第一个标签：NullTransition，因为FSMSystem类使用它。
/// </summary>
public enum Transition  //转换条件
{
    NullTransition = 0, // Use this transition to represent a non-existing transition in your system —— 使用此转换表示系统中不存在的转换
    Z_StanBy,
    Z_LetsMove,
    Z_GotTarget,
    Z_HpZero,
    P_ClickReal,
    P_ClickFake,
    P_BeingPlanted,
    P_NotCounterAndNotAttack, //既没在攻击,也没被攻击
    P_IsAttacking,  //正在攻击,没被攻击
    P_GetAttacked,  //没在攻击,被攻击了
    P_CounterAndAttack, //即在攻击,也在被攻击
    P_Disappear,
    S_JumpOut,
    S_DropDown,
    S_FlyToUI,
    S_Disappear,
    C_Planted,
    C_CDDone,   //CD好了,太阳不够
    C_SunDone,  //太阳钩蛾类,CD没好
    C_AllDown,
}

/// <summary>
/// Place the labels for the States in this enum. ——  在此枚举中放置状态的标签。
/// Don't change the first label, NullStateID as FSMSystem class uses it.不要改变第一个标签：NullStateID，因为FSMSystem类使用它。
/// </summary>
public enum StateID //状态
{
    NullStateID = 0, // Use this ID to represent a non-existing State in your system —— 使用此ID表示系统中不存在的状态
    Z_Idle,
    Z_Move,
    Z_Attack,
    Z_Dead,
    P_Invisible,
    P_Real,          //点击看片后的实物
    P_Fake,         //虚影植物
    P_Planted,      //种植下去的植物
    P_CANotAll,     //没攻击,没被攻击
    P_Attacking,    //攻击,没被攻击 
    P_Counter,          //没攻击,被攻击
    P_CAAll,        //攻击,同时被攻击
    S_FlowerJump,   
    S_Nature,
    S_Collect,
    S_Invisible,
    C_NoCDNoSun,
    C_HasCDNoSun,
    C_NoCdHasSun,
    C_HasAll,
}

/// <summary>
/// This class represents the States in the Finite State System.该类表示有限状态系统中的状态。
/// Each state has a Dictionary with pairs (transition-state) showing 每个状态都有一个对显示(转换状态)的字典
/// which state the FSM should be if a transition is fired while this state is the current state.如果在此状态为当前状态时触发转换，则FSM应处于那种状态。
/// Method Reason is used to determine which transition should be fired .方法原因用于确定应触发哪个转换。
/// Method Act has the code to perform the actions the NPC is supposed do if it's on this state.方法具有执行NPC动作的代码应该在这种状态下执行。
/// </summary>

public abstract class FSMState
{
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    protected StateID stateID;
    public StateID ID { get { return stateID; } }

    public void AddTransition(Transition trans, StateID id)
    {
        // Check if anyone of the args is invalid
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
            return;
        }

        // Since this is a Deterministic FSM,
        //   check if the current transition was already inside the map
        if (map.ContainsKey(trans))
        {
            Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() +
                           "Impossible to assign to another state");
            return;
        }
        map.Add(trans, id);
    }

    /// <summary>
    /// This method deletes a pair transition-state from this state's map.
    /// If the transition was not inside the state's map, an ERROR message is printed.
    /// </summary>
    public void DeleteTransition(Transition trans)
    {
        // Check for NullTransition
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }

        // Check if the pair is inside the map before deleting
        if (map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() +
                       " was not on the state's transition list");
    }

    /// <summary>
    /// This method returns the new state the FSM should be if
    ///    this state receives a transition and 
    /// </summary>
    public StateID GetOutputState(Transition trans)
    {
        // Check if the map has this transition
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return StateID.NullStateID;
    }

    /// <summary>
    /// This method is used to set up the State condition before entering it.
    /// It is called automatically by the FSMSystem class before assigning it
    /// to the current state.
    /// </summary>
    public virtual void DoBeforeEntering() { }

    /// <summary>
    /// This method is used to make anything necessary, as reseting variables
    /// before the FSMSystem changes to another one. It is called automatically
    /// by the FSMSystem before changing to a new state.
    /// </summary>
    public virtual void DoBeforeLeaving() { }

    /// <summary>
    /// This method decides if the state should transition to another on its list
    /// NPC is a reference to the object that is controlled by this class
    /// </summary>
    public abstract void Reason(GameObject npc);

    /// <summary>
    /// This method controls the behavior of the NPC in the game World.
    /// Every action, movement or communication the NPC does should be placed here
    /// NPC is a reference to the object that is controlled by this class
    /// </summary>
    public abstract void Act(GameObject npc);
}
