using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WasZoneTampered : GrimConditional
{
    public SharedString ImportZoneId;

    private Zone zone;
    private bool zoneTampered;

    private void OnZoneTampered()
    {
        zoneTampered = true;
    }

    public override void OnStart()
    {
        base.OnStart();
        zoneTampered = false;
        zone = ZoneManager.Zones[ImportZoneId.Value];
        zone.OnZoneChanged += OnZoneTampered;
    }

    public override TaskStatus OnUpdate()
    {
        return zoneTampered ? TaskStatus.Failure : TaskStatus.Success;
    }

    public override void OnEnd()
    {
        zone.OnZoneChanged -= OnZoneTampered;
    }
}