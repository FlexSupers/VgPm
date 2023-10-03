using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using VG.Pm.Data.Services;
using VG.Pm.Data.ViewModel;
using VG.Pm.Pages.Status.Edit;
using VG.Pm.Pages.TaskType.Edit;
using VG.Pm.Shared;

namespace VG.Pm.Pages.TaskType
{
    public class TaskTypeView : ComponentBase
    {
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] private TaskTypeService Service { get; set; }
        [Inject] private LogApplicationErrorService LogService { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }

        protected List<TaskTypeViewModel> Model { get; set; }

        public LogApplicationErrorViewModel Log = new LogApplicationErrorViewModel();

        public TaskTypeViewModel mCurrentItem;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Model = Service.Get();

                await InvokeAsync(StateHasChanged);
            }
        }
        public async Task AddItemDialog()
        {
            try
            {
                var newItem = new TaskTypeViewModel();

                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditTaskType> { { x => x.TaskTypeViewModel, newItem } };
                parameters.Add(x => x.Title, "New Type");
                var dialog = DialogService.Show<EditTaskType>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    TaskTypeViewModel returnModel = new TaskTypeViewModel();
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
                LogService.Create(Log, ex.Message, ex.StackTrace, ex.InnerException.StackTrace, DateTime.Now);
            }
        }

        public async void EditItemDialog(TaskTypeViewModel item)
        {
            try
            {
                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditTaskType> { { x => x.TaskTypeViewModel, item } };
                parameters.Add(x => x.Title, "Edit Type");
                var dialog = DialogService.Show<EditTaskType>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    TaskTypeViewModel returnModel = new TaskTypeViewModel();
                    returnModel = (TaskTypeViewModel)result.Data;
                    var newItem = Service.Update(returnModel);
                    var index = Model.FindIndex(x => x.TaskTypeId == newItem.TaskTypeId);
                    Model[index] = newItem;
                    Snackbar.Add("Элемент сохранен", Severity.Success);
                    StateHasChanged();
                }
                else
                {
                    var oldItem = Service.ReloadItem(item);
                    var index = Model.FindIndex(x => x.TaskTypeId == oldItem.TaskTypeId);
                    Model[index] = oldItem;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                LogService.Create(Log, ex.Message, ex.StackTrace, ex.InnerException.StackTrace, DateTime.Now);
            }

        }

        public async Task DeleteItemAsync(TaskTypeViewModel mCurrentItem)
        {
            try
            {
                var dialog = DialogService.Show<Delete>("Are you sure want to delete this type?");
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
                LogService.Create(Log, ex.Message, ex.StackTrace, ex.InnerException.StackTrace, DateTime.Now);
            }
        }
    }
}
