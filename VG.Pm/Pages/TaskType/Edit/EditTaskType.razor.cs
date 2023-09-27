using Microsoft.AspNetCore.Components;
using MudBlazor;
using VG.Pm.Data.Services;
using VG.Pm.Data.ViewModel;

namespace VG.Pm.Pages.TaskType.Edit
{
    public class EditTaskTypeView : ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public TaskTypeViewModel TaskTypeViewModel { get; set; } = new TaskTypeViewModel();
        [Parameter]
        public string Title { get; set; }
        [Inject] protected TaskTypeService Service { get; set; }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void Save()
        {
            MudDialog.Close(DialogResult.Ok(TaskTypeViewModel));
        }
    }
}
