using System.Collections.Generic;
using UnityEngine;

public static class ReservationManager
{
    private static List<ThingReservation> Reservations { get; } = new List<ThingReservation>();

    public static bool CanReserve(Thing target, ReservationType type, out Creature owner)
    {
        foreach (var res in Reservations)
        {
            if (res.Target == target && res.ReservationType == type)
            {
                owner = res.Claimant;
                return false;
            }
        }

        owner = null;
        return true;
    }

    public static bool TryReserve(Creature claimant, Thing target, Job job, ReservationType type)
    {
        if (!CanReserve(target, type, out Creature owner))
        {
            // TODO: add actual name instead of owner object
            return false;
        }

        ThingReservation thingReservation = new ThingReservation(claimant, target, job, type);
        Debug.Log($"[ReservationManager] Reserved {target.ThingDef.Name} on type {type}");
        Reservations.Add(thingReservation);
        return true;
    }

    // public static void ReleaseTypes(Creature remover, Thing target, Job job, List<ReservationType> types)
    // {
    //     foreach (var type in types)
    //     {
    //         Release(remover, target, job, type);
    //     }
    // }

    public static void Release(Creature remover, Thing target, Job job, ReservationType type)
    {
        ThingReservation reservationToRemove = null;

        for (int i = 0; i < Reservations.Count; i++)
        {
            var res = Reservations[i];

            if (res.Claimant != remover || res.Target != target || res.Job != job || res.ReservationType != type)
            {
                continue;
            }

            reservationToRemove = res;
            break;
        }

        if (reservationToRemove != null)
        {
            Reservations.Remove(reservationToRemove);
            Debug.Log($"[ReservationManager] Released {target} from type {type}. There are now {Reservations.Count} reservations left");
        }
    }

    public static void ReleaseAllForTarget(Thing target)
    {
        for (int i = Reservations.Count - 1; i >= 0; i--)
        {
            if (Reservations[i].Target == target)
            {
                Reservations.RemoveAt(i);
            }
        }
    }

    public static void ReleaseAllByCreature(Creature creature)
    {
        for (int i = Reservations.Count - 1; i >= 0; i--)
        {
            if (Reservations[i].Claimant == creature)
            {
                Reservations.RemoveAt(i);
            }
        }
    }

    public static void ReleaseAllByJob(Creature creature, Job job)
    {
        for (int i = Reservations.Count - 1; i >= 0; i--)
        {
            if (Reservations[i].Claimant == creature && Reservations[i].Job == job)
            {
                Debug.Log("Removing reservation by job");
                Reservations.RemoveAt(i);
            }
        }
    }


    public static void Reset()
    {
        Reservations.Clear();
    }
}

// TODO: interact is currently used for chopping trees but that may be too general
public enum ReservationType
{
    HaulFrom,
    HaulTo,
    Construct,
    CutPlant,
    Repair,
    Interact
}

public class ThingReservation
{
    public Creature Claimant { get; }
    public Thing Target { get; }
    public Job Job { get; }

    // TODO max pawns should depend on thing, not reservation
    // public int MaxPawns { get; set; }
    public ReservationType ReservationType { get; set; }

    public ThingReservation(Creature claimant, Thing target, Job job, ReservationType type)
    {
        Claimant = claimant;
        Target = target;
        Job = job;
        // MaxPawns = maxPawns;
        ReservationType = type;
    }
}