namespace AtosLearningAPI.Model;

public class Question
{
    public int Id { get; set; }
    public int ExamId { get; set; }
    public string Title { get; set; }
    public int TimeLimit { get; set; }
    public Answer[] Answers { get; set; }
}