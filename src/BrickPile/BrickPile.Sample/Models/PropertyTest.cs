using System;
using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Domain.Models;

namespace BrickPile.Sample.Models {
    [PageType]
    public class PropertyTest : PageModel {
        
        public bool Bool { get; set; }

        public bool? NullableBool { get; set; }

        public DateTime DateTime { get; set; }

        public DateTime? NullableDateTime { get; set; }

        public int Int { get; set; }

        public int? NullableInt { get; set; }

        public decimal Decimal { get; set; }

        public double Double { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }



    }
}