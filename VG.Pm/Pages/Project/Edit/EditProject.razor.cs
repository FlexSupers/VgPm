using Microsoft.AspNetCore.Components;
using MudBlazor;
using VG.Pm.Data.Services;
using VG.Pm.Data.ViewModel;

namespace VG.Pm.Pages.Project.Edit
{
    public class EditProjectView : ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public ProjectViewModel ProjectViewModel { get; set; } = new ProjectViewModel();
        [Parameter]
        public string Title { get; set; }
        [Inject] protected ProjectService Service { get; set; }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void Save()
        {
            MudDialog.Close(DialogResult.Ok(ProjectViewModel));
        }
    }
}
