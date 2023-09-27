using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using VG.Pm.Data.Services;
using VG.Pm.Data.ViewModel;
using VG.Pm.Pages.Project.Edit;
using VG.Pm.Pages.Status.Edit;
using VG.Pm.Shared;

namespace VG.Pm.Pages.Status
{
    public class StatusView : ComponentBase
    {
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] private StatusService Service { get; set; }
        [Inject] private LogApplicationErrorService LogService { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] protected IJSRuntime jsruntime { get; set; }

        protected List<StatusViewModel> Model { get; set; }
        public StatusViewModel stat = new StatusViewModel();
        public LogApplicationErrorViewModel log = new LogApplicationErrorViewModel();
        /*protected LogApplicationStackTraceView StackTraceModel = new LogApplicationStackTraceView();*/
        public StatusViewModel CurrentItem;
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
        public async Task AddItemDialog()
        {
            try
            {
                var newItem = new StatusViewModel();

                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditStatus> { { x => x.StatusViewModel, newItem } };
                parameters.Add(x => x.Title, "New Status");
                var dialog = DialogService.Show<EditStatus>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    StatusViewModel returnModel = new StatusViewModel();
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

        public async void EditItemDialog(StatusViewModel item)
        {
            try
            {
                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditStatus> { { x => x.StatusViewModel, item } };
                parameters.Add(x => x.Title, "Edit Status");
                var dialog = DialogService.Show<EditStatus>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    StatusViewModel returnModel = new StatusViewModel();
                    returnModel = (StatusViewModel)result.Data;
                    var newItem = Service.Update(returnModel);
                    var index = Model.FindIndex(x => x.StatusId == newItem.StatusId);
                    Model[index] = newItem;
                    Snackbar.Add("Элемент сохранен", Severity.Success);
                    StateHasChanged();
                }
                else
                {
                    var oldItem = Service.ReloadItem(item);
                    var index = Model.FindIndex(x => x.StatusId == oldItem.StatusId);
                    Model[index] = oldItem;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                LogService.Create(log, ex.Message, ex.StackTrace, ex.InnerException.StackTrace, DateTime.Now);
            }

        }

        public async Task DeleteItemAsync(StatusViewModel mCurrentItem)
        {
            try
            {
                var dialog = DialogService.Show<Delete>("Are you sure want to delete this status?");
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
