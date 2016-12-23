using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winforms.Binder.Bindings
{
    public static class ControlBindingExtensions
    {
        public static Binding<TViewModelData, TControlData> Bind<TViewModelData, TControlData>(this Control control, 
            Expression<Func<TControlData>> controlExpression,
            Expression<Func<TViewModelData>> propertyExpression,
            Converter<TViewModelData, TControlData> viewModelToControlConverter,
            Converter<TControlData, TViewModelData> controlToViewModelConverter)
        {
            var binding = BindingExtensions.Bind(
                controlExpression,
                propertyExpression,
                viewModelToControlConverter: viewModelToControlConverter,
                controlToViewModelConverter: controlToViewModelConverter);

            binding.ControlProperty.ToThreadSafeControlSetterIfApplicable();
            binding.ViewModelProperty.ToThreadSafeControlSetterIfApplicable();

            control.TextChanged += (sender, args) => binding.UpdateViewModel();

            return binding;
        }

        public static Binding<T, T> Bind<T>(this Control control,
            Expression<Func<T>> controlExpression,
            Expression<Func<T>> propertyExpression)
            => control.Bind(
                controlExpression,
                propertyExpression,
                Converters.BypassConverter<T>(),
                Converters.BypassConverter<T>());

        private static void ToThreadSafeControlSetterIfApplicable<T>(this Prop<T> controlProperty)
        {
            if (controlProperty.IsInstanceSet && controlProperty.Instance is Control)
            {
                controlProperty.Setter = MakeControlSetterSafe((Control) controlProperty.Instance,
                    controlProperty.Setter);
            }
        }

        private static Action<TControlData> MakeControlSetterSafe<TControlData>(Control control, Action<TControlData> controlSetter)
        {
            var invokeSetControlData = new SetControlValueCallback<TControlData>(controlSetter);

            return s =>
            {
                if (control.InvokeRequired)
                {
                    var findForm = control.FindForm();
                    if (findForm != null)
                    {
                        findForm.Invoke(invokeSetControlData, s);
                    }
                    else
                    {
                        throw new InvalidOperationException("This control is not attached to a winform.");
                    }
                }
                else
                {
                    controlSetter(s);
                }
            };
        }

        private delegate void SetControlValueCallback<in TControlData>(TControlData text);
    }
}
