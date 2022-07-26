namespace ApiEcommerce.Dtos.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool? Isactive { get; set; }
        public string Description { get; set; }
    }
}
