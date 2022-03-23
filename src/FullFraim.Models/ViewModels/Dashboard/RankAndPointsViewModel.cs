using FullFraim.Models.Dto_s.Pagination;

namespace FullFraim.Models.ViewModels.Dashboard
{
    public class RankAndPointsViewModel
    {
        public string FullUserName { get; set; }
        public string PointsTillNextRank { get; set; }
        public string CurrentPoints { get; set; }
        public string Rank { get; set; }
        public PaginationFilter Filter { get; set; }
    }
}
