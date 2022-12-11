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

		_roboRunModel = new RoboRunModel(_roboRunDataAccess);
		_roboRunViewModel = new RoboRunViewModel(_roboRunModel);

		_appShell = new AppShell(_roboRunStore, _roboRunDataAccess, _roboRunModel, _roboRunViewModel)
		{
			BindingContext = _roboRunViewModel
		};
		MainPage = _appShell;
	}

    #endregion

    #region Application methods

    protected override Window CreateWindow(IActivationState? activationState)
    {
		Window window = base.CreateWindow(activationState);

		window.Created += (s, e) =>
		{
            Random random = new Random();
            int x, y;
            x = random.Next((int)_roboRunModel.GameTableSize);
            y = random.Next((int)_roboRunModel.GameTableSize);
            while (x == (int)_roboRunModel.GameTableSize / 2 && y == (int)_roboRunModel.GameTableSize / 2)
            {
                x = random.Next((int)_roboRunModel.GameTableSize);
                y = random.Next((int)_roboRunModel.GameTableSize);
            }
            Array values = Enum.GetValues(typeof(Direction));
            Direction randomDirection = (Direction)values.GetValue(random.Next(values.Length));
            
			_roboRunModel.NewGame(x, y, randomDirection);
			_appShell.StartTimer();
		};

		window.Activated += (s, e) =>
		{
			if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, SuspendedGameSavePath)))
				return;

			Task.Run(async () =>
			{
				try
				{
					await _roboRunModel.LoadGameAsync(SuspendedGameSavePath);

					_appShell.StartTimer();
				}
				catch {}
			});
		};

		window.Stopped += (s, e) =>
		{
			Task.Run(async () =>
			{
				try
				{
					_appShell.StopTimer();
					await _roboRunModel.SaveGameAsync(SuspendedGameSavePath);
				}
				catch {}
			});
		};

		return window;
    }

    #endregion
}
