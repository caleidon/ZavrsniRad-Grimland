using System.Collections.Generic;

public class Room
{
    public string Id { get; }
    public HashSet<string> RegionIds { get; }
    public HashSet<string> NeighborRoomIds { get; }

    public Room(HashSet<Region> regions)
    {
        string roomId = IdManager.GenerateGUID();
        Id = roomId;
        RegionIds = new HashSet<string>();
        NeighborRoomIds = new HashSet<string>();

        if (regions != null)
        {
            foreach (Region region in regions)
            {
                AddRegion(region);
            }
        }

        RoomManager.Rooms.Add(roomId, this);
    }


    public bool IsRoomAccessible(Room goalRoom)
    {
        // If the rooms are the same, then the goalRoom is accessible
        if (this == goalRoom) return true;

        HashSet<string> checkedRooms = new HashSet<string>();
        HashSet<string> roomsToCheck = new HashSet<string>();

        checkedRooms.Add(goalRoom.Id);
        roomsToCheck.UnionWith(goalRoom.NeighborRoomIds);

        while (roomsToCheck.Count > 0)
        {
            foreach (string roomToCheckId in roomsToCheck)
            {
                if (roomToCheckId == Id)
                {
                    return true;
                }

                Room checkedRoom = RoomManager.Rooms[roomToCheckId];
                checkedRooms.Add(roomToCheckId);
                roomsToCheck.Remove(roomToCheckId);

                foreach (string neighborRoomId in checkedRoom.NeighborRoomIds)
                {
                    if (!checkedRooms.Contains(neighborRoomId))
                    {
                        roomsToCheck.Add(neighborRoomId);
                    }
                }
            }
        }

        return false;
    }


    public void Transfer(Room newRoom)
    {
        foreach (string regionID in RegionIds)
        {
            newRoom.AddRegion(RegionManager.Regions[regionID]);
        }

        Delete();
    }

    public void UpdateNeighbors()
    {
        NeighborRoomIds.Clear();

        // Get all regions in this room
        foreach (var region in GetRegions())
        {
            // Get all neighbors of each region
            foreach (Region neighborRegion in region.GetNeighbors())
            {
                // For each neighbor region, get it's room
                Room neighborRoom = neighborRegion.GetRoom();

                // If we haven't already, add it as a neighbor room to this one
                if (neighborRoom != this)
                {
                    AddNeighborRoom(neighborRoom);
                }
            }
        }
    }

    public void AddRegion(Region region)
    {
        RegionIds.Add(region.Id);
        region.RoomId = Id;
    }

    public void RemoveRegion(Region region)
    {
        RegionIds.Remove(region.Id);
        region.RoomId = null;
    }

    private void AddNeighborRoom(Room neighborRoom)
    {
        NeighborRoomIds.Add(neighborRoom.Id);
        neighborRoom.NeighborRoomIds.Add(Id);
    }

    private HashSet<Region> GetRegions()
    {
        HashSet<Region> regions = new HashSet<Region>();

        foreach (string regionID in RegionIds)
        {
            Region region = RegionManager.Regions[regionID];
            regions.Add(region);
        }

        return regions;
    }

    private HashSet<Room> GetNeighborRooms()
    {
        HashSet<Room> neighborRooms = new HashSet<Room>();

        foreach (string neighborRoomID in NeighborRoomIds)
        {
            Room neighborRoom = RoomManager.Rooms[neighborRoomID];
            neighborRooms.Add(neighborRoom);
        }

        return neighborRooms;
    }

    public void Delete()
    {
        foreach (var neighborRoom in GetNeighborRooms())
        {
            neighborRoom.NeighborRoomIds.Remove(Id);
        }

        RoomManager.Rooms.Remove(Id);
    }
}