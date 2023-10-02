namespace API.Data.Entities
{
    public class Worker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }

        public Building Building { get; set; }
    }
}
