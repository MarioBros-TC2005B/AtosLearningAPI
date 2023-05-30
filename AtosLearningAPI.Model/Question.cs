namespace AtosLearningAPI.Model;

public class Question
{
    public string Id { get; set; }
    public string ExamId { get; set; }
    public string Title { get; set; }
    public Answer[] Answers { get; set; }
}