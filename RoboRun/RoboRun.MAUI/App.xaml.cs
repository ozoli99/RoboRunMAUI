using Java.Security.Cert;
using RoboRun.Model;
using RoboRun.Persistence;
using RoboRun.ViewModel;

namespace RoboRun.MAUI;

public partial class App : Application
{
	#region Private constants

	private const string SuspendedGameSavePath = "SuspendedGame";

	#endregion

	#region Private fields

	private readonly AppShell _appShell;
	private readonly IRoboRunDataAccess _roboRunDataAccess;
	private readonly RoboRunModel _roboRunModel;
	private readonly IStore _roboRunStore;
	private readonly RoboRunViewModel _roboRunViewModel;

    #endregion

    #region Constructor

    public App()
	{
		InitializeComponent();

		_roboRunStore = new RoboRunStore();
		_roboRunDataAccess = new RoboRunFileDataAccess(FileSystem.AppDataDirectory);


	}

    #endregion
}
