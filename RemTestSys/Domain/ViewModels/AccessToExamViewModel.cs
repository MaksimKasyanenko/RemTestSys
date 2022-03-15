namespace RemTestSys.Domain.ViewModels;

public class AccessToExamViewModel
{
    public int Id{get;set;}
    public string ExamName{get;set;}
    public string TargetName{get;set;}
    public bool ForAll{get;set;}
    public bool ForGroup{get;set;}
    public bool ForPerson{get;set;}
}