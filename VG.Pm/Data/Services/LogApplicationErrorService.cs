using System.Reflection;
using VG.Pm.Data.ViewModel;
using VG.Pm.PmDb;
using VG.Pm.PmDb.Models;
using VG.Pm.PmDb.Shared;

namespace VG.Pm.Data.Services
{
    public class LogApplicationErrorService
    {
        EFRepository<LogApplicationError> repoLog;
        private static PmDbContext DbContext;

        public LogApplicationErrorService(PmDbContext context)
        {
            repoLog = new EFRepository<LogApplicationError>(context);
            DbContext = context;
        }
        public async Task<List<LogApplicationErrorViewModel>> GetAll()
        {
            var listItems = repoLog.Get();
            var result = listItems.Select(x => Convert(x)).ToList();
            result.Reverse();
            return await System.Threading.Tasks.Task.FromResult(result);
        }

        private static LogApplicationErrorViewModel Convert(LogApplicationError r)
        {
            var item = new LogApplicationErrorViewModel(r);
            return item;
        }

        public LogApplicationErrorViewModel ReloadItem(LogApplicationErrorViewModel item)
        {
            var x = repoLog.Reload(item.LogApplicationErrorId);
            if (x == null)
            {
                return null;
            }
            return Convert(x);
        }

        /*public void DeleteSelected()
        {
            var itemsToDelete = DbContext.DbSetLogApplication.Where(x => x.IsEnable == true);
            DbContext.DbSetLogApplication.RemoveRange(itemsToDelete);
            DbContext.SaveChanges();
        }*/

        public void Delete(LogApplicationErrorViewModel item)
        {
            var x = repoLog.FindById(item.LogApplicationErrorId);
            repoLog.Remove(x);
        }

        public LogApplicationErrorViewModel Create(LogApplicationErrorViewModel item, string msg, string stackTrace, string? innerEx, DateTime date)
        {
            var version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            item = new LogApplicationErrorViewModel
            {
                InsertDate = date,
                ErrorMessage = msg,
                ErrorContext = stackTrace,
                ErrorInnerException = innerEx
            };
            Console.WriteLine($"\n{msg}   {stackTrace}  {date} {innerEx}");
            var newItem = repoLog.Create(item.Item);
            return Convert(newItem);
        }

        public LogApplicationErrorViewModel Update(LogApplicationErrorViewModel item)
        {
            var x = repoLog.FindById(item.LogApplicationErrorId);
            x.ErrorMessage = item.ErrorMessage;
            x.ErrorContext = item.ErrorContext;
            x.InsertDate = item.InsertDate;
            x.ErrorInnerException = item.ErrorInnerException;
            /*x.IsEnable = item.IsEnable;*/
            return Convert(repoLog.Update(x));
        }

        public void DeleteAllLogs()
        {
            DbContext.DbSetLogApplication.RemoveRange(DbContext.DbSetLogApplication);
            DbContext.SaveChanges();
        }

        public List<LogApplicationErrorViewModel> Filtering(DateTime? y)
        {
            var filteredListLogs = repoLog.GetQuery().Where(x => x.InsertDate.Date == y.GetValueOrDefault().Date).ToList();
            var result = filteredListLogs.Select(Convert).ToList();
            result.Reverse();
            return result;
        }
        public List<LogApplicationErrorViewModel> FilteringError(string message)
        {
            var filteredListLogs = repoLog.GetQuery().Where(x => (x.ErrorMessage.StartsWith(message) || x.ErrorContext.StartsWith(message))).ToList();
            var result = filteredListLogs.Select(Convert).ToList();
            result.Reverse();
            return result;
        }
    }
}
