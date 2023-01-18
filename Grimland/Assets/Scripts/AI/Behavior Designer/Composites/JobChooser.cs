using System;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class JobChooser : Composite
{
    // The index of the child that is currently running or is about to run.
    private int currentJobIndex;

    public Creature Creature { get; private set; }
    public Job CurrentJob { get; private set; }


    public override void OnStart()
    {
        Creature = (Creature) IdManager.GetThingById(Owner.GetVariable("creatureId").GetValue().ToString());
    }

    public override int CurrentChildIndex()
    {
        Job job = JobManager.RequestJob(Creature);
        CurrentJob = job ?? new WanderJob(Creature);

        if (CurrentJob.GetType() != typeof(WanderJob))
        {
            Debug.Log($"==== BEGINNING JOB: {CurrentJob}");
        }

        currentJobIndex = GetJobIndexByName(CurrentJob);
        return currentJobIndex;
    }

    private int GetJobIndexByName(Job job)
    {
        string jobName = job.GetType().Name;

        for (int i = 0; i < Children.Count; i++)
        {
            if (Children[i].FriendlyName == jobName)
            {
                return i;
            }
        }

        Debug.LogError("Couldn't find job index with requested name!");
        return 0;
    }

    public override bool CanExecute()
    {
        //TODO: make use of this, maybe pawn condition checking goes in here? When sleeping, this should return false?
        return true;
    }

    public override void OnConditionalAbort(int childIndex)
    {
        // The JobChooser was interrupted.
        // currentJobIndex = 0;
        // Debug.Log("JOB CHOOSER WAS ABORTED CONDITIONALLY()");
    }

    public override void OnChildExecuted(TaskStatus childStatus)
    {
        // Whenever a job (it's root task) is finished, this is called
        // Based on outcome, we should perform cleanup and optional other actions
        switch (childStatus)
        {
            case TaskStatus.Failure:
                CurrentJob.OnJobFailed(Creature);
                break;
            case TaskStatus.Success:
                CurrentJob.OnJobCompleted(Creature);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(childStatus), childStatus, null);
        }
    }
}