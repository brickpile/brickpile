using System;
using System.Globalization;
using System.Text;

namespace BrickPile.Core
{
    public class Slug
    {
        /// <summary>
        /// Creates the slug.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static string CreateSlug(IPage page)
        {
            return NormalizeUrlSegment(page.Metadata.Slug ?? page.Metadata.Name ?? page.Id);
        }

        /// <summary>
        /// Normalizes the URL segment.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private static string NormalizeUrlSegment(string text)
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
        /// Removes the diacritics.
        /// </summary>
        /// <param name="input">The input.</param>
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
