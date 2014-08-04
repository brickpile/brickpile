using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace BrickPile.Core.Extensions
{
    public static class HtmlHelperGravatar
    {
        /// <summary>
        ///     Creates HTML for an <c>img</c> element that presents a Gravatar icon.
        /// </summary>
        /// <param name="html">The <see cref="HtmlHelper" /> upon which this extension method is provided.</param>
        /// <param name="email">The email address used to identify the icon.</param>
        /// <param name="size">An optional parameter that specifies the size of the square image in pixels.</param>
        /// <param name="rating">An optional parameter that specifies the safety level of allowed images.</param>
        /// <param name="defaultImage">
        ///     An optional parameter that controls what image is displayed for email addresses that don't
        ///     have associated Gravatar icons.
        /// </param>
        /// <param name="htmlAttributes">
        ///     An optional parameter holding additional attributes to be included on the <c>img</c>
        ///     element.
        /// </param>
        /// <returns>An HTML string of the <c>img</c> element that presents a Gravatar icon.</returns>
        public static string Gravatar(this HtmlHelper html,
            string email,
            int? size = null,
            GravatarRating rating = GravatarRating.Default,
            GravatarDefaultImage defaultImage = GravatarDefaultImage.MysteryMan,
            object htmlAttributes = null)
        {
            var url = new StringBuilder("http://www.gravatar.com/avatar/", 90);
            url.Append(GetEmailHash(email));

            var isFirst = true;
            Action<string, string> addParam = (p, v) =>
            {
                url.Append(isFirst ? '?' : '&');
                isFirst = false;
                url.Append(p);
                url.Append('=');
                url.Append(v);
            };

            if (size != null)
            {
                if (size < 1 || size > 512)
                    throw new ArgumentOutOfRangeException("size", size, "Must be null or between 1 and 512, inclusive.");
                addParam("s", size.Value.ToString());
            }

            if (rating != GravatarRating.Default)
                addParam("r", rating.ToString().ToLower());

            if (defaultImage != GravatarDefaultImage.Default)
            {
                if (defaultImage == GravatarDefaultImage.Http404)
                    addParam("d", "404");
                else if (defaultImage == GravatarDefaultImage.Identicon)
                    addParam("d", "identicon");
                if (defaultImage == GravatarDefaultImage.MonsterId)
                    addParam("d", "monsterid");
                if (defaultImage == GravatarDefaultImage.MysteryMan)
                    addParam("d", "mm");
                if (defaultImage == GravatarDefaultImage.Retro)
                    addParam("d", "retro");
                if (defaultImage == GravatarDefaultImage.Wavatar)
                    addParam("d", "wavatar");
            }

            var tag = new TagBuilder("img");
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tag.Attributes.Add("src", url.ToString());

            if (size != null)
            {
                tag.Attributes.Add("width", size.ToString());
                tag.Attributes.Add("height", size.ToString());
            }

            return tag.ToString();
        }

        private static string GetEmailHash(string email)
        {
            if (email == null)
                return new string('0', 32);

            email = email.Trim().ToLower();

            var emailBytes = Encoding.ASCII.GetBytes(email);
            var hashBytes = new MD5CryptoServiceProvider().ComputeHash(emailBytes);

            Debug.Assert(hashBytes.Length == 16);

            var hash = new StringBuilder();
            foreach (var b in hashBytes)
                hash.Append(b.ToString("x2"));
            return hash.ToString();
        }
    }

    public enum GravatarRating
    {
        /// <summary>
        ///     The default value as specified by the Gravatar service.  That is, no rating value is specified
        ///     with the request.  At the time of authoring, the default level was <see cref="G" />.
        /// </summary>
        Default,

        /// <summary>
        ///     Suitable for display on all websites with any audience type.  This is the default.
        /// </summary>
        G,

        /// <summary>
        ///     May contain rude gestures, provocatively dressed individuals, the lesser swear words, or mild violence.
        /// </summary>
        Pg,

        /// <summary>
        ///     May contain such things as harsh profanity, intense violence, nudity, or hard drug use.
        /// </summary>
        R,

        /// <summary>
        ///     May contain hardcore sexual imagery or extremely disturbing violence.
        /// </summary>
        X
    }

    public enum GravatarDefaultImage
    {
        /// <summary>
        ///     The default value image.  That is, the image returned when no specific default value is included
        ///     with the request.  At the time of authoring, this image is the Gravatar icon.
        /// </summary>
        Default,

        /// <summary>
        ///     Do not load any image if none is associated with the email hash, instead return an HTTP 404 (File Not Found)
        ///     response.
        /// </summary>
        Http404,

        /// <summary>
        ///     A simple, cartoon-style silhouetted outline of a person (does not vary by email hash).
        /// </summary>
        MysteryMan,

        /// <summary>
        ///     A geometric pattern based on an email hash.
        /// </summary>
        Identicon,

        /// <summary>
        ///     A generated 'monster' with different colors, faces, etc.
        /// </summary>
        MonsterId,

        /// <summary>
        ///     An awesome generated, 8-bit arcade-style pixelated faces
        /// </summary>
        Retro,

        /// <summary>
        ///     Generated faces with differing features and backgrounds.
        /// </summary>
        Wavatar
    }
}