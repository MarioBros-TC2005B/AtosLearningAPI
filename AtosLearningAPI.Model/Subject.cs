using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AtosLearningAPI.Model;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CourseId { get; set; }
}