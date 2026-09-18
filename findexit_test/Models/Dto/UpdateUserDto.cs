using System.ComponentModel.DataAnnotations;

namespace findexit_test.Models.Dto;

public class UpdateUserDto
{
    [Required] public string FIO { get; set; }
    [Required] public string Position { get; set; }

}