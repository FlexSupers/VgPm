using VG.Pm.Data.ViewModel;
using VG.Pm.PmDb;
using VG.Pm.PmDb.Models;

namespace VG.Pm.Data.Services
{
    public class ProjectService
    {
        EFRepository<Project> repoProj;
        private static PmDbContext DbContext;

        public ProjectService(PmDbContext context)
        {
            repoProj = new EFRepository<Project>(context);
            DbContext = context;
        }

        public List<ProjectViewModel> Get()
        {
            var list = repoProj.GetQuery().ToList();
            var result = list.Select(Convert).ToList();
            return result;
        }

        public static ProjectViewModel Convert(Project r)
        {
            var item = new ProjectViewModel(r);
            return item;
        }

        public ProjectViewModel ReloadItem(ProjectViewModel item)
        {
            var x = repoProj.Reload(item.ProjectId);
            if (x == null)
            {
                return null;
            }
            return Convert(x);
        }

        public void Delete(ProjectViewModel item)
        {
            var x = repoProj.FindById(item.ProjectId);
            repoProj.Remove(x);
        }

        public ProjectViewModel Update(ProjectViewModel item)
        {
            var x = repoProj.FindById(item.ProjectId);
            x.Title = item.Title;
            x.ChangeLogJson = item.ChangeLogJson;
            return Convert(repoProj.Update(x));
        }

        public ProjectViewModel Create(ProjectViewModel item)
        {
            var newItem = repoProj.Create(item.Item);
            return Convert(newItem);
        }

        public List<ProjectViewModel> Filtering(string y)
        {
            var filteredListRooms = repoProj.GetQuery().Where(x => (x.Title.StartsWith(y))).ToList();
            var result = filteredListRooms.Select(Convert).ToList();
            return result;
        }
    }
}
