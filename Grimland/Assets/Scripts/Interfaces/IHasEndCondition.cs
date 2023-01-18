using System;
using System.Collections.Generic;

public interface IHasEndCondition
{
    public List<Func<bool>> EndConditions { get; set; }
    void AddEndCondition(Func<bool> endCondition);
}