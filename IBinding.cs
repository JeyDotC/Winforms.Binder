namespace Winforms.Binder
{
    public interface IBinding
    {
        void UpdateViewModel();
        void UpdateControl();
        void UpdateCounterpartOf(Prop prop);
    }
}