using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using VG.Pm.Data.Services;
using VG.Pm.Data.ViewModel;
using VG.Pm.Pages.Project.Edit;
using VG.Pm.Shared;

namespace VG.Pm.Pages.Project
{
    public class ProjectView : ComponentBase
    {
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] private ProjectService Service { get; set; }
        [Inject] private LogApplicationErrorService LogService { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] protected IJSRuntime jsruntime { get; set; }

        protected List<ProjectViewModel> Model { get; set; }
        public ProjectViewModel proj = new ProjectViewModel();
        public LogApplicationErrorViewModel log = new LogApplicationErrorViewModel();
        /*protected LogApplicationStackTraceView StackTraceModel = new LogApplicationStackTraceView();*/
        public ProjectViewModel CurrentItem;
        public string filterValue { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Model = Service.Get();
                //Model.Reverse();
                await InvokeAsync(StateHasChanged);
            }
        }
        public string FilterValue
        {
            get => filterValue;

            set
            {
                filterValue = value;
                Filter();
            }
        }
        protected void Filter()
        {
            Model = Service.Filtering(filterValue);
            StateHasChanged();
        }
        public async Task AddItemDialog()
        {
            try
            {
                var newItem = new ProjectViewModel();

                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditProject> { { x => x.ProjectViewModel, newItem } };
                parameters.Add(x => x.Title, "New Project");
                var dialog = DialogService.Show<EditProject>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    ProjectViewModel returnModel = new ProjectViewModel();
                    //returnModel = (UserViewModel)result.Data;
                    returnModel = newItem;
                    var newUser = Service.Create(returnModel);
                    Model.Add(newItem);
                    Snackbar.Add("Элемент сохранен", Severity.Success);
                    StateHasChanged();
                }

            }
            catch (Exception ex)
            {
                LogService.Create(log, ex.Message, ex.StackTrace, ex.InnerException.StackTrace, DateTime.Now);
            }
        }

        public async void EditItemDialog(ProjectViewModel item)
        {
            try
            {
                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditProject> { { x => x.ProjectViewModel, item } };
                parameters.Add(x => x.Title, "Edit Project");
                var dialog = DialogService.Show<EditProject>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    ProjectViewModel returnModel = new ProjectViewModel();
                    returnModel = (ProjectViewModel)result.Data;
                    var newItem = Service.Update(returnModel);
                    var index = Model.FindIndex(x => x.ProjectId == newItem.ProjectId);
                    Model[index] = newItem;
                    Snackbar.Add("Элемент сохранен", Severity.Success);
                    StateHasChanged();
                }
                else
                {
                    var oldItem = Service.ReloadItem(item);
                    var index = Model.FindIndex(x => x.ProjectId == oldItem.ProjectId);
                    Model[index] = oldItem;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                LogService.Create(log, ex.Message, ex.StackTrace, ex.InnerException.StackTrace, DateTime.Now);
            }

        }

        public async Task DeleteItemAsync(ProjectViewModel mCurrentItem)
        {
            try
            {
                var dialog = DialogService.Show<Delete>("Are you sure want to delete this project?");
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    Service.Delete(mCurrentItem);
                    Model.Remove(mCurrentItem);
                    Snackbar.Add("Элемент удален", Severity.Success);
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                LogService.Create(log, ex.Message, ex.StackTrace, ex.InnerException.StackTrace, DateTime.Now);
            }
        }
    }
}
