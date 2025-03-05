using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Context
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public Category? Category { get; set; }
    }
}
