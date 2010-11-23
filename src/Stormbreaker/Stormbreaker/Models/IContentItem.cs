namespace Stormbreaker.Models {
    /// <summary>
    /// The IContentItem interface 
    /// </summary>
    public interface IContentItem {
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        string Id { get; }
        IMetaData MetaData { get; set; }
        IStructureInfo StructureInfo { get; set; }
        string Url { get; set; }
    }
}