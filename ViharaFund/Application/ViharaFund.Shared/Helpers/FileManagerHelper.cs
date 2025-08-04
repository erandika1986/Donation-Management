using System.Reflection;
using ViharaFund.Domain.Enums;

namespace ViharaFund.Shared.Helpers
{
    public static class FileManagerHelper
    {
        /// <summary>
        /// GetFileType
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileType GetFileType(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                // Extract the file extension and remove the leading dot
                string extension = Path.GetExtension(fileName)?.TrimStart('.').ToLower();

                // Match the extension to the corresponding FileType enum value
                return extension switch
                {
                    "pdf" => FileType.Pdf,
                    "jpg" => FileType.Jpg,
                    "jpeg" => FileType.Jpg, // Consider both jpg and jpeg as Jpg
                    "png" => FileType.Png,
                    "txt" => FileType.Txt,
                    "doc" => FileType.Doc,
                    "docx" => FileType.Docx,
                    "xls" => FileType.Xls,
                    "xlsx" => FileType.Xlsx,
                    "ppt" => FileType.Ppt,
                    "pptx" => FileType.Pptx,
                    "csv" => FileType.Csv,
                    "mp3" => FileType.Mp3,
                    "mp4" => FileType.Mp4,
                    "zip" => FileType.Zip,
                    "rar" => FileType.Rar,
                    "gif" => FileType.Gif,
                    "html" => FileType.Html,
                    "xml" => FileType.Xml,
                    "json" => FileType.Json,
                    "bmp" => FileType.Bmp,
                    "svg" => FileType.Svg,
                    "exe" => FileType.Exe,
                    "7z" => FileType.SevenZip,
                    _ => FileType.Unknown // Default to Unknown for unrecognized extensions
                };
            }
            else
            {
                return FileType.Unknown;
            }


        }

        /// <summary>
        /// GetFileSizeInMegaBytes
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetFileSizeInMegaBytes(MemoryStream memoryStream)
        {
            if (memoryStream is null)
            {
                throw new ArgumentNullException(nameof(memoryStream), "MemoryStream cannot be null.");
            }

            var clonedStream = new MemoryStream(memoryStream.ToArray());

            var sizeInBytes = clonedStream.Length;

            var sizeInMB = Math.Round(sizeInBytes / (1024.0 * 1024.0), 2);

            return $"{sizeInMB} MB";
        }

        /// <summary>
        /// GetFullPathRelativeToEntryAssembly
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns>string</returns>

        public static string GetFullPathRelativeToEntryAssembly(string folderPath)
        {
            var outputDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

            if (string.IsNullOrEmpty(outputDirectory))
            {
                return string.Empty;

            }

            return Path.Combine(outputDirectory, folderPath);
        }
    }
}
