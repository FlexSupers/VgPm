﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using VG.Pm.Data.Services;
using VG.Pm.Data.ViewModel;
using VG.Pm.Pages.Project.Edit;
using VG.Pm.Pages.Tasks.Edit;
using VG.Pm.Shared;

namespace VG.Pm.Pages.Tasks
{
    public class TaskView : ComponentBase
    {
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] private ProjectService ProjService { get; set; }
        [Inject] private StatusService StatService { get; set; }
        [Inject] private TaskService Service { get; set; }
        [Inject] private LogApplicationErrorService LogService { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] protected IJSRuntime jsruntime { get; set; }

        protected List<TaskViewModel> Model { get; set; }
        protected List<ProjectViewModel> ProjectModel { get; set; } = new();
        protected List<StatusViewModel> StatusModel { get; set; } = new();

        public LogApplicationErrorViewModel log = new LogApplicationErrorViewModel();

        public TaskViewModel mCurrentItem;
        private string mTest;
        public string Test { get => mTest; set => mTest = value; }

        public string mFilterValue;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Model = Service.Get();
                ProjectModel = ProjService.Get();
                StatusModel = StatService.Get();

                await InvokeAsync(StateHasChanged);
            }
        }
        public string FilterValue
        {
            get => mFilterValue;

            set
            {
                mFilterValue = value;
                Filter();
            }
        }
        protected void Filter()
        {
            Model = Service.FilteringEmploers(mFilterValue);
            StateHasChanged();
        }
        public async Task AddItemDialog()
        {
            try
            {
                var newItem = new TaskViewModel();

                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditTasks> { { x => x.TaskViewModel, newItem } };
                parameters.Add(x => x.Title, "New Task");
                var dialog = DialogService.Show<EditTasks>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    TaskViewModel returnModel = new TaskViewModel();
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

        public async void EditItemDialog(TaskViewModel item)
        {
            try
            {
                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<EditTasks> { { x => x.TaskViewModel, item } };
                parameters.Add(x => x.Title, "Edit Task");
                var dialog = DialogService.Show<EditTasks>("", parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    TaskViewModel returnModel = new TaskViewModel();
                    returnModel = (TaskViewModel)result.Data;
                    var newItem = Service.Update(returnModel);
                    var index = Model.FindIndex(x => x.ProjectId == newItem.ProjectId);
                    Model[index] = newItem;
                    Snackbar.Add("Item updated", Severity.Success);
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

        public async Task DeleteItemAsync(TaskViewModel mCurrentItem)
        {
            try
            {
                var dialog = DialogService.Show<Delete>("Are you sure want to delete this task?");
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    Service.Delete(mCurrentItem);
                    Model.Remove(mCurrentItem);
                    Snackbar.Add("Item deleted", Severity.Success);
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
