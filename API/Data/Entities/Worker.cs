using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Data.Entities
{
    public class Worker : IUserOwnedResource
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Level { get; set; }

        [Required]
        public int BuildingId { get; set; }
        public Building Building { get; set; }

        public string OwnerId { get; set; }
        public GameUser Owner { get; set; }
    }
}
