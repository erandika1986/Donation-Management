using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum FileType
    {
        [Description("Unknown or unsupported file type")]
        Unknown = 0,

        [Description("PDF document")]
        Pdf = 1,

        [Description("JPEG image")]
        Jpg = 2,

        [Description("PNG image")]
        Png = 3,

        [Description("Plain text file")]
        Txt = 4,

        [Description("Microsoft Word document")]
        Doc = 5,

        [Description("Microsoft Word Open XML document")]
        Docx = 6,

        [Description("Microsoft Excel spreadsheet")]
        Xls = 7,

        [Description("Microsoft Excel Open XML spreadsheet")]
        Xlsx = 8,

        [Description("Microsoft PowerPoint presentation")]
        Ppt = 9,

        [Description("Microsoft PowerPoint Open XML presentation")]
        Pptx = 10,

        [Description("Comma-separated values file")]
        Csv = 11,

        [Description("MP3 audio file")]
        Mp3 = 12,

        [Description("MP4 video file")]
        Mp4 = 13,

        [Description("ZIP archive")]
        Zip = 14,

        [Description("RAR archive")]
        Rar = 15,

        [Description("GIF image")]
        Gif = 16,

        [Description("HTML document")]
        Html = 17,

        [Description("XML document")]
        Xml = 18,

        [Description("JSON file")]
        Json = 19,

        [Description("Bitmap image")]
        Bmp = 20,

        [Description("Scalable Vector Graphics")]
        Svg = 21,

        [Description("Executable file")]
        Exe = 22,

        [Description("7-Zip archive")]
        SevenZip = 23

    }
}
