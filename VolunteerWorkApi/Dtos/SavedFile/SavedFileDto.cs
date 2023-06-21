namespace VolunteerWorkApi.Dtos.SavedFile
{
    public record SavedFileDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string FileKey { get; set; }

        public string OriginalFileName { get; set; }

        public long FileSize { get; set; }

        public string FileExtension { get; set; }
    }
}
