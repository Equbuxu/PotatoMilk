using System;

namespace PotatoMilk.Components
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentNameAttribute : System.Attribute
    {
        private string name;

        public ComponentNameAttribute(string name)
        {
            this.name = name;
        }

        public string GetName() => name;
    }
}
