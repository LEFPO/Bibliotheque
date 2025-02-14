using bibliothecaire.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace bibliothecaire.ViewModel;

public class BaseViewModel : ObservableObject
{
    protected readonly DatabaseService _databaseService;

    public BaseViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }
}
