using System;

namespace BrickPile.Domain {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ControlsAttribute : Attribute {
        public Type Type { get; set; }
        public ControlsAttribute(Type type) {
            Type = type;
        }
    }
}