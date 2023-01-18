using System;
using UnityEngine;

public abstract class Thing
{
    public enum ThingInteractionNode
    {
        None,
        Center,
        Sides,
        Corners,
        InteractionNode,
        Anywhere
    }

    public string Id { get; set; }
    public bool IsDestroyed { get; set; }
    public ThingDef ThingDef { get; set; }
    public virtual bool IsDamagable => true;

    public abstract bool IsRegionThing { get; }
    public abstract Vector3Int GetNode();
    public abstract ThingInteractionNode InteractionNode { get; }

    // TODO: likely make this abstract so it has to be implemented
    // TODO: implement damage info
    public virtual void Damage(int damage)
    {
        if (!IsDamagable) { }
    }

    public virtual void Destroy(DestroyMode destroyMode)
    {
        IdManager.RemoveFromThingList(Id);
        IsDestroyed = true;

        switch (destroyMode)
        {
            case DestroyMode.Normal:
                break;
            case DestroyMode.Vanish:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(destroyMode), destroyMode, null);
        }
    }

    protected void GenerateThingId(string previousId)
    {
        Id = IdManager.GenerateThingID(this, previousId);
    }

    // TODO: also make this recieve options to what ticks it needs to subscribe
    protected void SubscribeToTicks()
    {
        TickManager.OnTick += Tick;
        TickManager.OnMediumTick += MediumTick;
        TickManager.OnLongTick += LongTick;
    }

    protected virtual void Tick() { }

    protected virtual void MediumTick() { }

    protected virtual void LongTick() { }

    protected virtual void MegaTick() { }
}