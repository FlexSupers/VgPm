using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace VG.Pm.Shared
{
    public class DeleteView : ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        public bool Answer { get; set; } = true;

        public void Delete()
        {
            MudDialog.Close(DialogResult.Ok(Answer));
        }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
