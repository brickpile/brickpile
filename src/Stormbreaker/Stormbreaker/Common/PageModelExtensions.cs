/* Copyright (C) 2011 by Marcus Lindblom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System;
using System.Globalization;
using System.Text;
using Stormbreaker.Models;

namespace Stormbreaker.Common {
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
            return NormalizeSlug(pageModel.Metadata.Name);
        }
        /// <summary>
        /// Removes all unsafe characters from the page name and returns a lower case slug
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string NormalizeSlug(string text) {
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
        private static void RemoveDiacritics(StringBuilder input) {
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