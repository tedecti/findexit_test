using System.ComponentModel.DataAnnotations;

namespace findexit_test.Models;

public class Task
{
    [Key] public int Id { get; set; }
    [Required] public string Title { get; set; }
    public string? Details { get; set; }
    public float? Percent { get; set; }

    public User User { get; set; }
    public int UserId { get; set; }

    [DataType(DataType.DateTime)] public DateTime CreatedAt { get; set; }
    [DataType(DataType.DateTime)] public DateTime ExpiredAt { get; set; }
}