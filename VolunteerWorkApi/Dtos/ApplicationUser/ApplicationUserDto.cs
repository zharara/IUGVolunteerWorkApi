﻿using VolunteerWorkApi.Dtos.SavedFile;

namespace VolunteerWorkApi.Dtos.ApplicationUser
{
    public record ApplicationUserDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UserName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; private set; }

        public SavedFileDto? ProfilePicture { get; set; }

        public string? FCMToken { get; set; }
    }
}
