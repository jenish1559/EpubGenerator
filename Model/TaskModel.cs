using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace EPubGenerator.Model
{
    [XmlRoot(ElementName = "hfTask", Namespace = "http://www.abbyy.com/HotFolder/Engine/TaskImpl")]
    public class HfTask
    {
        [XmlElement(ElementName = "engineTask", Namespace = "http://www.abbyy.com/HotFolder/Engine/Task")]
        public EngineTask EngineTask { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "sys", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Sys { get; set; }
        [XmlAttribute(AttributeName = "serializationVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string SerializationVersion { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "startType")]
        public string StartType { get; set; }
        [XmlAttribute(AttributeName = "startTime")]
        public string StartTime { get; set; }
        [XmlAttribute(AttributeName = "daysToStart")]
        public string DaysToStart { get; set; }
        [XmlAttribute(AttributeName = "dayOfMonthNumber")]
        public string DayOfMonthNumber { get; set; }
        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }
        [XmlAttribute(AttributeName = "lastRunStartTime")]
        public string LastRunStartTime { get; set; }
        [XmlAttribute(AttributeName = "lastRunProcessesImagesCount")]
        public string LastRunProcessesImagesCount { get; set; }
        [XmlAttribute(AttributeName = "lastRunFinalResult")]
        public string LastRunFinalResult { get; set; }
        [XmlAttribute(AttributeName = "dayNumber")]
        public string DayNumber { get; set; }
    }

    [XmlRoot(ElementName = "language", Namespace = "http://www.abbyy.com/FineReader/BatchRecognitionLanguage")]
    public class Language
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "languages")]
        public string Languages { get; set; }
    }

    [XmlRoot(ElementName = "ocr", Namespace = "http://www.abbyy.com/FineReader/OCROptions")]
    public class Ocr
    {
        [XmlElement(ElementName = "language", Namespace = "http://www.abbyy.com/FineReader/BatchRecognitionLanguage")]
        public Language Language { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "textType")]
        public string TextType { get; set; }
        [XmlAttribute(AttributeName = "hilightLevel")]
        public string HilightLevel { get; set; }
        [XmlAttribute(AttributeName = "patternName")]
        public string PatternName { get; set; }
        [XmlAttribute(AttributeName = "patternMode")]
        public string PatternMode { get; set; }
    }

    [XmlRoot(ElementName = "preprocess", Namespace = "http://www.abbyy.com/FineReader/ImagePreprocessOptions")]
    public class Preprocess
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
    }

    [XmlRoot(ElementName = "documentSynthesis", Namespace = "http://www.abbyy.com/FineReader/DocumentSynthesisOptions")]
    public class DocumentSynthesis
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "fonts")]
        public string Fonts { get; set; }
    }

    [XmlRoot(ElementName = "pictureOptions", Namespace = "http://www.abbyy.com/FineReader/ExportFormatPictureOptions")]
    public class PictureOptions
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "colourControlType")]
        public string ColourControlType { get; set; }
        [XmlAttribute(AttributeName = "pictureResolution")]
        public string PictureResolution { get; set; }
        [XmlAttribute(AttributeName = "jpegQuality")]
        public string JpegQuality { get; set; }
        [XmlAttribute(AttributeName = "allowLossQuality")]
        public string AllowLossQuality { get; set; }
        [XmlAttribute(AttributeName = "prohibitedColorFormats")]
        public string ProhibitedColorFormats { get; set; }
        [XmlAttribute(AttributeName = "prohibitedGrayFormats")]
        public string ProhibitedGrayFormats { get; set; }
        [XmlAttribute(AttributeName = "prohibitedBWFormats")]
        public string ProhibitedBWFormats { get; set; }
    }

    [XmlRoot(ElementName = "rtfOptions", Namespace = "http://www.abbyy.com/FineReader/RTFExportOptions")]
    public class RtfOptions
    {
        [XmlElement(ElementName = "pictureOptions", Namespace = "http://www.abbyy.com/FineReader/ExportFormatPictureOptions")]
        public PictureOptions PictureOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "errorTextColor")]
        public string ErrorTextColor { get; set; }
        [XmlAttribute(AttributeName = "errorBackgroundColor")]
        public string ErrorBackgroundColor { get; set; }
        [XmlAttribute(AttributeName = "hyperlinksColor")]
        public string HyperlinksColor { get; set; }
        [XmlAttribute(AttributeName = "paperSize")]
        public string PaperSize { get; set; }
        [XmlAttribute(AttributeName = "exportPageFormat")]
        public string ExportPageFormat { get; set; }
        [XmlAttribute(AttributeName = "paperMargins")]
        public string PaperMargins { get; set; }
        [XmlAttribute(AttributeName = "plainTextFont")]
        public string PlainTextFont { get; set; }
    }

    [XmlRoot(ElementName = "xlOptions", Namespace = "http://www.abbyy.com/FineReader/XLExportOptions")]
    public class XlOptions
    {
        [XmlElement(ElementName = "pictureOptions", Namespace = "http://www.abbyy.com/FineReader/ExportFormatPictureOptions")]
        public PictureOptions PictureOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "fileFormat")]
        public string FileFormat { get; set; }
        [XmlAttribute(AttributeName = "pageFormat")]
        public string PageFormat { get; set; }
    }

    [XmlRoot(ElementName = "securitySettings", Namespace = "http://www.abbyy.com/FineReader/PdfSecuritySettings")]
    public class SecuritySettings
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "setOwnerPassword")]
        public string SetOwnerPassword { get; set; }
        [XmlAttribute(AttributeName = "setUserPassword")]
        public string SetUserPassword { get; set; }
        [XmlAttribute(AttributeName = "ownerPassword")]
        public string OwnerPassword { get; set; }
        [XmlAttribute(AttributeName = "userPassword")]
        public string UserPassword { get; set; }
        [XmlAttribute(AttributeName = "isModifyContentsAllowed")]
        public string IsModifyContentsAllowed { get; set; }
        [XmlAttribute(AttributeName = "isFillFormAllowed")]
        public string IsFillFormAllowed { get; set; }
        [XmlAttribute(AttributeName = "isModifyInteractiveContentAllowed")]
        public string IsModifyInteractiveContentAllowed { get; set; }
        [XmlAttribute(AttributeName = "isAssembleDocumentAllowed")]
        public string IsAssembleDocumentAllowed { get; set; }
        [XmlAttribute(AttributeName = "allowInformationCopy")]
        public string AllowInformationCopy { get; set; }
        [XmlAttribute(AttributeName = "allowDisabilityAccess")]
        public string AllowDisabilityAccess { get; set; }
        [XmlAttribute(AttributeName = "rc4KeyLength")]
        public string Rc4KeyLength { get; set; }
        [XmlAttribute(AttributeName = "printSecuritySettings")]
        public string PrintSecuritySettings { get; set; }
        [XmlAttribute(AttributeName = "encryptionType")]
        public string EncryptionType { get; set; }
        [XmlAttribute(AttributeName = "documentEncryptionType")]
        public string DocumentEncryptionType { get; set; }
    }

    [XmlRoot(ElementName = "pdfOptions", Namespace = "http://www.abbyy.com/FineReader/PDFExportOptions")]
    public class PdfOptions
    {
        [XmlElement(ElementName = "pictureOptions", Namespace = "http://www.abbyy.com/FineReader/ExportFormatPictureOptions")]
        public PictureOptions PictureOptions { get; set; }
        [XmlElement(ElementName = "securitySettings", Namespace = "http://www.abbyy.com/FineReader/PdfSecuritySettings")]
        public SecuritySettings SecuritySettings { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "exportMode")]
        public string ExportMode { get; set; }
        [XmlAttribute(AttributeName = "fontMode")]
        public string FontMode { get; set; }
        [XmlAttribute(AttributeName = "hyperlinksColor")]
        public string HyperlinksColor { get; set; }
        [XmlAttribute(AttributeName = "paperSize")]
        public string PaperSize { get; set; }
        [XmlAttribute(AttributeName = "paperMargins")]
        public string PaperMargins { get; set; }
        [XmlAttribute(AttributeName = "outlineCreationMode")]
        public string OutlineCreationMode { get; set; }
        [XmlAttribute(AttributeName = "profile")]
        public string Profile { get; set; }
        [XmlAttribute(AttributeName = "pdfaVersion")]
        public string PdfaVersion { get; set; }
    }

    [XmlRoot(ElementName = "CodePageOptions", Namespace = "http://www.abbyy.com/FineReader/ExportOptionsCodePage")]
    public class CodePageOptions
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "encodeType")]
        public string EncodeType { get; set; }
        [XmlAttribute(AttributeName = "codePageId")]
        public string CodePageId { get; set; }
    }

    [XmlRoot(ElementName = "htmlOptions", Namespace = "http://www.abbyy.com/FineReader/HTMLExportOptions")]
    public class HtmlOptions
    {
        [XmlElement(ElementName = "CodePageOptions", Namespace = "http://www.abbyy.com/FineReader/ExportOptionsCodePage")]
        public CodePageOptions CodePageOptions { get; set; }
        [XmlElement(ElementName = "pictureOptions", Namespace = "http://www.abbyy.com/FineReader/ExportFormatPictureOptions")]
        public PictureOptions PictureOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "exportPageFormat")]
        public string ExportPageFormat { get; set; }
        [XmlAttribute(AttributeName = "partitionMode")]
        public string PartitionMode { get; set; }
    }

    [XmlRoot(ElementName = "pptOptions", Namespace = "http://www.abbyy.com/FineReader/PPTExportOptions")]
    public class PptOptions
    {
        [XmlElement(ElementName = "pictureOptions", Namespace = "http://www.abbyy.com/FineReader/ExportFormatPictureOptions")]
        public PictureOptions PictureOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
    }

    [XmlRoot(ElementName = "textOptions", Namespace = "http://www.abbyy.com/FineReader/TextExportOptions")]
    public class TextOptions
    {
        [XmlElement(ElementName = "CodePageOptions", Namespace = "http://www.abbyy.com/FineReader/ExportOptionsCodePage")]
        public CodePageOptions CodePageOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
    }

    [XmlRoot(ElementName = "csvOptions", Namespace = "http://www.abbyy.com/FineReader/CSVExportOptions")]
    public class CsvOptions
    {
        [XmlElement(ElementName = "CodePageOptions", Namespace = "http://www.abbyy.com/FineReader/ExportOptionsCodePage")]
        public CodePageOptions CodePageOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "tabSeparator")]
        public string TabSeparator { get; set; }
    }

    [XmlRoot(ElementName = "xmlOptions", Namespace = "http://www.abbyy.com/FineReader/XMLExportOptions")]
    public class XmlOptions
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
    }

    [XmlRoot(ElementName = "ebookOptions", Namespace = "http://www.abbyy.com/FineReader/EbookExportOptions")]
    public class EbookOptions
    {
        [XmlElement(ElementName = "pictureOptions", Namespace = "http://www.abbyy.com/FineReader/ExportFormatPictureOptions")]
        public PictureOptions PictureOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "fontFormat")]
        public string FontFormat { get; set; }
        [XmlAttribute(AttributeName = "saveFontSize")]
        public string SaveFontSize { get; set; }
        [XmlAttribute(AttributeName = "embedFonts")]
        public string EmbedFonts { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "epubVersion")]
        public string EpubVersion { get; set; }
    }

    [XmlRoot(ElementName = "DjVuOptions", Namespace = "http://www.abbyy.com/FineReader/DjVuExportOptions")]
    public class DjVuOptions
    {
        [XmlElement(ElementName = "pictureOptions", Namespace = "http://www.abbyy.com/FineReader/ExportFormatPictureOptions")]
        public PictureOptions PictureOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "mode")]
        public string Mode { get; set; }
        [XmlAttribute(AttributeName = "layerMode")]
        public string LayerMode { get; set; }
        [XmlAttribute(AttributeName = "textParticularity")]
        public string TextParticularity { get; set; }
    }

    [XmlRoot(ElementName = "xpsOptions", Namespace = "http://www.abbyy.com/FineReader/XPSExportOptions")]
    public class XpsOptions
    {
        [XmlElement(ElementName = "pictureOptions", Namespace = "http://www.abbyy.com/FineReader/ExportFormatPictureOptions")]
        public PictureOptions PictureOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "mode")]
        public string Mode { get; set; }
        [XmlAttribute(AttributeName = "pageSizeMode")]
        public string PageSizeMode { get; set; }
        [XmlAttribute(AttributeName = "paperSize")]
        public string PaperSize { get; set; }
    }

    [XmlRoot(ElementName = "export", Namespace = "http://www.abbyy.com/FineReader/ExportOptionsImpl")]
    public class Export
    {
        [XmlElement(ElementName = "rtfOptions", Namespace = "http://www.abbyy.com/FineReader/RTFExportOptions")]
        public RtfOptions RtfOptions { get; set; }
        [XmlElement(ElementName = "xlOptions", Namespace = "http://www.abbyy.com/FineReader/XLExportOptions")]
        public XlOptions XlOptions { get; set; }
        [XmlElement(ElementName = "pdfOptions", Namespace = "http://www.abbyy.com/FineReader/PDFExportOptions")]
        public PdfOptions PdfOptions { get; set; }
        [XmlElement(ElementName = "htmlOptions", Namespace = "http://www.abbyy.com/FineReader/HTMLExportOptions")]
        public HtmlOptions HtmlOptions { get; set; }
        [XmlElement(ElementName = "pptOptions", Namespace = "http://www.abbyy.com/FineReader/PPTExportOptions")]
        public PptOptions PptOptions { get; set; }
        [XmlElement(ElementName = "textOptions", Namespace = "http://www.abbyy.com/FineReader/TextExportOptions")]
        public TextOptions TextOptions { get; set; }
        [XmlElement(ElementName = "csvOptions", Namespace = "http://www.abbyy.com/FineReader/CSVExportOptions")]
        public CsvOptions CsvOptions { get; set; }
        [XmlElement(ElementName = "xmlOptions", Namespace = "http://www.abbyy.com/FineReader/XMLExportOptions")]
        public XmlOptions XmlOptions { get; set; }
        [XmlElement(ElementName = "ebookOptions", Namespace = "http://www.abbyy.com/FineReader/EbookExportOptions")]
        public EbookOptions EbookOptions { get; set; }
        [XmlElement(ElementName = "DjVuOptions", Namespace = "http://www.abbyy.com/FineReader/DjVuExportOptions")]
        public DjVuOptions DjVuOptions { get; set; }
        [XmlElement(ElementName = "xpsOptions", Namespace = "http://www.abbyy.com/FineReader/XPSExportOptions")]
        public XpsOptions XpsOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
    }

    [XmlRoot(ElementName = "documentMetadata", Namespace = "http://www.abbyy.com/FineReader/DocumentMetadataImpl")]
    public class DocumentMetadata
    {
        [XmlElement(ElementName = "authorsInfo", Namespace = "http://www.abbyy.com/FineReader/DocumentMetadataImpl")]
        public string AuthorsInfo { get; set; }
        [XmlElement(ElementName = "title", Namespace = "http://www.abbyy.com/FineReader/DocumentMetadataImpl")]
        public string Title { get; set; }
        [XmlElement(ElementName = "subject", Namespace = "http://www.abbyy.com/FineReader/DocumentMetadataImpl")]
        public string Subject { get; set; }
        [XmlElement(ElementName = "keywords", Namespace = "http://www.abbyy.com/FineReader/DocumentMetadataImpl")]
        public string Keywords { get; set; }
        [XmlElement(ElementName = "producer", Namespace = "http://www.abbyy.com/FineReader/DocumentMetadataImpl")]
        public string Producer { get; set; }
        [XmlElement(ElementName = "creator", Namespace = "http://www.abbyy.com/FineReader/DocumentMetadataImpl")]
        public string Creator { get; set; }
        [XmlElement(ElementName = "creationDate", Namespace = "http://www.abbyy.com/FineReader/DocumentMetadataImpl")]
        public string CreationDate { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
    }

    [XmlRoot(ElementName = "batchOptions", Namespace = "http://www.abbyy.com/FineReader/BatchOptions")]
    public class BatchOptions
    {
        [XmlElement(ElementName = "ocr", Namespace = "http://www.abbyy.com/FineReader/OCROptions")]
        public Ocr Ocr { get; set; }
        [XmlElement(ElementName = "preprocess", Namespace = "http://www.abbyy.com/FineReader/ImagePreprocessOptions")]
        public Preprocess Preprocess { get; set; }
        [XmlElement(ElementName = "documentSynthesis", Namespace = "http://www.abbyy.com/FineReader/DocumentSynthesisOptions")]
        public DocumentSynthesis DocumentSynthesis { get; set; }
        [XmlElement(ElementName = "export", Namespace = "http://www.abbyy.com/FineReader/ExportOptionsImpl")]
        public Export Export { get; set; }
        [XmlElement(ElementName = "documentMetadata", Namespace = "http://www.abbyy.com/FineReader/DocumentMetadataImpl")]
        public DocumentMetadata DocumentMetadata { get; set; }
        [XmlElement(ElementName = "userPatterns", Namespace = "http://www.abbyy.com/FineReader/BatchOptions")]
        public string UserPatterns { get; set; }
        [XmlElement(ElementName = "properties", Namespace = "http://www.abbyy.com/FineReader/BatchOptions")]
        public string Properties { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "hasLanguageDataBase")]
        public string HasLanguageDataBase { get; set; }
    }

    [XmlRoot(ElementName = "openBatch", Namespace = "http://www.abbyy.com/HotFolder/Engine/OpenBatchStep")]
    public class OpenBatch
    {
        [XmlElement(ElementName = "batchOptions", Namespace = "http://www.abbyy.com/FineReader/BatchOptions")]
        public BatchOptions BatchOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "needCreateNewBatch")]
        public string NeedCreateNewBatch { get; set; }
        [XmlAttribute(AttributeName = "batchToOpenPath")]
        public string BatchToOpenPath { get; set; }
        [XmlAttribute(AttributeName = "batchToCreateFolder")]
        public string BatchToCreateFolder { get; set; }
    }

    [XmlRoot(ElementName = "addImages", Namespace = "http://www.abbyy.com/HotFolder/Engine/TaskAddEmagesStep")]
    public class AddImages
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "folderType")]
        public string FolderType { get; set; }
        [XmlAttribute(AttributeName = "folderContent")]
        public string FolderContent { get; set; }
        [XmlAttribute(AttributeName = "imageFormatMask")]
        public string ImageFormatMask { get; set; }
        [XmlAttribute(AttributeName = "folderPath")]
        public string FolderPath { get; set; }
        [XmlAttribute(AttributeName = "outlookFolderId")]
        public string OutlookFolderId { get; set; }
        [XmlAttribute(AttributeName = "processSubfolders")]
        public string ProcessSubfolders { get; set; }
        [XmlAttribute(AttributeName = "ftpLogin")]
        public string FtpLogin { get; set; }
        [XmlAttribute(AttributeName = "ftpPassword")]
        public string FtpPassword { get; set; }
        [XmlAttribute(AttributeName = "isAnonymousFtpLogin")]
        public string IsAnonymousFtpLogin { get; set; }
    }

    [XmlRoot(ElementName = "analyzeRecognize", Namespace = "http://www.abbyy.com/HotFolder/Engine/ProcessDocumentStep")]
    public class AnalyzeRecognize
    {
        [XmlElement(ElementName = "batchOptions", Namespace = "http://www.abbyy.com/FineReader/BatchOptions")]
        public BatchOptions BatchOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "autoAnalyzeBlocks")]
        public string AutoAnalyzeBlocks { get; set; }
        [XmlAttribute(AttributeName = "blocksFilePath")]
        public string BlocksFilePath { get; set; }
        [XmlAttribute(AttributeName = "stepMode")]
        public string StepMode { get; set; }
        [XmlAttribute(AttributeName = "recognitionOptionsSpecified")]
        public string RecognitionOptionsSpecified { get; set; }
    }

    [XmlRoot(ElementName = "exportOptions", Namespace = "http://www.abbyy.com/FineReader/ExportOptionsImpl")]
    public class ExportOptions
    {
        [XmlElement(ElementName = "rtfOptions", Namespace = "http://www.abbyy.com/FineReader/RTFExportOptions")]
        public RtfOptions RtfOptions { get; set; }
        [XmlElement(ElementName = "xlOptions", Namespace = "http://www.abbyy.com/FineReader/XLExportOptions")]
        public XlOptions XlOptions { get; set; }
        [XmlElement(ElementName = "pdfOptions", Namespace = "http://www.abbyy.com/FineReader/PDFExportOptions")]
        public PdfOptions PdfOptions { get; set; }
        [XmlElement(ElementName = "htmlOptions", Namespace = "http://www.abbyy.com/FineReader/HTMLExportOptions")]
        public HtmlOptions HtmlOptions { get; set; }
        [XmlElement(ElementName = "pptOptions", Namespace = "http://www.abbyy.com/FineReader/PPTExportOptions")]
        public PptOptions PptOptions { get; set; }
        [XmlElement(ElementName = "textOptions", Namespace = "http://www.abbyy.com/FineReader/TextExportOptions")]
        public TextOptions TextOptions { get; set; }
        [XmlElement(ElementName = "csvOptions", Namespace = "http://www.abbyy.com/FineReader/CSVExportOptions")]
        public CsvOptions CsvOptions { get; set; }
        [XmlElement(ElementName = "xmlOptions", Namespace = "http://www.abbyy.com/FineReader/XMLExportOptions")]
        public XmlOptions XmlOptions { get; set; }
        [XmlElement(ElementName = "ebookOptions", Namespace = "http://www.abbyy.com/FineReader/EbookExportOptions")]
        public EbookOptions EbookOptions { get; set; }
        [XmlElement(ElementName = "DjVuOptions", Namespace = "http://www.abbyy.com/FineReader/DjVuExportOptions")]
        public DjVuOptions DjVuOptions { get; set; }
        [XmlElement(ElementName = "xpsOptions", Namespace = "http://www.abbyy.com/FineReader/XPSExportOptions")]
        public XpsOptions XpsOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
    }

    [XmlRoot(ElementName = "processPdfOptions", Namespace = "http://www.abbyy.com/PdfEngine/CreatePdfOptions")]
    public class ProcessPdfOptions
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
    }

    [XmlRoot(ElementName = "step", Namespace = "http://www.abbyy.com/HotFolder/Engine/SaveStep")]
    public class Step
    {
        [XmlElement(ElementName = "exportOptions", Namespace = "http://www.abbyy.com/FineReader/ExportOptionsImpl")]
        public ExportOptions ExportOptions { get; set; }
        [XmlElement(ElementName = "processPdfOptions", Namespace = "http://www.abbyy.com/PdfEngine/CreatePdfOptions")]
        public ProcessPdfOptions ProcessPdfOptions { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "exportFormat")]
        public string ExportFormat { get; set; }
        [XmlAttribute(AttributeName = "savePath")]
        public string SavePath { get; set; }
        [XmlAttribute(AttributeName = "exportName")]
        public string ExportName { get; set; }
        [XmlAttribute(AttributeName = "userName")]
        public string UserName { get; set; }
        [XmlAttribute(AttributeName = "userPassword")]
        public string UserPassword { get; set; }
        [XmlAttribute(AttributeName = "useAutoLogon")]
        public string UseAutoLogon { get; set; }
        [XmlAttribute(AttributeName = "exportOptionsModification")]
        public string ExportOptionsModification { get; set; }
    }

    [XmlRoot(ElementName = "saveStepsCollection", Namespace = "http://www.abbyy.com/HotFolder/Engine/SaveStepsCollection")]
    public class SaveStepsCollection
    {
        [XmlElement(ElementName = "step", Namespace = "http://www.abbyy.com/HotFolder/Engine/SaveStep")]
        public Step Step { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
    }

    [XmlRoot(ElementName = "postProcessing", Namespace = "http://www.abbyy.com/HotFolder/Engine/PostProcessingStep")]
    public class PostProcessing
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "postProcessAction")]
        public string PostProcessAction { get; set; }
        [XmlAttribute(AttributeName = "processedFilesFolder")]
        public string ProcessedFilesFolder { get; set; }
        [XmlAttribute(AttributeName = "outlookFolderId")]
        public string OutlookFolderId { get; set; }
        [XmlAttribute(AttributeName = "copyLogToOutputFolder")]
        public string CopyLogToOutputFolder { get; set; }
        [XmlAttribute(AttributeName = "localeFolderPath")]
        public string LocaleFolderPath { get; set; }
        [XmlAttribute(AttributeName = "needLocaleCopy")]
        public string NeedLocaleCopy { get; set; }
    }

    [XmlRoot(ElementName = "engineTask", Namespace = "http://www.abbyy.com/HotFolder/Engine/Task")]
    public class EngineTask
    {
        [XmlElement(ElementName = "openBatch", Namespace = "http://www.abbyy.com/HotFolder/Engine/OpenBatchStep")]
        public OpenBatch OpenBatch { get; set; }
        [XmlElement(ElementName = "addImages", Namespace = "http://www.abbyy.com/HotFolder/Engine/TaskAddEmagesStep")]
        public AddImages AddImages { get; set; }
        [XmlElement(ElementName = "analyzeRecognize", Namespace = "http://www.abbyy.com/HotFolder/Engine/ProcessDocumentStep")]
        public AnalyzeRecognize AnalyzeRecognize { get; set; }
        [XmlElement(ElementName = "saveStepsCollection", Namespace = "http://www.abbyy.com/HotFolder/Engine/SaveStepsCollection")]
        public SaveStepsCollection SaveStepsCollection { get; set; }
        [XmlElement(ElementName = "postProcessing", Namespace = "http://www.abbyy.com/HotFolder/Engine/PostProcessingStep")]
        public PostProcessing PostProcessing { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "objectVersion", Namespace = "http://www.abbyy.com/XMLArchive/systemDataNamespace")]
        public string ObjectVersion { get; set; }
        [XmlAttribute(AttributeName = "checkFilesChanging")]
        public string CheckFilesChanging { get; set; }
        [XmlAttribute(AttributeName = "useProcessedImagesList")]
        public string UseProcessedImagesList { get; set; }
        [XmlAttribute(AttributeName = "continueExportCounter")]
        public string ContinueExportCounter { get; set; }
        [XmlAttribute(AttributeName = "dontStoreLogWhenNothingDone")]
        public string DontStoreLogWhenNothingDone { get; set; }
        [XmlAttribute(AttributeName = "fileSplitMode")]
        public string FileSplitMode { get; set; }
    }

   

}
