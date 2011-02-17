using System;

namespace Stormbreaker.Models {
    public interface IImageModel : IDocumentModel {
        string OwnerUserId { get; set; }
        DateTime DateUploaded { get; set; }
        string Title { get; set; }
        string Filename { get; set; }        
    }
}