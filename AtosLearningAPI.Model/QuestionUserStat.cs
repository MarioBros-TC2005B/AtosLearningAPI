namespace AtosLearningAPI.Model;

public class QuestionUserStat
{
    public int QuestionId { get; set; }
    public int ExamId { get; set; }
    public string Question { get; set; }
    public int Correct { get; set; }
}