using VG.Pm.Data.ViewModel;
using VG.Pm.PmDb;
using VG.Pm.PmDb.Models;
using Task = VG.Pm.PmDb.Models.Task;

namespace VG.Pm.Data.Services
{
    public class TaskService
    {
        EFRepository<Task> repoTask;
        private static PmDbContext DbContext;

        public TaskService(PmDbContext context)
        {
            repoTask = new EFRepository<Task>(context);
            DbContext = context;
        }

        public List<TaskViewModel> Get()
        {
            var list = repoTask.GetQuery().ToList();
            var result = list.Select(Convert).ToList();
            return result;
        }

        public static TaskViewModel Convert(Task r)
        {
            var item = new TaskViewModel(r);
            return item;
        }

        public TaskViewModel ReloadItem(TaskViewModel item)
        {
            var x = repoTask.Reload(item.TaskId);
            if (x == null)
            {
                return null;
            }
            return Convert(x);
        }

        public void Delete(TaskViewModel item)
        {
            var x = repoTask.FindById(item.TaskId);
            repoTask.Remove(x);
        }

        public TaskViewModel Update(TaskViewModel item)
        {
            var x = repoTask.FindById(item.TaskId);
            x.Title = item.Title;
            x.ChangeLogJson = item.ChangeLogJson;
            x.ProjectId = item.ProjectId;
            x.StatusId = item.StatusId;
            x.Description = item.Description;
            x.TaskTypeId = item.TaskTypeId;
            return Convert(repoTask.Update(x));
        }

        public TaskViewModel Create(TaskViewModel item)
        {
            var newItem = repoTask.Create(item.Item);
            return Convert(newItem);
        }
        public List<TaskViewModel> FilteringEmploers(string y)
        {
            var filteredListRooms = repoTask.GetQuery().Where(x => (x.Title.Contains(y) || x.Description.Contains(y))).ToList();
            var result = filteredListRooms.Select(Convert).ToList();
            return result;
        }
        public List<TaskViewModel> FilteringProject(int y)
        {
            var filteredProjects = repoTask.GetQuery().Where(x => (x.ProjectId == y));
            var result = filteredProjects.Select(Convert).ToList();

            return result;
        }
        public List<TaskViewModel> FilteringType(int y)
        {
            var filteredProjects = repoTask.GetQuery().Where(x => (x.TaskTypeId == y));
            var result = filteredProjects.Select(Convert).ToList();

            return result;
        }
    }
}
