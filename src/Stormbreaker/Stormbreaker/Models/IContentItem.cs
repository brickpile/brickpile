namespace Stormbreaker.Models {
    /// <summary>
    /// The IContentItem interface 
    /// </summary>
    public interface IContentItem {
        string Id { get; set; }
        IMetaData MetaData { get; }
        IStructureInfo StructureInfo { get; }
        string Url { get; }
    }
}