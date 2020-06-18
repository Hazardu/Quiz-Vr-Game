using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    public bool outer;
    public float outerRange, innerRange;
    public UnityEvent onOuterEnter, onOuterExit, onInnerEnter, onInnerExit;
    private enum State { outside, inOuter, inInner, }
    [SerializeField]private State stateLeft, stateRight;
    public bool calledInner, calledOuter;

    // Start is called before the first frame update
    void Start()
    {
        //square those two values
        outerRange *= outerRange;
        innerRange *= innerRange;
    }

    // Update is called once per frame
    void Update()
    {
        stateLeft = TestForHand(PlayerHands.left);
        stateRight = TestForHand(PlayerHands.right);

        var l = (int)TestForHand(PlayerHands.left);
        var r = (int)TestForHand(PlayerHands.right);

        if (r > 0 || l> 0)
        {
            if(r>1|| l > 1)
            {
                //in inner
                if(!calledInner)
                {
                    calledInner = true;
                    onInnerEnter?.Invoke();
                }
            }
            else if (calledInner)
            {
                calledInner = false;
                onInnerExit?.Invoke();
            }

            if (outer &&!calledOuter)
            {
                calledOuter = true;
                onOuterEnter?.Invoke();
            }

        }
        else
        {
            if (calledInner)
            {
                calledInner = false;
                onInnerExit?.Invoke();
            }
            if (outer&&calledOuter)
            {
                calledOuter = false;
                onOuterExit?.Invoke();
            }
        }

    }

    State TestForHand(Transform hand)
    {
        float mag = (transform.position - hand.position).sqrMagnitude;
        if (mag <= innerRange)
        {
            return State.inInner;
        }
        if (mag > outerRange && outer)
        {
            return State.outside;
        }
        return State.inOuter;
    }


}
