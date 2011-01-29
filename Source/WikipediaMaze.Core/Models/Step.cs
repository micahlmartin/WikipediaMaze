namespace WikipediaMaze.Core
{
    public class Step
    {
        public virtual int Id { get; private set; }
        public virtual string Topic { get; set; }
        public virtual int StepNumber { get; set; }
        public virtual int SolutionId { get; set; }
    }
}