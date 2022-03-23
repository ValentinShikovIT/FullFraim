using FullFraim.Models.Dto_s.Pagination;
using FullFraim.Models.Dto_s.Users;
using FullFraim.Models.ViewModels.Dashboard;
using System.Collections.Generic;

namespace FullFraim.Models.ViewModels.User
{
    public class UsersPageViewModel
    {
        public string Sorting { get; set; } = string.Empty;
        public PaginatedModel<PhotoJunkyDto> PaginatedModel { get; set; }
        public List<RankAndPointsViewModel> Model { get; set; }
        public PaginationFilter PageFilter { get; set; }
    }
}
