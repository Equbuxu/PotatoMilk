using PotatoMilk.Components;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PotatoMilk.Helpers
{
    public static class ComponentHelper
    {
        public static T TryGetComponent<T>(GameObject container, [CallerMemberName] string callerName = "Unknown component")
            where T : IComponent
        {
            try
            {
                return container.GetComponent<T>();
            }
            catch (Exception e)
            {
                throw new Exception(callerName + " depends on" + typeof(T), e);
            }
        }

        public static T TryGetDataValue<T>(Dictionary<string, object> data, string name, T defValue)
        {
            if (data == null)
                return defValue;
            data.TryGetValue(name, out object value);
            if (value == null || value is not T)
                return defValue;
            return (T)value;
        }
    }
}
