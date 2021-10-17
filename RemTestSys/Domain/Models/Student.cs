using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RemTestSys.Domain.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string LogId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName { get { return $"{this.FirstName} {this.LastName}"; } }
        public Group Group { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
