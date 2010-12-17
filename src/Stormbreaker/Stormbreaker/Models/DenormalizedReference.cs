namespace Stormbreaker.Models {
    public class DenormalizedReference<T> where T : IDocument
    {
        public string Id { get; set; }
        public string Slug { get; set; }
        public static implicit operator DenormalizedReference<T>(T document) {
            return new DenormalizedReference<T>
                       {
                           Id = document.Id,
                           Slug = document.Slug
                       };
        }
    }
}