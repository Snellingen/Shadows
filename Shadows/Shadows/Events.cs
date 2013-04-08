using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shadows
{
    public static class Events
    {
        public static event MyEventHandler MyEvent;

        public static void FireMyEvent(Selected selected)
        {
            var evt = MyEvent;
            if (evt != null)
                evt(selected);
        }
    }
}
