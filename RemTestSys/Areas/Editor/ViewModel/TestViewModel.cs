public class TestViewModel{
    public int Id{get;set;}
    public string Name{get;set;}
    public string Description{get;set;}
    public int Duration{get;set;}
    public int[] QuestionCounts{get;set;}
    public double[] QuestionCosts{get;set;}
    public Test.MapPart[] GetMapParts(){
        if(QuestionCosts.Length != QuestionCounts.Length)throw new InvalidOperationException("Counts and Costs of questions must be equal");
        Test.MapParts[] res = new Test.MapPart[QuestionCosts.Length];
        for(int i=0; i<res.Length; i++){
            res[i]=new Test.MapPart{
                QuestionCount=QuestionCounts[i],
                QuestionCast=QuestionCosts[i]
            };
        }
        return res;
    }
}