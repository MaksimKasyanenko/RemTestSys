using RemTestSys.Domain.Models;

public class TestViewModel{
    public int Id{get;set;}
    public string Name{get;set;}
    public string Description{get;set;}
    public int Duration{get;set;}
    public Test.MapPart[] MapParts { get; set; }
}