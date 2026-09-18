using System.ComponentModel.DataAnnotations;

namespace findexit_test.Models.Dto;

public class UserRegisterDto
{
    [Required] public string FIO { get; set; }
    [Required] public string Position { get; set; }
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
}