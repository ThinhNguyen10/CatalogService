namespace ApiEcommerce.Dtos.Product
{
    public class UpdateProductDto
    {
        public string Productname { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public double? Discount { get; set; }

        public string Image { get; set; }
    }
}
