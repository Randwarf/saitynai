using System.ComponentModel.DataAnnotations;

namespace API.Data.Dtos
{
    public record UserDTO(string Id, string UserName, string Email);
    public record RegisterUserDTO([Required] string UserName, [EmailAddress][Required] string Email, [Required] string Password);
    public record LoginUserDTO(string UserName, string Password);
    public record SuccessfulLoginDTO(string AccessToken);
}
