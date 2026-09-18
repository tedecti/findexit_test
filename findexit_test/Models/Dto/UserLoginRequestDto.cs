using System.ComponentModel.DataAnnotations;

namespace findexit_test.Models.Dto;

public class UserLoginRequestDto
{
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
}