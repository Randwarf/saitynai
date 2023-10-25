using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Data.Entities
{
    public class Building : IUserOwnedResource
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Level { get; set; }

        [Required]
        public int SaveId { get; set; }
        [Required]
        public Save Save { get; set; }

        public string OwnerId { get; set; }
        public GameUser Owner { get; set; }
    }
}
