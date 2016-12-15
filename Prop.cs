using System;
using System.Diagnostics.Contracts;

namespace Winforms.Binder
{
    public class Prop<T> : Prop
    {
        public Func<T> Getter { get; set; }

        public Action<T> Setter { get; set; }
        
        public Prop(Func<T> getter, Action<T> setter, object instance = null, string propertyName = null)
            :base(instance, propertyName)
        {
            Getter = getter;
            Setter = setter;
        }

    }

    public abstract class Prop
    {
        protected Prop(object instance = null, string propertyName = null)
        {
            Instance = instance;
            PropertyName = propertyName;
        }

        public IBinding Binding { get; internal set; }

        public object Instance { get; }
        public string PropertyName { get; }
        public bool IsInstanceSet => Instance != null;
        public bool IsPropertyNameSet => !string.IsNullOrWhiteSpace(PropertyName);

        public bool IsBound => Binding != null;
    }
}