namespace Shared.AllConstants
{
    public static class ErrorMessages
    {
        /// <summary>
        /// {0} user id, {1} contest id
        /// </summary>
        public static string AlreadyInContest = $"User with id: {0} already in contest with id: {1}!";

        public static string ReviewOutsidePhaseTwo = $"Review option not available outside Phase Two.";
        public static string ReviewAlreadyGiven = $"The photo has already been reviewed.";

        public static string NameMustBeUnique = $"Name must be unique.";
        public static string CannotSendBothUrlAndImage = $"Cannot send both url and image file.";
        public static string ContestCoverRequired = $"Contest cover is *Required";
        public static string CannotEnroll = $"You are not allowed to enroll!";

        public static string JuryCannotBeParticipant = "Jury can not be a participant.";
    }

    public static class ClientErrorMessages
    {
        public static string NotFound = "Page was not found! Please contact Administration or check the url for mistakes!";
        public static string Unauthorized = "You are not authorized to enter this page! Please re-sign with additional permissions";
        public static string ServerError = "Something went wrong on our side! We are deeply sorry!";
        public static string ServiceUnavailable = "Service Unavailable!";
        public static string InvalidInput = "Bad Request!";
    }
}
