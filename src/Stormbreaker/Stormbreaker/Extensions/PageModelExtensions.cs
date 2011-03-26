using System;
using System.Globalization;
using System.Text;
using Stormbreaker.Models;

namespace Stormbreaker.Extensions {
    /// <summary>
    /// Extension methods for PageModel objects
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class PageModelExtensions {
        /// <summary>
        /// Returns the controller name associated with this model
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public static string GetControllerName(this IPageModel pageModel) {
            return pageModel.GetType().Name.ToLower();
        }
        /// <summary>
        /// Generates and returns a friendly slug associated with this model
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public static string GenerateSlug(this IPageModel pageModel) {
            return NormalizeSlug(pageModel.MetaData.Name);
        }
        /// <summary>
        /// Removes all unsafe characters from the page name and returns a lower case slug
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string NormalizeSlug(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var sb = new StringBuilder(text);

            sb.Replace(":", "-");
            sb.Replace("/", "-");
            sb.Replace("?", "-");
            sb.Replace("#", "-");
            sb.Replace("[", "-");
            sb.Replace("]", "-");
            sb.Replace("@", "-");
            sb.Replace(",", "-");
            sb.Replace(".", "-");
            sb.Replace("\"", "-");
            sb.Replace("&", "-");
            sb.Replace("'", "-");
            sb.Replace(" ", "-");
            sb.Replace("(", "-");
            sb.Replace(")", "-");

            RemoveDiacritics(sb);

            while (sb.ToString().IndexOf("--", StringComparison.OrdinalIgnoreCase) > -1)
                sb.Replace("--", "-");

            if (sb[0] == '-')
                sb.Remove(0, 1);

            if (sb[sb.Length - 1] == '-')
                sb.Remove(sb.Length - 1, 1);

            return sb.ToString().ToLower();
        }
        /// <summary>
        /// Removes all diacritic characters as a step in the slug normalization
        /// </summary>
        /// <param name="input"></param>
        private static void RemoveDiacritics(StringBuilder input)
        {
            var normalized = input.ToString().Normalize(NormalizationForm.FormD);
            input.Remove(0, input.Length);

            for (var i = 0; i < normalized.Length; i++)
            {
                var c = normalized[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    input.Append(c);
            }
        }
    }
}