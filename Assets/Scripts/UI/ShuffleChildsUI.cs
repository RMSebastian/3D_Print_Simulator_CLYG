using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShuffleChildsUI : MonoBehaviour
{
    public WaitableBase goodAnimation;
    public WaitableBase badAnimation;
    public UnityEvent events;
    public void ShuffleChildren()
    {
        int childCount = transform.childCount;

        Transform[] children = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
        for (int i = 0; i < childCount; i++)
        {
            int randomIndex = Random.Range(i, childCount);
            Transform temp = children[randomIndex];
            children[randomIndex] = children[i];
            children[i] = temp;
        }

        for (int i = 0; i < childCount; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }
    public void PassTest(bool Pass)
    {
        if(Pass)
        {
            goodAnimation.HandleRequest(events.Invoke);
        }
        else
        {
            badAnimation.HandleRequest(ShuffleChildren);
        }
    }
}
