using System;
using Game.System;

public class DefaultEvent : IPubSub.IEvent
{
    public DefaultEvent()
    {
    }

    public override string ToString()
    {
        return "foobar";
    }
}
