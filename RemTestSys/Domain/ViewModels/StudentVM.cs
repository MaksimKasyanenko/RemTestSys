namespace RemTestSys.Domain.ViewModels{
    public class StudentVM{
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public int GroupId { get; set; }
        public string GroupName{get;set;}
        public DateTime RegistrationDate{get;set;}
    }
}