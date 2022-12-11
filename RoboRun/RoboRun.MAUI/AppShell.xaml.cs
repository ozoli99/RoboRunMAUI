using RoboRun.MAUI.View;
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

    #region Application methods

    public AppShell(IStore roboRunStore, IRoboRunDataAccess roboRubDataAccess, RoboRunModel roboRunModel, RoboRunViewModel roboRunViewModel)
	{
		InitializeComponent();

        _store = roboRunStore;
        _roboRunDataAccess = roboRubDataAccess;
        _roboRunModel = roboRunModel;
        _roboRunViewModel = roboRunViewModel;

        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (_, _) => _roboRunModel.AdvanceTime();

        _roboRunModel.GameWin += RoboRunModel_GameWin;

        _roboRunViewModel.NewGame += RoboRunViewModel_NewGame;
        _roboRunViewModel.LoadGame += RoboRunViewModel_LoadGame;
        _roboRunViewModel.SaveGame += RoboRunViewModel_SaveGame;
        _roboRunViewModel.ExitGame += RoboRunViewModel_ExitGame;

        _storedGameBrowserModel = new StoredGameBrowserModel(_store);
        _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
        _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
        _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
    }

    #endregion

    #region Internal methods

    internal void StartTimer() => _timer.Start();

    internal void StopTimer() => _timer.Stop();

    #endregion

    #region Model event handlers

    private async void RoboRunModel_GameWin(object? sender, RoboRunEventArgs e)
    {
        StopTimer();


    }

    #endregion

    #region ViewModel event handlers

    private void RoboRunViewModel_NewGame(object? sender, EventArgs e)
    {
        _roboRunModel.NewGame();

        StartTimer();
    }

    private async void RoboRunViewModel_LoadGame(object? sender, EventArgs e)
    {
        await _storedGameBrowserModel.UpdateAsync();
        await Navigation.PushAsync(new LoadGamePage
        {
            BindingContext = _storedGameBrowserViewModel
        });
    }

    private async void RoboRunViewModel_SaveGame(object? sender, EventArgs e)
    {
        await _storedGameBrowserModel.UpdateAsync();
        await Navigation.PushAsync(new SaveGamePage
        {
            BindingContext = _storedGameBrowserViewModel
        });
    }

    private async void RoboRunViewModel_ExitGame(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage
        {
            BindingContext = _roboRunViewModel
        });
    }

    private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
    {
        await Navigation.PopAsync();

        try
        {
            await _roboRunModel.LoadGameAsync(e.Name);

            await Navigation.PopAsync();
            await DisplayAlert("RoboRun Game", "Load was successfull", "Ok");

            StartTimer();
        }
        catch
        {
            await DisplayAlert("RoboRun Game", "Load failed", "Ok");
        }
    }

    private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
    {
        await Navigation.PopAsync();
        StopTimer();

        try
        {
            await _roboRunModel.SaveGameAsync(e.Name);
            await DisplayAlert("RoboRun Game", "Save was successfull", "Ok");
        }
        catch
        {
            await DisplayAlert("RoboRun Game", "Save failed", "Ok");
        }
    }

    #endregion
}
