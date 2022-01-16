namespace ScheduleVkManager.UI.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; }

        public MainViewModel()
        {
            CurrentViewModel = new StartupViewModel();
        }
    }
}
