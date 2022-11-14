using Ahed_project.MasterData;
using Ahed_project.MasterData.ContentClasses;
using Ahed_project.Services;
using Ahed_project.Services.Global;
using Ahed_project.Services.Global.Interface;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel : BindableBase
    {
        public ObservableCollection<LoggerMessage> Logs
        {
            get => _logs.Logs;
        }
        private readonly PageService _pageService;
        private readonly IUnitedStorage _storage;
        public PageStates PageStates
        {
            get => _storage.GetPageStates();
            set=> _storage.SetPageStates(value);
        }
        public ContentInGlobal Data
        {
            get => _storage.GetContentData();
            set=> _storage.SetContentData(value);
        }
        private readonly LogsStorage _logs;
        public ContentPageViewModel(PageService pageService, IUnitedStorage storage, LogsStorage logs)
        {
            _pageService = pageService;
            _storage = storage;
            _logs = logs;
        }

    }
}
