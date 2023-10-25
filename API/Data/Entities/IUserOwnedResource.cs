using System.ComponentModel.DataAnnotations;

namespace API.Data.Entities
{
    public interface IUserOwnedResource
    {
        [Required]
        public string OwnerId { get; }
    }
}