using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public enum State
    {
        Idle = 0,
        Moving = 1,
        Falling = 2, // cant move
        Casting = 3
    }

    public State state;

    private void Start()
    {
        state = State.Idle;
    }

    public void Set( State state )
    {
        this.state = state;
    }
    public void Set(int stateIndex)
    {
        Set((State) stateIndex);
    }

    public bool CanMove()
    {
        return state == State.Idle || state == State.Moving;
    }

    public IEnumerator SwitchStateForDuration(State stateA, State stateB, float duration)
    {
        Set(stateA);
        yield return new WaitForSeconds(duration);
        Set(stateB);   
    }

}
