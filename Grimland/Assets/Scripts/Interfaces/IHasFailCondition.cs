using System;
using System.Collections.Generic;

public interface IHasFailCondition
{
    public List<Func<bool>> FailConditions { get; set; }
    void AddFailCondition(Func<bool> failCondition);
}