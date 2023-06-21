using VolunteerWorkApi.Dtos.VolunteerProgramTask;
using VolunteerWorkApi.Dtos.VolunteerStudent;

namespace VolunteerWorkApi.Dtos.VolunteerStudentTaskAccomplish
{
    public record VolunteerStudentTaskAccomplishDto
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsAccomplished { get; set; }

        public double Rate { get; set; }

        public long? VolunteerStudentId { get; set; }

        public VolunteerStudentDto VolunteerStudent { get; set; }

        public long VolunteerProgramTaskId { get; set; }

        public VolunteerProgramTaskDto VolunteerProgramTask { get; set; }
    }
}
