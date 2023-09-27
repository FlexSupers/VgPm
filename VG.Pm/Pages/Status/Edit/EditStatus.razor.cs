using Microsoft.AspNetCore.Components;
using MudBlazor;
using VG.Pm.Data.Services;
using VG.Pm.Data.ViewModel;

namespace VG.Pm.Pages.Status.Edit
{
    public class EditStatusView : ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public StatusViewModel StatusViewModel { get; set; } = new StatusViewModel();
        [Parameter]
        public string Title { get; set; }
        [Inject] protected StatusService Service { get; set; }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void Save()
        {
            MudDialog.Close(DialogResult.Ok(StatusViewModel));
        }
    }
}
