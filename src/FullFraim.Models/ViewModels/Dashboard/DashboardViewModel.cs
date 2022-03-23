using FullFraim.Models.Dto_s.Phases;
using System;

namespace FullFraim.Models.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int ContestId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Cover_Url { get; set; }
        public string Description { get; set; }
        public PhaseDto ActivePhase { get; set; }
        public int ContestCategory { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrentUserParticipant { get; set; }
        public bool IsCurrentUserJury { get; set; }
        public bool HasCurrentUserSybmittedPhoto { get; set; }
    }
}
