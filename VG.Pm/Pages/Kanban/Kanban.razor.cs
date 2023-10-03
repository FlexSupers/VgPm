using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using VG.Pm.Data.Services;
using VG.Pm.Data.ViewModel;
using VG.Pm.Pages.Tasks.Edit;

namespace VG.Pm.Pages.Kanban
{
    public class KanbanView : ComponentBase
    {
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] private ProjectService ProjService { get; set; }
        [Inject] private TaskTypeService TypeService { get; set; }
        [Inject] private StatusService StatService { get; set; }
        [Inject] private TaskService Service { get; set; }
        [Inject] private LogApplicationErrorService LogService { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }

        public ProjectViewModel ProjectViewModel { get; set; } = new ProjectViewModel();

        protected List<TaskViewModel> TaskList { get; set; }
        protected List<ProjectViewModel> ProjectModel { get; set; } = new();
        protected List<StatusViewModel> StatusModel { get; set; } = new();
        protected List<TaskTypeViewModel> TypeModel { get; set; } = new();
        public TaskViewModel TaskModel = new TaskViewModel();
        public LogApplicationErrorViewModel Log = new LogApplicationErrorViewModel();
        public TaskViewModel mCurrentItem;

        public List<DropItem> dropzoneItems = new();
        public List<DropItem> serverData = new();

        public MudDropContainer<DropItem> container;

        public string mFilterTasks;
        public int mFilterProjects;
        public int mFilterTypes;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                TaskList = Service.Get();
                ProjectModel = ProjService.Get();
                StatusModel = StatService.Get();
                TypeModel = TypeService.Get();
                var i = 0;
                foreach (var item in TaskList)
                {
                    var rez = new DropItem() { TaskViewModel = item, Selector = StatusModel.FirstOrDefault(x => x.StatusId == item.StatusId).Title };
                    serverData.Add(rez);
                    i++;
                }
                await LoadServerData();
            }
        }
        public string FilterTasks
        {
            get => mFilterTasks;

            set
            {
                mFilterTasks = value;
                FilterTask();
            }
        }
        public int FilterProject
        {
            get => mFilterProjects;

            set
            {
                mFilterProjects = value;
                FilterProjects();
            }
        }
        public int FilterType
        {
            get => mFilterTypes;

            set
            {
                mFilterTypes = value;
                FilterTypes();
            }
        }
        protected async void FilterTask()
        {
            TaskList = Service.FilteringEmploers(mFilterTasks);
            var i = 0;
            serverData.Clear();
            foreach (var item in TaskList)
            {
                var rez = new DropItem() { TaskViewModel = item, Selector = StatusModel.FirstOrDefault(x => x.StatusId == item.StatusId).Title };
                serverData.Add(rez);
                i++;
            }
            await LoadServerData();
            StateHasChanged();
        }
        protected async void FilterProjects()
        {
            TaskList = Service.FilteringProject(mFilterProjects);
            var i = 0;
            serverData.Clear();
            foreach (var item in TaskList)
            {
                var rez = new DropItem() { TaskViewModel = item, Selector = StatusModel.FirstOrDefault(x => x.StatusId == item.StatusId).Title };
                serverData.Add(rez);
                i++;
            }
            await LoadServerData();
            StateHasChanged();
        }
        protected async void FilterTypes()
        {
            TaskList = Service.FilteringType(mFilterTypes);
            var i = 0;
            serverData.Clear();
            foreach (var item in TaskList)
            {
                var rez = new DropItem() { TaskViewModel = item, Selector = StatusModel.FirstOrDefault(x => x.StatusId == item.StatusId).Title };
                serverData.Add(rez);
                i++;
            }
            await LoadServerData();
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
                    TaskList.Add(newItem);
                    Snackbar.Add("Item added", Severity.Success);
                    dropzoneItems.Add(new DropItem { TaskViewModel = returnModel, Selector = StatusModel.FirstOrDefault(x => x.StatusId == returnModel.StatusId).Title });
                    serverData.Add(new DropItem { TaskViewModel = returnModel, Selector = StatusModel.FirstOrDefault(x => x.StatusId == returnModel.StatusId).Title });
                    container.Refresh();
                    await LoadServerData();
                    StateHasChanged();
                }

            }
            catch (Exception ex)
            {
                LogService.Create(Log, ex.Message, ex.StackTrace, ex.InnerException.StackTrace, DateTime.Now);
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
                    var index = TaskList.FindIndex(x => x.TaskId == newItem.TaskId);
                    TaskList[index] = newItem;
                    serverData.Add(new DropItem { TaskViewModel = newItem, Selector = StatusModel.FirstOrDefault(x => x.StatusId == newItem.StatusId).Title });
                    var oldItem = serverData.FirstOrDefault(x => x.TaskViewModel.TaskId == newItem.TaskId);
                    serverData.Remove(oldItem);
                    Snackbar.Add("Item changed", Severity.Info);
                    container.Refresh();
                    await LoadServerData();
                    StateHasChanged();
                }
                else
                {
                    var oldItem = Service.ReloadItem(item);
                    var index = TaskList.FindIndex(x => x.TaskId == oldItem.TaskId);
                    TaskList[index] = oldItem;
                    container.Refresh();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                LogService.Create(Log, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }

        }
        public async void ItemUpdated(MudItemDropInfo<DropItem> dropItem)
        {
            dropItem.Item.Selector = dropItem.DropzoneIdentifier;
            var list1 = StatusModel.FirstOrDefault(x => x.Title == dropItem.Item.Selector);
            dropItem.Item.TaskViewModel.StatusId = list1.StatusId;
            SaveData(dropItem.Item.TaskViewModel);
            Snackbar.Add("Item updated", Severity.Info);
        }

        public async void SaveData(TaskViewModel item)
        {
            try
            {
                if (item.StatusId > 0)
                {
                    var newItem = Service.Update(item);
                    var index = StatusModel.FindIndex(x => x.StatusId == newItem.StatusId);
                    TaskList[index] = newItem;
                }
                else
                {
                    var newItem = Service.Create(item);
                    TaskList.Add(newItem);
                }
                StateHasChanged();

            }
            catch (Exception ex)
            {
                /*LogService.Create(log, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);*/
            }
        }

        private async Task LoadServerData()
        {
            dropzoneItems = serverData
                .Select(item => new DropItem()
                {
                    TaskViewModel = item.TaskViewModel,
                    Selector = item.Selector
                })
                .ToList();
             RefreshContainer();
        }
        private void RefreshContainer()
        {
            StateHasChanged();
            container?.Refresh();
        }
        public class DropItem
        {
            public TaskViewModel TaskViewModel { get; init; }
            public string Selector { get; set; }
        }
        public async Task InfoItemAsync(TaskViewModel item)
        {
            try
            {
                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
                var parameters = new DialogParameters<Info> { { x => x.TaskViewModel, item } };
                parameters.Add(x => x.Title, "Info");
                var dialog = DialogService.Show<Info>("", parameters, options);
                var result = await dialog.Result;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                //LogService.Create(Log, ex.Message, ex.StackTrace, ex.InnerException.StackTrace, DateTime.Now);
            }
        }
    }
}
