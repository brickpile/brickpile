using System;
using Stormbreaker.RavenDBMembership;

namespace Stormbreaker.Models {
    public class Image : IImageModel {
        public string Id { get; set; }
        public string OwnerUserId { get; set; }
        public DateTime DateUploaded { get; set; }
        public string Title { get; set; }
        public string Filename { get; set; }
        public Image(User owner, string title, string filename) {
            Id = "raven/library/images/"; // db assigns id
            OwnerUserId = owner.Id;
            DateUploaded = DateTime.Now;
            Title = title;
            Filename = filename;
        }
    }
}