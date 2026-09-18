using System.ComponentModel.DataAnnotations;

namespace findexit_test.Models;

public class User
{
    [Key] public int Id { get; set; }
    [Required] public string FIO { get; set; }
    [Required] public string Position { get; set; }
    [Required] public string Login { get; set; }
    [Required] public string PasswordHash { get; set; }
}