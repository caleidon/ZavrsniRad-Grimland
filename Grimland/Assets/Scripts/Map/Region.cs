using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Region
{
    public string Id { get; set; }
    public Rect Rect { get; set; }
    public HashSet<Vector3Int> Nodes { get; set; }
    public string RoomId { get; set; }

    public HashSet<uint> CurrentLinks { get; set; }

    // TODO: regionthings can certainly be converted to use yield
    public HashSet<Thing> RegionThings { get; set; }

    public Region(Rect rect, HashSet<Vector3Int> regionNodes = null, HashSet<uint> links = null)
    {
        string regionId = IdManager.GenerateGUID();

        Id = regionId;
        Rect = rect;

        Nodes = regionNodes ?? new HashSet<Vector3Int>();
        CurrentLinks = links ?? new HashSet<uint>();
        RegionThings = new HashSet<Thing>();

        RegionManager.Regions.Add(regionId, this);

        if (regionNodes?.Count > 0)
        {
            AddToRegionNodes(regionNodes);
        }

        UpdateLinks();
    }

    public enum Direction : sbyte
    {
        Right = 0,
        Up = 1
    }

    public readonly struct LinkTile
    {
        public readonly Vector3Int node;
        public readonly Direction direction;
        public readonly bool isDoor;
        public readonly bool initialized;

        public LinkTile(Vector3Int node, Direction direction, bool isDoor)
        {
            this.node = node;
            this.direction = direction;
            this.isDoor = isDoor;
            initialized = true;
        }
    }

    public void AddNode(Vector3Int node)
    {
        Nodes.Add(node);

        if (RegionManager.RegionNodes.ContainsKey(node))
        {
            RegionManager.RegionNodes[node] = this;
        }
        else
        {
            RegionManager.RegionNodes.Add(node, this);
        }
    }

    public void RemoveNode(Vector3Int node)
    {
        Nodes.Remove(node);

        if (RegionManager.RegionNodes.ContainsKey(node))
        {
            RegionManager.RegionNodes.Remove(node);
        }
    }

    public void AddToRegionNodes(HashSet<Vector3Int> regionNodes)
    {
        foreach (var node in regionNodes)
        {
            if (RegionManager.RegionNodes.ContainsKey(node))
            {
                RegionManager.RegionNodes[node] = this;
            }
            else
            {
                RegionManager.RegionNodes.Add(node, this);
            }
        }
    }

    public void RemoveFromRegionNodes(HashSet<Vector3Int> regionNodes)
    {
        foreach (var node in regionNodes)
        {
            if (RegionManager.RegionNodes.ContainsKey(node))
            {
                RegionManager.RegionNodes.Remove(node);
            }
        }
    }

    public void UpdateNeighborLinks(Vector3Int node)
    {
        HashSet<Region> affectedNeighbors = GetNeighbors(node);

        foreach (Region affectedNeighbor in affectedNeighbors)
        {
            affectedNeighbor.UpdateLinks();
        }
    }

    public void UpdateLinks()
    {
        List<LinkTile> linkTiles = FindLinkTiles();
        HashSet<uint> newLinks = GenerateLinks(linkTiles);

        // Detect old links and delete them from the map's link store
        CurrentLinks.ExceptWith(newLinks);
        foreach (uint link in CurrentLinks)
        {
            DeleteLink(link);
        }

        foreach (uint link in newLinks)
        {
            // If the link already exists, add myself
            if (RegionManager.Links.TryGetValue(link, out HashSet<string> linkedRegions))
            {
                linkedRegions.Add(Id);
                continue;
            }

            // If the link doesn't exist, create it and add myself
            HashSet<string> newLinkedRegions = new HashSet<string> {Id};
            RegionManager.Links.Add(link, newLinkedRegions);
        }

        // Set the new links to the current ones
        CurrentLinks = newLinks;
    }

    public List<LinkTile> FindLinkTiles()
    {
        List<LinkTile> linkTiles = new List<LinkTile>();

        foreach (Vector3Int node in Nodes)
        {
            foreach (Vector3Int neighborOffset in DirectionUtils.SideOffsets)
            {
                Vector3Int neighborPos = node + neighborOffset;

                // If a neighbor is outside of the map, an obstacle or part of the same region, ignore it and continue
                // A tile is considered a LinkTile if all of these conditions are met
                if (!Map.Contains(neighborPos) || Nodes.Contains(neighborPos) || !NodeManager.GetNodeDataAt(neighborPos).IsWalkable)
                {
                    continue;
                }

                // A link tile consists of its coordinates and its direction
                var (linkTileNode, linkTileDirection) = GetLinkTileInformation(node, neighborPos, neighborOffset);
                bool isDoor = NodeManager.GetNodeDataAt(neighborPos).IsDoor;

                linkTiles.Add(new LinkTile(linkTileNode, linkTileDirection, isDoor));
            }
        }

        return linkTiles;
    }

    private static Tuple<Vector3Int, Direction> GetLinkTileInformation(Vector3Int node, Vector3Int neighborNode,
        Vector3Int offset)
    {
        if (offset == Vector3Int.down)
        {
            return new Tuple<Vector3Int, Direction>(node, Direction.Up);
        }

        if (offset == Vector3Int.left)
        {
            return new Tuple<Vector3Int, Direction>(node, Direction.Right);
        }

        if (offset == Vector3Int.up)
        {
            return new Tuple<Vector3Int, Direction>(neighborNode, Direction.Up);
        }

        if (offset == Vector3Int.right)
        {
            return new Tuple<Vector3Int, Direction>(neighborNode, Direction.Right);
        }

        return null;
    }

    private static HashSet<uint> GenerateLinks(List<LinkTile> linkTiles)
    {
        // Order all link tiles so that we iterate through the bottom-left most first, then go right and up
        List<LinkTile> sortedLinkTiles = linkTiles.OrderBy(lt => lt.node.x).ThenBy(lt => lt.node.y).ToList();

        HashSet<uint> newLinks = new HashSet<uint>();

        while (sortedLinkTiles.Count > 0)
        {
            LinkTile startingLinkTile = sortedLinkTiles[0];

            switch (startingLinkTile.direction)
            {
                case Direction.Right:
                    newLinks.Add(CalculateLink(startingLinkTile, Vector3Int.up, sortedLinkTiles));
                    break;
                case Direction.Up:
                    newLinks.Add(CalculateLink(startingLinkTile, Vector3Int.right, sortedLinkTiles));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            sortedLinkTiles.Remove(startingLinkTile);
        }

        return newLinks;
    }


    private static uint CalculateLink(LinkTile startingLinkTile, Vector3Int moveDirection, List<LinkTile> linkTiles)
    {
        uint linkLength = 1;

        // If the starting LinkTile is a door, we can calculate it immediately
        if (!startingLinkTile.isDoor)
        {
            Vector3Int neighborPos = startingLinkTile.node + moveDirection;
            LinkTile neighborLinkTile = linkTiles.Find(linkTile => linkTile.node == neighborPos);

            // Keep going and increment length as long as conditions are met
            while (neighborLinkTile.initialized && neighborLinkTile.direction == startingLinkTile.direction &&
                   !neighborLinkTile.isDoor)
            {
                linkLength++;
                linkTiles.Remove(neighborLinkTile);
                neighborPos += moveDirection;
                neighborLinkTile = linkTiles.Find(linkTile => linkTile.node == neighborPos);
            }
        }

        // A link has the following format:
        // {x}{y}{length}{direction}
        string linkString = $"{startingLinkTile.node.x:D3}{startingLinkTile.node.y:D3}{linkLength}{(moveDirection == Vector3Int.up ? "1" : "0")}";
        uint link = CRC32C.CalculateHash(linkString);
        return link;
    }

    private Region FindNeighborInLink(uint link)
    {
        HashSet<string> linkedRegions = RegionManager.Links[link];
        string neighborID = linkedRegions.First(regionID => regionID != Id);
        return RegionManager.Regions[neighborID];
    }

    public HashSet<Region> GetNeighbors(Vector3Int? node = null)
    {
        HashSet<Region> neighbors = new HashSet<Region>();

        if (node.HasValue)
        {
            neighbors = RegionManager.FindAnyRegions(node.Value);
            neighbors.Remove(this);
            return neighbors;
        }

        foreach (var link in CurrentLinks)
        {
            Region neighbor = FindNeighborInLink(link);
            neighbors.Add(neighbor);
        }

        return neighbors;
    }

    public void RemoveFromLinks()
    {
        foreach (uint link in CurrentLinks.ToList())
        {
            if (!RegionManager.Links.TryGetValue(link, out HashSet<string> linkedRegions))
            {
                return;
            }

            linkedRegions.Remove(Id);
        }

        CurrentLinks.Clear();
    }

    public void Delete()
    {
        RemoveFromLinks();
        GetRoom()?.RemoveRegion(this);
        RemoveFromRegionNodes(Nodes);
        RegionManager.Regions.Remove(Id);
    }

    public Room GetRoom()
    {
        if (RoomId != null)
        {
            return RoomManager.Rooms.TryGetValue(RoomId, out Room room) ? room : null;
        }

        return null;
    }

    private static void DeleteLink(uint link)
    {
        RegionManager.Links.Remove(link);
    }

    public int ItemAmountInRegionThings(ItemDef itemDef)
    {
        int amountFound = 0;

        foreach (var thing in RegionThings)
        {
            if (thing is Item item)
            {
                if (item.GetItemDef().IsSameAs(itemDef))
                {
                    amountFound += item.Amount;
                }
            }
        }

        return amountFound;
    }

    public bool TryFindItemInRegionThings(ItemDef itemDef, out Item foundItem)
    {
        foreach (var thing in RegionThings)
        {
            if (!(thing is Item item))
            {
                continue;
            }

            if (!item.GetItemDef().IsSameAs(itemDef))
            {
                continue;
            }

            foundItem = item;
            return true;
        }

        foundItem = null;
        return false;
    }
}