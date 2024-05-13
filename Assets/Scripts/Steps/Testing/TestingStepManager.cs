using System.Collections.Generic;
using UnityEngine;
public class TestingStepManager : StepManager
{
    private List<TestManager> testManagers = new List<TestManager>();
    private TestManager currenttm;
    private int tmIndex = 0;
    public void AddTestManager(TestManager testManager)
    {
        this.testManagers.Add(testManager);
    }
    private void SortLists()
    {
        testManagers.Sort((left, right) => left.name.CompareTo(right.name));
    }
    public override void StartStep()
    {
        base.StartStep();
        SortLists();
        ActivateStep();
        progressBarManager.NextSlider();
        int value = testManagers.Count;
        progressBarManager.Init(ID, value);
    }
    public void ActivateStep()
    {
        currenttm = testManagers[tmIndex];
        currenttm.InitiateInteraction();
    }
    public void NextTestPiece()
    {
        tmIndex++;
        OnDevelopmentStep?.Invoke();
        if (tmIndex > testManagers.Count - 1)
        {
            FinishedStep();
        }
        else
        {
            currenttm = testManagers[tmIndex];
            currenttm.InitiateInteraction();
        }

    }
    public override void DeleteData()
    {
        foreach (TestManager mm in testManagers) Destroy(mm);
        testManagers = null;
        currenttm = null;
    }
}
