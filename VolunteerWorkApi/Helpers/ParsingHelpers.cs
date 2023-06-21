using System.Globalization;

namespace VolunteerWorkApi.Helpers
{
    public static class ParsingHelpers
    {
        public static long? ParseUserId(string? claimIdString)
        {
           bool parsed = int.TryParse(
                   claimIdString,
                   NumberStyles.Integer, null, out int userId);

            return parsed ? userId : null;
        }
    }
}
