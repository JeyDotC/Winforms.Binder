using System;

namespace Winforms.Binder
{
    public class Binding<TViewModelData, TControlData> : IBinding
    {
        private bool _viewModelUpdatedFromEvent;
        private bool _controlUpdatedFromEvent;

        public Prop<TViewModelData> ViewModelProperty { get; set; }

        public Prop<TControlData> ControlProperty { get; set; }

        public Converter<TViewModelData, TControlData> ViewModelToControlConverter { get; set; }

        public Converter<TControlData, TViewModelData> ControlToViewModelConverter { get; set; }

        public Binding(Prop<TViewModelData> viewModelProperty, Prop<TControlData> controlProperty)
        {
            ViewModelProperty = viewModelProperty;
            ControlProperty = controlProperty;
            ControlProperty.Binding = this;
            ViewModelProperty.Binding = this;
        }

        public void UpdateViewModel()
        {
            if (!_viewModelUpdatedFromEvent)
            {
                _controlUpdatedFromEvent = true;
                ViewModelProperty.Setter(ControlToViewModelConverter(ControlProperty.Getter()));
                _controlUpdatedFromEvent = false;
            }
        }

        public void UpdateControl()
        {
            if (!_controlUpdatedFromEvent)
            {
                _viewModelUpdatedFromEvent = true;
                ControlProperty.Setter(ViewModelToControlConverter(ViewModelProperty.Getter()));
                _viewModelUpdatedFromEvent = false;
            }
        }

        public void UpdateCounterpartOf(Prop prop)
        {
            if (prop == ViewModelProperty)
            {
                UpdateControl();
            }
            else if (prop == ControlProperty)
            {
                UpdateViewModel();
            }
            else
            {
                throw new InvalidOperationException("The given property has no counterpart in this binding.");
            }
        }
    }
}
