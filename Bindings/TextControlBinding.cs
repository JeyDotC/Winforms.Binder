using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winforms.Binder.Bindings
{
    public static class TextControlBinding
    {
        public static Binding<TViewModelData, string> BindTextTo<TViewModelData>(
            this Control control, 
            Expression<Func<TViewModelData>> propertyExpression) =>
            control.Bind(
                () => control.Text, 
                propertyExpression,
                Converters.ToStringConverter<TViewModelData>(),
                Converters.FromStringConverter<TViewModelData>());

        public static Binding<string, string> BindTextTo(
            this Control control, 
            Expression<Func<string>> propertyExpression) => 
            control.Bind(() => control.Text, propertyExpression);
    }
}
