using System;
using BehaviorDesigner.Runtime;

[Serializable]
public class SharedHaulDestination : SharedVariable<HaulDestination>
{
    public static implicit operator SharedHaulDestination(HaulDestination value)
    {
        return new SharedHaulDestination { mValue = value };
    }
}