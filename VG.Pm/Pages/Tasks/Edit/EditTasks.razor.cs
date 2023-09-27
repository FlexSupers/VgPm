using Microsoft.AspNetCore.Components;
using MudBlazor;
using VG.Pm.Data.Services;
using VG.Pm.Data.ViewModel;

namespace VG.Pm.Pages.Tasks.Edit
{
    public class EditTasksView : ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public TaskViewModel TaskViewModel { get; set; } = new TaskViewModel();
        public StatusViewModel StatusViewModel { get; set; } = new StatusViewModel();
        public ProjectViewModel ProjectViewModel { get; set; } = new ProjectViewModel();
        public List<StatusViewModel> StatusList { get; set; } = new List<StatusViewModel>();
        public List<ProjectViewModel> ProjectList { get; set; } = new List<ProjectViewModel>();
        public List<TaskTypeViewModel> TypeList { get; set; } = new List<TaskTypeViewModel>();
        [Parameter]
        public string Title { get; set; }
        [Inject] protected TaskService Service { get; set; }
        [Inject] protected StatusService statService { get; set; }
        [Inject] protected ProjectService projService { get; set; }
        [Inject] protected TaskTypeService typeService { get; set; }
        protected async override Task OnInitializedAsync()
        {
            StatusList = statService.Get();
            ProjectList = projService.Get();
            TypeList = typeService.Get();
            await InvokeAsync(StateHasChanged);
        }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        public void Save()
        {
            MudDialog.Close(DialogResult.Ok(TaskViewModel));
        }
        
    }
}
