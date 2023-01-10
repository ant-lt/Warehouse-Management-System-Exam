namespace WMS.Domain.Models.DTO
{
    public class GetProductDto
    {

        public int Id { get; set; }

        public string SKU { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Volume { get; set; }

        public decimal Weigth { get; set; }
    }
}
