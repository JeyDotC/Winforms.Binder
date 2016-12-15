using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms.Binder.Bindings
{
    public static class PropertyExtensions
    {
        internal static void ListenToPropertyChanged(this Prop prop)
        {
            if (prop.IsInstanceSet && prop.IsPropertyNameSet && prop.Instance is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)prop.Instance).PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == prop.PropertyName)
                    {
                        prop.Binding.UpdateCounterpartOf(prop);
                    }
                };
            }
        }
    }
}
