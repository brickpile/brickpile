using System;
using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Samples.Controllers;

namespace BrickPile.Samples.Models {
    [PageType(Name = "Container", ControllerType = typeof(ContainerController))]
    public class Container : Page {

        public string Heading { get; set; }

        //[UIHint("Markdown")]
        public string MainBody { get; set; }

        //[Required]
        //public PageReference ContainerPage { get; set; }

        [Display(Name = "Person")]
        public Person Person { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: yyyy-MM-dd}")]
        public DateTime IssueDate { get; set; }

        public Container() {
            this.Person = new Person();
        }
    }
    public class ContainerViewModel {
        public Container CurrentPage { get; set; }
    }

    public class Person {
        
        public string Firstname { get; set; }
        
        public string Lastname { get; set; }
    }
}