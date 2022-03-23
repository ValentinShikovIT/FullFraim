using FullFraim.Data.Base;

namespace FullFraim.Data.Models
{
    public class ParticipantContest : DeletableJunctionEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int ContestId { get; set; }
        public Contest Contest { get; set; }

        public int? PhotoId { get; set; }
        public Photo Photo { get; set; }
    }
}
