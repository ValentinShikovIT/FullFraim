using System;

namespace FullFraim.Models.Dto_s.Phases
{
    public class PhaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive
        {
            get
            {
                if (DateTime.UtcNow > StartDate
                && DateTime.UtcNow < EndDate)
                {
                    return true;
                }

                return false;
            }
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
