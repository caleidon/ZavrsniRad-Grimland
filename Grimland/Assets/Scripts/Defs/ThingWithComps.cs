using System.Collections.Generic;

public abstract class ThingWithComps : Thing
{
    private List<Component> Comps;

    protected override void Tick()
    {
        if (Comps != null)
        {
            int i = 0;
            int count = Comps.Count;
            while (i < count)
            {
                Comps[i].Tick();
                i++;
            }
        }
    }

    protected override void MediumTick()
    {
        if (Comps != null)
        {
            int i = 0;
            int count = Comps.Count;
            while (i < count)
            {
                Comps[i].MediumTick();
                i++;
            }
        }
    }

    protected override void LongTick()
    {
        if (Comps != null)
        {
            int i = 0;
            int count = Comps.Count;
            while (i < count)
            {
                Comps[i].LongTick();
                i++;
            }
        }
    }

    protected override void MegaTick()
    {
        if (Comps != null)
        {
            int i = 0;
            int count = Comps.Count;
            while (i < count)
            {
                Comps[i].MegaTick();
                i++;
            }
        }
    }
}