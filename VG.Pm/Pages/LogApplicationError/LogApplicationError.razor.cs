using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using VG.Pm.Data.Services;
using VG.Pm.Data.ViewModel;

namespace VG.Pm.Pages.LogApplicationError
{
    public class LogApplicationErrorView : ComponentBase
    {
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] private LogApplicationErrorService LogService { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] protected IJSRuntime jsruntime { get; set; }

        protected List<LogApplicationErrorViewModel> Model { get; set; }

        public LogApplicationErrorViewModel Log = new LogApplicationErrorViewModel();

        public bool flag;
        public LogApplicationErrorViewModel mCurrentItem;
        public DateTime mFilterValue;
        public string mFilterError = "";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                FilterValue = DateTime.Now;
                Model = await LogService.GetAll();
                //Model.Reverse();
                await InvokeAsync(StateHasChanged);
            }
        }

        public DateTime FilterValue
        {
            get => mFilterValue;

            set
            {
                mFilterValue = value;
                Filter();
            }
        }
        public string FilterError
        {
            get => mFilterError;

            set
            {
                mFilterError = value;
                FiltersError();
            }
        }

        public void Error(LogApplicationErrorViewModel item)
        {
            try
            {
                try
                {
                    throw new ArgumentException();
                }

                catch (ArgumentException e)
                {
                    //make sure this path does not exist
                    if (File.Exists("file://Bigsky//log.txt%22)%20==%20false") == false)
                    {
                        throw new FileNotFoundException("File Not found when trying to write argument exception to the file", e);
                    }
                }
            }

            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    LogService.Create(Log, e.Message, e.StackTrace, e.InnerException.StackTrace, DateTime.Now);
                }
            }
            /*try
            {
                try
                {
                    throw new ArgumentOutOfRangeException(nameof(Model), "Error");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    throw new ArgumentOutOfRangeException(nameof(Model), "End");

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    LogService.Create(log, ex.Message, ex.StackTrace, ex.InnerException.StackTrace, DateTime.Now);
                }
                else
                {
                    LogService.Create(log, ex.Message, ex.StackTrace, ex.Message, DateTime.Now);
                }
            }*/
        }
        protected void Filter()
        {
            Model = LogService.Filtering(mFilterValue);
            StateHasChanged();
        }
        protected void FiltersError()
        {
            Model = LogService.FilteringError(mFilterError);
            StateHasChanged();
        }
    }
}
