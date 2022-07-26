namespace ApiEcommerce.Dtos.Product
{
    public class CreateProductDto
    {
        public string Productid { get; set; }
        public string Productname { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public double? Discount { get; set; }
        public int? Category { get; set; }
        public string Image { get; set; }
    }
}
