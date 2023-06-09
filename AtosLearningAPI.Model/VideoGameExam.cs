namespace AtosLearningAPI.Model;

public class VideoGameExam
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string SubjectName { get; set; }
    public string TeacherName { get; set; }
    public string ImageUrl { get; set; }
    public int QuestionsCount { get; set; }
    public int? Score { get; set; }
    public DateTime? EndDateTime { get; set; }
    
}