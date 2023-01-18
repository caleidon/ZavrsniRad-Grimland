using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Job : IHasFailCondition, IHasEndCondition
{
    public string Id { get; set; }
    public List<Func<bool>> EndConditions { get; set; } = new List<Func<bool>>();
    public List<Func<bool>> FailConditions { get; set; } = new List<Func<bool>>();
    public List<Vector3Int> KnownLocations { get; set; } = new List<Vector3Int>();
    public List<string> ChildJobPrerequisitesIds { get; set; } = new List<string>();
    public abstract bool Suspendable { get; } // A job is suspendable if you can leave it and progress will not reset
    public abstract bool AutoRestart { get; } // if autorestart is enabled, if failed in any way, job will TRY to re-add itself
    public abstract Creature.PawnMovementUrgency MovementUrgency { get; }

    public abstract bool CanBeDoneBy(Creature creature);

    public bool GlobalFailConditionsSatisfied => FailConditions.All(failCondition => !failCondition());
    public bool GlobalEndConditionsSatisfied => EndConditions.All(endCondition => !endCondition());

    // protected Job([CanBeNull] string id)
    // {
    //     
    // }

    public virtual void OnJobCompleted(Creature creature)
    {
        ReleaseReservations(creature);
    }

    public virtual void OnJobFailed(Creature creature)
    {
        ReleaseReservations(creature);
    }

    public virtual bool AreRequirementsFulfilled(Creature creature)
    {
        return true;
    }

    public bool CanMakeReservations(Creature creature)
    {
        bool reservationsPossible = MakeReservations(creature);

        ReleaseReservations(creature);

        return reservationsPossible;
    }

    public virtual bool MakeReservations(Creature creature)
    {
        return true;
    }

    private void ReleaseReservations(Creature creature)
    {
        ReservationManager.ReleaseAllByJob(creature, this);
        HaulReservationManager.ReleaseAllByJob(creature, this);
    }

    public void AddFailCondition(Func<bool> failCondition)
    {
        FailConditions.Add(() => !failCondition());
    }

    public void AddEndCondition(Func<bool> endCondition)
    {
        EndConditions.Add(() => !endCondition());
    }

    // public void GenerateJobId(string previousId)
    // {
    //     Id = IdManager.GenerateThingID(this, previousId);
    // }
}