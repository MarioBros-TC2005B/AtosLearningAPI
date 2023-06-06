namespace AtosLearningAPI.Model;

public class Exam
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string SubjectId { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string ImageUrl { get; set; }
}