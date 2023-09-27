using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Utilities;
using VG.Pm.Data.Services;
using VG.Pm.Data.ViewModel;
using VG.Pm.Pages.Tasks.Edit;
using static VG.Pm.Pages.Kanban.KanbanView;

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
        [Inject] protected IJSRuntime jsruntime { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        public ProjectViewModel ProjectViewModel { get; set; } = new ProjectViewModel();

        protected List<TaskViewModel> TaskList { get; set; }
        protected List<ProjectViewModel> ProjectModel { get; set; } = new();
        protected List<StatusViewModel> StatusModel { get; set; } = new();
        protected List<TaskTypeViewModel> TypeModel { get; set; } = new();
        public TaskViewModel task = new TaskViewModel();
        public LogApplicationErrorViewModel log = new LogApplicationErrorViewModel();
        public TaskViewModel CurrentItem;

        public List<DropItem> dropzoneItems = new();
        public List<DropItem> serverData = new();

        public MudDropContainer<DropItem> container;

        private HubConnection hubConnection;

        public string filterValue { get; set; }
        public int filterProj { get; set; }
        public int filterType { get; set; }

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
                //Model.Reverse();
                await LoadServerData();

                /*hubConnection = new HubConnectionBuilder()
                   .WithUrl(NavigationManager.ToAbsoluteUri("/boardHub"))
                   .Build();
                await hubConnection.StartAsync();
                hubConnection.On<TaskViewModel>("ItemUpdate", async (item) =>
                {
                    Console.WriteLine($"Item name:{item.Title}");
                    StateHasChanged();
                });*/
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
        public int FilterProj
        {
            get => filterProj;

            set
            {
                filterProj = value;
                FilterProject();
            }
        }
        public int FilterType
        {
            get => filterType;

            set
            {
                filterType = value;
                FilterTypes();
            }
        }
        protected async void Filter()
        {
            TaskList = Service.FilteringEmploers(filterValue);
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
        protected async void FilterProject()
        {
            TaskList = Service.FilteringProject(filterProj);
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
            TaskList = Service.FilteringType(filterType);
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
                    //returnModel = (UserViewModel)result.Data;
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
                    var index = TaskList.FindIndex(x => x.TaskId == newItem.TaskId);
                    TaskList[index] = newItem;
                    serverData.Add(new DropItem { TaskViewModel = newItem, Selector = StatusModel.FirstOrDefault(x => x.StatusId == newItem.StatusId).Title });
                    var oldItem = serverData.FirstOrDefault(x => x.TaskViewModel.TaskId == newItem.TaskId);
                    serverData.Remove(oldItem);
                    Snackbar.Add("Item changed", Severity.Success);
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
                LogService.Create(log, ex.Message, ex.StackTrace, ex.InnerException.Message, DateTime.Now);
            }

        }

        public async void Status()
        {

        }
        public async void ItemUpdated(MudItemDropInfo<DropItem> dropItem)
        {
            dropItem.Item.Selector = dropItem.DropzoneIdentifier;
            var list1 = StatusModel.FirstOrDefault(x => x.Title == dropItem.Item.Selector);
            dropItem.Item.TaskViewModel.StatusId = list1.StatusId;
            SaveData(dropItem.Item.TaskViewModel);
            Snackbar.Add("Item updated", Severity.Info);

            /*await hubConnection.SendCoreAsync("ItemUpdate", new object?[] {dropItem.Item.TaskViewModel });*/
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
            await RefreshContainer();
        }
        private async Task RefreshContainer()
        {

            await InvokeAsync(StateHasChanged);
            await Task.Delay(1);

            container?.Refresh();

            await Task.CompletedTask;
        }
        public class DropItem
        {
            public TaskViewModel TaskViewModel { get; init; }
            public string Selector { get; set; }
            public string Title { get; set; }
        }
    }
}
