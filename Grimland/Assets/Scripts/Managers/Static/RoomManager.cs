using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RoomManager
{
    public static Dictionary<string, Room> Rooms { get; } = new Dictionary<string, Room>();

    public static void GenerateRooms()
    {
        // TODO: take into account doors here

        // Assume all regions have no rooms assigned to them
        HashSet<Region> roomlessRegions = new HashSet<Region>();

        foreach (Region region in RegionManager.Regions.Values)
        {
            roomlessRegions.Add(region);
        }

        while (roomlessRegions.Count > 0)
        {
            // Select a random starting region, then flood outwards 
            Region startingRegion = roomlessRegions.First();

            // Assigned the flooded regions to a newly created room until there are no roomless regions
            HashSet<Region> floodedRegions = FloodFill.FloodAllRegions(startingRegion);
            Room newRoom = new Room(floodedRegions);

            roomlessRegions.ExceptWith(floodedRegions);
        }
    }

    public static void JoinRooms(Region region, HashSet<Room> currentRooms)
    {
        switch (currentRooms.Count)
        {
            case 0: // No rooms found around node
                // Create a new room and add the region
                Room newRoom = new Room(new HashSet<Region> { region });
                newRoom.UpdateNeighbors();
                break;

            case 1: // Exactly one room found around node
                // Add the region to this existing room
                Room existingRoom = currentRooms.First();
                existingRoom.AddRegion(region);
                existingRoom.UpdateNeighbors();
                break;

            default: // Multiple rooms found around node
                // Transfer all the regions into the biggest room and add the region
                Room biggestRoom = FindBiggestRoom(currentRooms, true);

                biggestRoom.AddRegion(region);
                biggestRoom.UpdateNeighbors();

                break;
        }
    }

    public static void SplitRooms(Vector3Int node, Room roomToSplit)
    {
        HashSet<Region> affectedRegions = RegionManager.FindAnyRegions(node);

        // If only one region was in range, there is only one room so there is no need to split
        if (affectedRegions.Count <= 1)
        {
            return;
        }

        HashSet<HashSet<Region>> floodedRoomSets = new HashSet<HashSet<Region>>();

        bool abort = true;

        foreach (Region region in affectedRegions)
        {
            if (region is DoorRegion)
            {
                continue;
            }

            HashSet<Region> floodedRegions = FloodFill.FloodAllRegions(region);

            // If only a single region flood fill has a different number of flooded regions
            // than the room itself, then it needs to be split
            if (floodedRegions.Count != roomToSplit.RegionIds.Count)
            {
                abort = false;
            }

            // If we haven't already added these flooded regions, do so
            // We need this check because sometimes the same set of regions can exist but in a different order
            if (floodedRoomSets.All(setOfRegions => !setOfRegions.SetEquals(floodedRegions)))
            {
                floodedRoomSets.Add(floodedRegions);
            }
        }

        if (abort)
        {
            return;
        }

        // Delete the room we're about to split
        roomToSplit.Delete();

        // Create new, split rooms with their appropriate sets of regions we flood filled earlier
        foreach (HashSet<Region> setOfRegions in floodedRoomSets)
        {
            Room room = new Room(setOfRegions);
            room.UpdateNeighbors();
        }
    }

    private static Room FindBiggestRoom(ICollection<Room> roomsToCompare, bool transferRooms = false)
    {
        // Go through all rooms and decide which one is biggest based on count of regions
        var biggestRoom = roomsToCompare.Aggregate((room1, room2) => room1.RegionIds.Count > room2.RegionIds.Count ? room1 : room2);
        roomsToCompare.Remove(biggestRoom);

        if (transferRooms)
        {
            foreach (var room in roomsToCompare)
            {
                room.Transfer(biggestRoom);
            }
        }

        return biggestRoom;
    }

    public static HashSet<Room> GetRoomsFromRegions(IEnumerable<Region> regions)
    {
        HashSet<Room> rooms = new HashSet<Room>();

        foreach (var region in regions)
        {
            if (region is DoorRegion) continue;
            rooms.Add(region.GetRoom());
        }

        return rooms;
    }

    public static void Reset()
    {
        Rooms.Clear();
    }
}