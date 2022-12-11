using RoboRun.Model;
using RoboRun.Persistence;
using RoboRun.ViewModel;

namespace RoboRun.MAUI;

public partial class AppShell : Shell
{
    #region Private fields

    private IRoboRunDataAccess _roboRunDataAccess;
    private readonly RoboRunModel _roboRunModel;
    private readonly RoboRunViewModel _roboRunViewModel;

    private readonly IDispatcherTimer _timer;

    private readonly IStore _store;
    private readonly StoredGameBrowserModel _storedGameBrowserModel;
    private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;

    #endregion

    public AppShell()
	{
		InitializeComponent();
	}
}
