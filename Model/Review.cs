namespace DoggoDex.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }

        public int DogId { get; set; }
        public Dog Dog { get; set; }

        public int BusinessOwnerId { get; set; }
        public BusinessOwner BusinessOwner { get; set; }
    }
}