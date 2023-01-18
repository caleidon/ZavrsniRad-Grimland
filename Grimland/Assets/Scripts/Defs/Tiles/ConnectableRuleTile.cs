using UnityEngine;
using UnityEngine.Tilemaps;

public class ConnectableRuleTile : RuleTile
{
    public enum ConnectionGroup
    {
        Wall,
        Fence
    }

    public ConnectionGroup Group { get; set; }

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is RuleOverrideTile overrideTile)
        {
            other = overrideTile.m_InstanceTile;
        }

        return neighbor switch
        {
            TilingRuleOutput.Neighbor.This => other is ConnectableRuleTile tile && tile.Group == Group,
            TilingRuleOutput.Neighbor.NotThis => !(other is ConnectableRuleTile tile && tile.Group == Group),
            _ => base.RuleMatch(neighbor, other)
        };
    }
}