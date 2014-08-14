namespace BrickPile.Core.Extensions
{
    /// <summary>
    ///     Provides BrickPile <see cref="IPage" /> helper methods.
    /// </summary>
    public static class PageExtensions
    {
        /// <summary>
        ///     Determines whether the specified page is draft.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static bool IsDraft(this IPage page)
        {
            return page.Id.Contains("draft");
        }

        public static string ContentId(this IPage page)
        {
            return page.Id + "/content";
        }
    }
}