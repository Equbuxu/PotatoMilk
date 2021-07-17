using System;

namespace PotatoMilk.Components
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ObjectNameAttribute : System.Attribute
    {
        private string name;

        public ObjectNameAttribute(string name)
        {
            this.name = name;
        }

        public string GetName() => name;
    }
}
