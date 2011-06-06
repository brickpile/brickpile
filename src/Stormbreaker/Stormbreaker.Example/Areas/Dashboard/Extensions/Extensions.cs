namespace Stormbreaker.Dashboard.Extensions {
    public static class Extensions {
        public static string FormatFileSize(this long fileSize) {
            string[] suffix = { "bytes", "KB", "MB", "GB" };
            long j = 0;

            while (fileSize > 1024 && j < 4) {
                fileSize = fileSize / 1024;
                j++;
            }
            return (fileSize + " " + suffix[j]);
        }
    }
}