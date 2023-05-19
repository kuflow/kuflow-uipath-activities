using MimeDetective;
using System.IO;
using System.Linq;

namespace KuFlow.UiPathLibrary.Utils
{
    public static class FiletUtils
    {
        private static readonly ContentInspector SharedContentInspector = new ContentInspectorBuilder()
        {
            Definitions = MimeDetective.Definitions.Default.All()
        }.Build();

        public static string GuessMimeTypeFromPath(string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                return GuessMimeTypeFromStream(fileStream);
            }
        }

        public static string GuessMimeTypeFromStream(Stream stream)
        {

            var byMimes = SharedContentInspector.Inspect(stream).ByMimeType();
            if (!byMimes.IsDefaultOrEmpty)
            {
                return byMimes.FirstOrDefault()?.MimeType;
            }

            var byExtensions = SharedContentInspector.Inspect(stream).ByFileExtension();
            if (!byExtensions.IsDefaultOrEmpty)
            {
                return byExtensions.FirstOrDefault()?.Extension;
            }

            return "application/octet-stream";
        }
    }
}
