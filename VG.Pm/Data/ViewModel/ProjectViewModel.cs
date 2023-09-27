using System.ComponentModel.DataAnnotations;
using VG.Pm.PmDb.Models;

namespace VG.Pm.Data.ViewModel
{
    public class ProjectViewModel
    {
        private Project _item;
        public Project Item => _item;

        public ProjectViewModel()
        {
            _item = new Project();

        }

        public ProjectViewModel(Project item)
        {
            _item = item;
        }

        [Key]
        public int ProjectId
        {
            get => _item.ProjectId;
            set => _item.ProjectId = value;
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
    }
}
