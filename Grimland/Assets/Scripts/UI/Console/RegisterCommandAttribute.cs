using System;

namespace CommandTerminal
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RegisterCommandAttribute : Attribute
    {
        public int MinArgCount { get; set; }

        public int MaxArgCount { get; set; } = -1;

        public string Name { get; set; }
        public string Help { get; set; }
        public string Hint { get; set; }

        public RegisterCommandAttribute(string commandName = null)
        {
            Name = commandName;
        }
    }
}