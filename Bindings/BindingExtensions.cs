using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Winforms.Binder.Bindings
{
    public static class BindingExtensions
    {
        public static Binding<TViewModelData, TControlData> Bind<TControlData, TViewModelData>(
           Expression<Func<TControlData>> controlPropertyExpression,
           Expression<Func<TViewModelData>> propertyExpression,
           Converter<TControlData, TViewModelData> controlToViewModelConverter,
           Converter<TViewModelData, TControlData> viewModelToControlConverter
       )
        {
            var viewModelProp = CalculatePropertyFromExpression(propertyExpression);
            var controlProp = CalculatePropertyFromExpression(controlPropertyExpression);
            
            var binding = new Binding<TViewModelData, TControlData>(viewModelProp, controlProp)
            {
                ControlToViewModelConverter = controlToViewModelConverter,
                ViewModelToControlConverter = viewModelToControlConverter
            };

            binding.ControlProperty.ListenToPropertyChanged();
            binding.ViewModelProperty.ListenToPropertyChanged();

            return binding;
        }
        
        public static Binding<T, T> Bind<T>(
                Expression<Func<T>> controlPropertyExpression,
                Expression<Func<T>> propertyExpression
            )
            => Bind(controlPropertyExpression, propertyExpression,
                Converters.BypassConverter<T>(),
                Converters.BypassConverter<T>());

        public static Binding<TViewModelData, TControlData> WithControlToViewModelConverter
            <TViewModelData, TControlData>(
            this Binding<TViewModelData, TControlData> binding, 
            Converter< TControlData, TViewModelData> converter)
        {
            binding.ControlToViewModelConverter = converter;
            return binding;
        }

        public static Binding<TViewModelData, TControlData> WithViewModelToControlConverter
            <TViewModelData, TControlData>(
            this Binding<TViewModelData, TControlData> binding, 
            Converter<TViewModelData, TControlData> converter)
        {
            binding.ViewModelToControlConverter = converter;
            return binding;
        }

        private static Prop<T> CalculatePropertyFromExpression<T>(Expression<Func<T>> propertyExpression)
        {
            var expression = (MemberExpression)propertyExpression.Body;
            var viewModelPropertyInfo = (PropertyInfo)expression.Member;

            Func<T> viewModelGetter = () => default(T);
            Action<T> viewModelSetter = value => { };

            if (viewModelPropertyInfo.CanRead)
            {
                var x = Expression.Convert(expression, expression.Type);
                var viewModelLambda = Expression.Lambda<Func<T>>(x);
                viewModelGetter = viewModelLambda.Compile();
            }

            var viewModelInstance = default(object);

            if (viewModelPropertyInfo.CanWrite)
            {
                var viewModelInstanceLambda = (MemberExpression)expression.Expression;
                viewModelInstance = Expression.Lambda(viewModelInstanceLambda).Compile().DynamicInvoke();

                viewModelSetter = v => viewModelPropertyInfo.SetValue(viewModelInstance, v);
            }

            return new Prop<T>(viewModelGetter, viewModelSetter, viewModelInstance, viewModelPropertyInfo.Name);
        }
    }
}
