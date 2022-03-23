using FullFraim.Models.Dto_s.Pagination;

namespace FullFraim.Models.ViewModels.Dashboard
{
    public class DashboardPhasesPaginatedViewModel
    {
        public PaginatedModel<DashboardViewModel> PhaseOne { get; set; }
        public PaginatedModel<DashboardViewModel> PhaseTwo { get; set; }
        public PaginatedModel<DashboardViewModel> Finished { get; set; }
    }
}
