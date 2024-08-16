using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
namespace SimpleTrader.WPF.ViewModels
{
    public class MajorIndexViewModel
    {
        private readonly IMajorIndexService _majorIndexService;
        public MajorIndex DowJones { get; set; }
        public MajorIndex Nasdaq { get; set; }
        public MajorIndex SP500 { get; set; }
        public MajorIndexViewModel(IMajorIndexService majorIndexService)
        {
            _majorIndexService = majorIndexService;
        }
        public static MajorIndexViewModel LoadMajorIndexViewModel(IMajorIndexService majorIndexService)
        {
            MajorIndexViewModel majorIndexViewModel = new MajorIndexViewModel(majorIndexService);
            return majorIndexViewModel;
        }
    }
}
