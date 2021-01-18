using System;

namespace Mine.Models
{
    public class ItemModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

        // The Value of the Item +9 Damange
        public int Value { get; set; } = 0;
    }
}