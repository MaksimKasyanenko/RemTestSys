using System;
using System.ComponentModel.DataAnnotations;

namespace RemTestSys.Domain.ViewModels{
    public class StudentViewModel{
        public int Id{get;set;}
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public int GroupId { get; set; }
        public string GroupName{get;set;}
        public DateTime RegistrationDate{get;set;}
        public string FullName
        {
            get => $"{FirstName} {LastName}";
        }
    }
}