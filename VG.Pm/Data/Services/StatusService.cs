using VG.Pm.Data.ViewModel;
using VG.Pm.PmDb;
using VG.Pm.PmDb.Models;

namespace VG.Pm.Data.Services
{
    public class StatusService
    {
        EFRepository<Status> repoStatus;
        private static PmDbContext DbContext;

        public StatusService(PmDbContext context)
        {
            repoStatus = new EFRepository<Status>(context);
            DbContext = context;
        }

        public List<StatusViewModel> Get()
        {
            var list = repoStatus.GetQuery().ToList();
            var result = list.Select(Convert).ToList();
            return result;
        }

        public static StatusViewModel Convert(Status r)
        {
            var item = new StatusViewModel(r);
            return item;
        }

        public StatusViewModel ReloadItem(StatusViewModel item)
        {
            var x = repoStatus.Reload(item.StatusId);
            if (x == null)
            {
                return null;
            }
            return Convert(x);
        }

        public void Delete(StatusViewModel item)
        {
            var x = repoStatus.FindById(item.StatusId);
            repoStatus.Remove(x);
        }

        public StatusViewModel Update(StatusViewModel item)
        {
            var x = repoStatus.FindById(item.StatusId);
            x.Title = item.Title;
            x.ChangeLogJson = item.ChangeLogJson;
            x.OrderId = item.OrderId;
            return Convert(repoStatus.Update(x));
        }

        public StatusViewModel Create(StatusViewModel item)
        {
            var newItem = repoStatus.Create(item.Item);
            return Convert(newItem);
        }
    }
}
