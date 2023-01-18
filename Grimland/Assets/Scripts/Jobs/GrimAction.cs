using System;
using System.Collections.Generic;
using System.Linq;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

public abstract class GrimAction : Action, IHasFailCondition
{
    public Job CurrentJob { get; set; }
    public Creature Creature { get; set; }
    public List<Func<bool>> FailConditions { get; set; }
    protected bool LocalFailConditionsSatisfied => FailConditions.All(failCondition => !failCondition());
    protected bool GlobalFailAndEndConditionsSatisfied => CurrentJob.GlobalEndConditionsSatisfied && CurrentJob.GlobalFailConditionsSatisfied;
    protected bool ReservationsSuccessful { get; private set; }

    public override void OnStart()
    {
        CurrentJob = Owner.FindTask<JobChooser>().CurrentJob;
        Creature = Owner.FindTask<JobChooser>().Creature;

        FailConditions = new List<Func<bool>>();

        ReservationsSuccessful = TryMakeLocalReservations();
    }

    public void AddFailCondition(Func<bool> failCondition)
    {
        FailConditions.Add(() => !failCondition());
    }

    protected virtual bool TryMakeLocalReservations()
    {
        return true;
    }

    public virtual void ReleaseLocalReservations() { }
}