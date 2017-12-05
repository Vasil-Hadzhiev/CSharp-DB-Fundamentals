namespace Products.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Category
    {

        public Category()
        {
            this.CategoryProducts = new HashSet<CategoryProduct>();
        }

        public int Id { get; set; }

        [MinLength(3)]
        public string Name { get; set; }

        public ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}