using System;

public static class GrimActionFailConditions
{
    public static T FailOn<T>(this T f, Func<bool> condition) where T : IHasFailCondition
    {
        f.AddFailCondition(delegate
        {
            if (condition())
            {
                return false;
            }

            return true;
        });
        return f;
    }
}