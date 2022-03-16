namespace RemTestSys.Domain.ViewModels;

public class AccessToExamViewModel
{
    public int Id{get;set;}
    public string ExamName{get;set;}
    public string TargetName{get;set;}
    public AccessTypes AccessType{get;set;}

    public enum AccessTypes{ForAll, ForGroup, ForPerson}
}