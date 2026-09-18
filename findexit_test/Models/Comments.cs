using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace findexit_test.Models;

public class Comments
{
    [Key] public int Id { get; set; }
    [Required] public string Text { get; set; }

    public Task Task { get; set; }
    public int TaskId { get; set; }

    public User User { get; set; }
    public int UserId { get; set; }

    [DataType(DataType.DateTime)] public DateTime CreatedAt { get; set; }
}