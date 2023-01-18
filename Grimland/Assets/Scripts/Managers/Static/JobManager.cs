using System.Collections.Generic;
using UnityEngine;

public static class JobManager
{
    public static HashSet<Job> inactiveJobs { get; } = new HashSet<Job>();
    public static HashSet<Job> availableJobs { get; } = new HashSet<Job>();

    public static Job RequestJob(Creature creature)
    {
        if (availableJobs.Count == 0)
        {
            return null;
        }

        Vector3Int creatureLocation = creature.GetNode();
        HashSet<Job> potentialJobs = FindPotentialJobs(creature);

        if (potentialJobs.Count > 0)
        {
            Job jobToDo = FindClosestJob(potentialJobs, creatureLocation);
            DequeueJob(jobToDo);
            return jobToDo;
        }

        return null;
    }

    private static HashSet<Job> FindPotentialJobs(Creature creature)
    {
        HashSet<Job> potentialJobs = new HashSet<Job>();

        foreach (var job in availableJobs)
        {
            if (job.CanBeDoneBy(creature) && job.AreRequirementsFulfilled(creature))
            {
                Debug.Log("==== TESTING CONDITIONS");
                if (job.CanMakeReservations(creature))
                {
                    potentialJobs.Add(job);
                }
            }
        }

        return potentialJobs;
    }

    private static Job FindClosestJob(HashSet<Job> potentialJobs, Vector3Int creatureLocation)
    {
        Job selectedJob = null;
        float smallestDistance = float.MaxValue;

        foreach (var potentialJob in potentialJobs)
        {
            float distance = Vector3Int.Distance(creatureLocation, potentialJob.KnownLocations[0]);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                selectedJob = potentialJob;
            }
        }

        return selectedJob;
    }

    public static void DequeueJob(Job job)
    {
        availableJobs.Remove(job);
    }

    public static void EnqueueJob(Job job)
    {
        availableJobs.Add(job);
    }

    public static void Reset()
    {
        inactiveJobs.Clear();
        availableJobs.Clear();
    }

    public static HashSet<T> FindJobOfType<T>() where T : class
    {
        HashSet<T> jobsOfType = new HashSet<T>();

        foreach (var job in availableJobs)
        {
            if (job is T wantedType)
            {
                jobsOfType.Add(wantedType);
            }
        }

        return jobsOfType;
    }
}