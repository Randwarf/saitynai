﻿namespace API.Data.Entities
{
    public class Building
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }

        public Save Save { get; set; }
    }
}
