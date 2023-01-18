using System.Collections.Generic;
using UnityEngine;

public static class HaulReservationManager
{
    private static List<HaulReservation> Reservations { get; } = new List<HaulReservation>();

    public static bool CanReserve(Vector3Int node, out Creature owner)
    {
        foreach (var res in Reservations)
        {
            if (res.Node == node)
            {
                owner = res.Claimant;
                return false;
            }
        }

        owner = null;
        return true;
    }

    public static bool TryReserve(Creature claimant, Vector3Int node, Job job)
    {
        if (!CanReserve(node, out Creature owner))
        {
            // TODO: add actual name instead of owner object
            Debug.LogError($"Tried to add reservation on node that was already reserved by {owner}");
            return false;
        }

        HaulReservation haulReservation = new HaulReservation(claimant, node, job);
        Debug.Log($"[HaulReservationManager] Reserved node: {node}");
        Reservations.Add(haulReservation);
        return true;
    }

    public static void Release(Creature remover, Vector3Int node, Job job)
    {
        HaulReservation reservationToRemove = null;

        for (int i = 0; i < Reservations.Count; i++)
        {
            var res = Reservations[i];

            if (res.Claimant != remover || res.Node != node || res.Job != job)
            {
                continue;
            }

            reservationToRemove = res;
            break;
        }

        if (reservationToRemove != null)
        {
            Reservations.Remove(reservationToRemove);
        }
    }

    public static void ReleaseAllForNode(Vector3Int node)
    {
        for (int i = Reservations.Count - 1; i >= 0; i--)
        {
            if (Reservations[i].Node == node)
            {
                Reservations.RemoveAt(i);
            }
        }
    }

    // TODO: this should probably happen when the creature dies
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
                Reservations.RemoveAt(i);
            }
        }
    }


    public static void Reset()
    {
        Reservations.Clear();
    }
}

public class HaulReservation
{
    public Creature Claimant { get; }
    public Vector3Int Node { get; }
    public Job Job { get; }

    public HaulReservation(Creature claimant, Vector3Int node, Job job)
    {
        Claimant = claimant;
        Node = node;
        Job = job;
    }
}