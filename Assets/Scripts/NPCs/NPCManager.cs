using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public class State
{
    public string name;
    public UnityEvent onEnter;
    public UnityEvent onExit;
}

public class NPCManager : MonoBehaviour
{
    [Header("State Management")]
    [SerializeField] private List<State> states = new List<State>();
    [SerializeField] private string currentStateName;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    
    private State currentState;
    
    void Start()
    {
        // Set initial state if states exist
        if (states.Count > 0 && string.IsNullOrEmpty(currentStateName))
        {
            GoToState(states[0].name);
        }
    }
    
    public void GoToState(string stateName)
    {
        // Find the target state
        State targetState = states.Find(s => s.name == stateName);
        
        if (targetState == null)
        {
            Debug.LogWarning($"State '{stateName}' not found in NPCManager on {gameObject.name}");
            return;
        }
        
        // Exit current state
        if (currentState != null && currentState.onExit != null)
        {
            currentState.onExit.Invoke();
        }
        
        // Update current state
        currentState = targetState;
        currentStateName = stateName;
        
        // Enter new state
        if (currentState.onEnter != null)
        {
            currentState.onEnter.Invoke();
        }

        // Trigger animation
        if (animator != null)
        {
            animator.SetTrigger(stateName);
            // or animator.SetBool(stateName, true);
        }
        
        Debug.Log($"NPC {gameObject.name} transitioned to state: {stateName}");
    }
    
    public void AddState(string stateName)
    {
        if (states.Exists(s => s.name == stateName))
        {
            Debug.LogWarning($"State '{stateName}' already exists in NPCManager on {gameObject.name}");
            return;
        }
        
        State newState = new State
        {
            name = stateName,
            onEnter = new UnityEvent(),
            onExit = new UnityEvent()
        };
        
        states.Add(newState);
    }
    
    public void RemoveState(string stateName)
    {
        State stateToRemove = states.Find(s => s.name == stateName);
        if (stateToRemove != null)
        {
            states.Remove(stateToRemove);
            
            // If we're removing the current state, transition to first available state
            if (currentState == stateToRemove)
            {
                if (states.Count > 0)
                {
                    GoToState(states[0].name);
                }
                else
                {
                    currentState = null;
                    currentStateName = "";
                }
            }
        }
    }
    
    public string GetCurrentStateName()
    {
        return currentStateName;
    }
    
    public State GetCurrentState()
    {
        return currentState;
    }
    
    public List<State> GetAllStates()
    {
        return new List<State>(states);
    }
}
