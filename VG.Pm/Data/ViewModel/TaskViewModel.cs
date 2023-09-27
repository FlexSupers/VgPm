using System.ComponentModel.DataAnnotations;
using VG.Pm.PmDb.Models;
using Task = VG.Pm.PmDb.Models.Task;

namespace VG.Pm.Data.ViewModel
{
    public class TaskViewModel
    {
        private Task _item;
        public Task Item => _item;

        public TaskViewModel()
        {
            _item = new Task();

        }

        public TaskViewModel(Task item)
        {
            _item = item;
        }

        [Key]
        public int TaskId
        {
            get => _item.TaskId;
            set => _item.TaskId = value;
        }

        public string Title
        {
            get => _item.Title;
            set => _item.Title = value;
        }
        public string? ChangeLogJson
        {
            get => _item.ChangeLogJson;
            set => _item.ChangeLogJson = value;
        }
        public byte[] Timestamp
        {
            get => _item.Timestamp;
            set => _item.Timestamp = value;
        }
        public int ProjectId
        {
            get => _item.ProjectId;
            set => _item.ProjectId = value;
        }
        public int StatusId
        {
            get => _item.StatusId;
            set => _item.StatusId = value;
        }
        public string Description
        {
            get => _item.Description;
            set => _item.Description = value;
        }
        public int TaskTypeId
        {
            get => _item.TaskTypeId;
            set => _item.TaskTypeId = value;
        }
    }
}
