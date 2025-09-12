namespace MeetUp.EShop.Presentation.CircuitServicesAccesor
{
    public class CircuitServicesAccesor
    {
        static readonly AsyncLocal<IServiceProvider> _blazoredService = new();

        public IServiceProvider? Service
        {
            get => _blazoredService.Value;
            set => _blazoredService.Value = value;
        }
    }
}
