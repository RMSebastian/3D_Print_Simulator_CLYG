using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private List<StepManager> steps = new List<StepManager>();
    private FinalAnimationScript fas;
    private int index = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else DestroyImmediate(this);
    }
    public void FinishedAStep()
    {
        steps[index].DeleteData();

        index++;
        if(index >= steps.Count)
        {
            EndSimulator();
        }
        else
        {
            BeginAStep();
        }
        
    }

    [ContextMenu("Start")]
    public void StartSimulator()
    {
        SortLists(steps);
        BeginAStep();
    }
    public void CloseSimulator()
    {
        steps = new List<StepManager>();
        index = 0;
    }
    public void EndSimulator()
    {
        LevelLoader.Instance.Screen(()=>fas.PrepareMinigame());   
    }
    private void BeginAStep()=> steps[index].StartStep();
    public void SetFinalInteraction(FinalAnimationScript fas) => this.fas = fas;
    public StepManager GetManager<T>() where T : StepManager =>  steps.Find(element => element.GetType() == typeof(T)) as T;
    private void SortLists<T>(List<T> list) where T : StepManager => list.Sort((left, right) => left.ID.CompareTo(right.ID));
    public void AddStepManagerToList(StepManager stepManager)=> this.steps.Add(stepManager);
    public static GameManager Instance => instance;

}
