namespace Stormbreaker.Dashboard.Models {
    public class AssetModel {
        public string Name { get; set; }
        public string Path { get; set; }
        public long FileSize { get; set; }
        public string LastModified { get; set; }
        public string @Class { get; set; }
    }
}