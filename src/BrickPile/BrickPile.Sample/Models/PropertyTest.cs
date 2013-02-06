using System;
using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.UI.Web.ViewModels;

namespace BrickPile.Sample.Models {
    [ContentType]
    public class PropertyTest : IContent {

        [ScaffoldColumn(false)]
        public string Id { get; set; }

        public bool? NullableBool { get; set; }

        public DateTime? NullableDateTime { get; set; }

        public int? NullableInt { get; set; }

        public decimal? Decimal { get; set; }

        public double? Double { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}