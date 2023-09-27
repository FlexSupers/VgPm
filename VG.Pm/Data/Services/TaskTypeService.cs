using VG.Pm.Data.ViewModel;
using VG.Pm.PmDb;
using VG.Pm.PmDb.Models;

namespace VG.Pm.Data.Services
{
    public class TaskTypeService
    {
        EFRepository<TaskType> repoStatus;
        private static PmDbContext DbContext;

        public TaskTypeService(PmDbContext context)
        {
            repoStatus = new EFRepository<TaskType>(context);
            DbContext = context;
        }

        public List<TaskTypeViewModel> Get()
        {
            var list = repoStatus.GetQuery().ToList();
            var result = list.Select(Convert).ToList();
            return result;
        }

        public static TaskTypeViewModel Convert(TaskType r)
        {
            var item = new TaskTypeViewModel(r);
            return item;
        }

        public TaskTypeViewModel ReloadItem (TaskTypeViewModel item)
        {
            var x = repoStatus.Reload(item.TaskTypeId);
            if (x == null)
            {
                return null;
            }
            return Convert(x);
        }

        public void Delete(TaskTypeViewModel item)
        {
            var x = repoStatus.FindById(item.TaskTypeId);
            repoStatus.Remove(x);
        }

        public TaskTypeViewModel Update(TaskTypeViewModel item)
        {
            var x = repoStatus.FindById(item.TaskTypeId);
            x.Title = item.Title;
            x.ChangeLogJson = item.ChangeLogJson;
            return Convert(repoStatus.Update(x));
        }

        public TaskTypeViewModel Create(TaskTypeViewModel item)
        {
            var newItem = repoStatus.Create(item.Item);
            return Convert(newItem);
        }
    }
}
