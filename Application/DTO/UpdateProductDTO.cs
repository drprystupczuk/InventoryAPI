﻿namespace InventoryAPI.Application.DTO
{
    public class UpdateProductDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public required string Category { get; set; }
    }
}
