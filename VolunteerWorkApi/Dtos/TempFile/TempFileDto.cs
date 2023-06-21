namespace VolunteerWorkApi.Dtos.TempFile
{
    public class TempFileDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string FileKey { get; set; }

        public string OriginalFileName { get; set; }
    }
}
