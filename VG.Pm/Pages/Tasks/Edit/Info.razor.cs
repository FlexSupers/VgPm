using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using System.Security.Claims;
using VG.Pm.Data.Services;
using VG.Pm.Data.ViewModel;
using VG.Pm.PmDb.Shared;

namespace VG.Pm.Pages.Tasks.Edit
{
    public class InfoView : ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public TaskViewModel TaskViewModel { get; set; } = new TaskViewModel();
        [Inject] protected TaskService Service { get; set; }
        [Inject] private LogApplicationErrorService LogService { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        public LogApplicationErrorViewModel Log = new LogApplicationErrorViewModel();
        protected List<TaskViewModel> Model { get; set; }
        protected List<ChangeLog> ChangeLogList { get; set; } = new();
        public ChangeLog mChangeLog = new ChangeLog();
        public TaskViewModel mCurrentItem;
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        protected async void Info()
        {
            try
            {
                ChangeLogList = JsonConvert.DeserializeObject<List<ChangeLog>>(TaskViewModel.ChangeLogJson);
            }
            catch (Exception ex)
            {
                LogService.Create(Log, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }

        }
        protected override async Task OnInitializedAsync()
        {
            Info();
        }
    }
}
