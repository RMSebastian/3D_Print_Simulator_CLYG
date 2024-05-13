using UnityEngine;
using UnityEngine.Events;

public abstract class StepManager : MonoBehaviour
{
    public int ID = 0;
    protected GameManager gameManager;
    [SerializeField] protected ProgressBarManager progressBarManager;
    [SerializeField] protected UnityEvent OnStartStep;
    [SerializeField] protected UnityEvent OnDevelopmentStep;
    [SerializeField] protected UnityEvent OnEndStep;
    protected virtual void Awake()
    {
        gameManager = GameManager.Instance;
        gameManager.AddStepManagerToList(this);
    }
    public virtual void StartStep()
    {
        OnStartStep?.Invoke();
    }
    public virtual void FinishedStep()
    {
        OnEndStep?.Invoke();
        gameManager?.FinishedAStep();
    }
    public virtual void DeleteData() { }
}
