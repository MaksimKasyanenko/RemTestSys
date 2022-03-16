using System;

namespace RemTestSys.Domain.ViewModels;

public class SessionViewModel
{
    public int Id{get;set;}
    public string TestName{get;set;}
    public string StudentName{get;set;}
    public DateTime StartTime{get;set;}
    public bool Finished{get;set;}
}