using Aspose.Pdf;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Models.ResultSlips;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class ResultSlipsGeneratorService : ResultSlipsGeneratorServiceBase, IResultSlipsGeneratorService
    {
        public ResultSlipsGeneratorService(IBlobStorageService blobStorageService, ILogger<IResultSlipsGeneratorService> logger) : base(blobStorageService, logger)
        { }

        public byte[] GetByteData(IEnumerable<DownloadOverallResultSlipsData> data)
        {
            Document = new();
            MemoryStream stream = new();

            if (data == null || !data.Any())
            {
                Document.Pages.Add();
                Document.Save(stream);
                return stream.ToArray();
            }

            foreach (DownloadOverallResultSlipsData item in data)
            {
                var page = AddPageAndSetProperties();

                page.Paragraphs.Add(BuildResultSlip(item));
                page.Paragraphs[0].Margin = new MarginInfo() { Top = 50 };
            }

            Document.Save(stream);
            return stream.ToArray();
        }

        private static Table BuildResultSlip(DownloadOverallResultSlipsData learner)
        {
            Table table = CreateTableAndSetProperties();

            // Learner name and Uln
            ResultSlipRow row = new(new HeaderRowStyle());
            List<ResultSlipRowProperty> data = new()
            {
                 new () { Name = DownloadOverallResultSlipsHeader.LearnerName, ColSpan = COLSPAN.COLSPAN_2 } ,
                 new () { Name = DownloadOverallResultSlipsHeader.Uln }
            };
            table.Rows.Add(row.GetRow(data));

            row = new ResultSlipRow(new DataRowStyle());
            data = new()
            {
                 new () { Name = learner.LearnerName, ColSpan = COLSPAN.COLSPAN_2 } ,
                 new () { Name = learner.Uln.ToString() }
            };
            table.Rows.Add(row.GetRow(data));

            // Provider name and UKPRN
            row = new ResultSlipRow(new HeaderRowStyle());
            data = new()
            {
                 new () { Name = DownloadOverallResultSlipsHeader.ProviderName, ColSpan = COLSPAN.COLSPAN_2  },
                 new () { Name = DownloadOverallResultSlipsHeader.ProviderUkprn }
            };
            table.Rows.Add(row.GetRow(data));

            row = new ResultSlipRow(new DataRowStyle());
            data = new()
            {
                 new () {  Name = learner.ProviderName, ColSpan = COLSPAN.COLSPAN_2  },
                 new () {  Name = learner.ProviderUkprn }
            };
            table.Rows.Add(row.GetRow(data));

            // T level
            row = new ResultSlipRow(new HeaderRowStyle());
            data = new()
            {
                 new () { Name = DownloadOverallResultSlipsHeader.Tlevel, ColSpan = COLSPAN.COLSPAN_3 }
            };
            table.Rows.Add(row.GetRow(data));

            row = new ResultSlipRow(new DataRowStyle());
            data = new()
            {
                 new () { Name = learner.Tlevel, ColSpan = COLSPAN.COLSPAN_3 }
            };
            table.Rows.Add(row.GetRow(data));

            // Core component
            row = new ResultSlipRow(new HeaderRowStyle());
            data = new()
            {
                 new () { Name = DownloadOverallResultSlipsHeader.CoreComponent, ColSpan = COLSPAN.COLSPAN_3 }
            };
            table.Rows.Add(row.GetRow(data));

            row = new ResultSlipRow(new DataRowStyle());
            data = new()
            {
                 new () { Name = learner.CoreComponent, ColSpan = COLSPAN.COLSPAN_3 }
            };
            table.Rows.Add(row.GetRow(data));

            // Core component code, exam period and result
            row = new ResultSlipRow(new HeaderRowStyle());
            data = new()
            {
                  new () { Name = DownloadOverallResultSlipsHeader.CoreCode },
                  new () { Name = DownloadOverallResultSlipsHeader.CoreAssessmentExamPeriod },
                  new () { Name = DownloadOverallResultSlipsHeader.CoreResult }
            };
            table.Rows.Add(row.GetRow(data));

            row = new ResultSlipRow(new DataRowStyle());
            data = new()
            {
                new () { Name = learner.CoreCode },
                new () { Name = learner.CoreAssessmentSeries },
                new () { Name = learner.CoreResult }
            };
            table.Rows.Add(row.GetRow(data));

            // Specialism component
            row = new ResultSlipRow(new HeaderRowStyle());
            data = new()
            {
                 new () { Name = DownloadOverallResultSlipsHeader.SpecialismComponent, ColSpan = COLSPAN.COLSPAN_3 }
            };
            table.Rows.Add(row.GetRow(data));

            row = new ResultSlipRow(new DataRowStyle());
            data = new()
            {
                  new () { Name = learner.SpecialismComponent, ColSpan = COLSPAN.COLSPAN_3 }
            };
            table.Rows.Add(row.GetRow(data));

            // Specialism component code, exam period and result
            row = new ResultSlipRow(new HeaderRowStyle());
            data = new()
            {
                  new () { Name = DownloadOverallResultSlipsHeader.SpecialismCode },
                  new () { Name = DownloadOverallResultSlipsHeader.SpecialismAssessmentExamPeriod },
                  new () { Name = DownloadOverallResultSlipsHeader.SpecialismResult }
            };
            table.Rows.Add(row.GetRow(data));

            row = new ResultSlipRow(new DataRowStyle());
            data = new()
            {
                new () { Name = learner.SpecialismCode },
                new () { Name = learner.SpecialismAssessmentSeries },
                new () { Name = learner.SpecialismResult }
            };
            table.Rows.Add(row.GetRow(data));

            // Industry placement status and overall result
            row = new ResultSlipRow(new HeaderRowStyle());
            data = new()
            {
                new () { Name = DownloadOverallResultSlipsHeader.IndustryPlacementStatus, ColSpan = COLSPAN.COLSPAN_2 },
                new () { Name = DownloadOverallResultSlipsHeader.OverallResult }
            };
            table.Rows.Add(row.GetRow(data));

            row = new ResultSlipRow(new DataRowStyle());
            data = new()
            {
                new () { Name = learner.IndustryPlacementStatus, ColSpan = COLSPAN.COLSPAN_2 },
                new () { Name = learner.OverallResult }
            };
            table.Rows.Add(row.GetRow(data));

            return table;
        }

        private Page AddPageAndSetProperties()
        {
            Page page = Document.Pages.Add();
            SetupPageProperties(page);
            AddHeaderImage(page);

            return page;
        }

        private static void AddHeaderImage(Page page)
        {
            int lowerLeftX = 90;
            int lowerLeftY = 475;
            int upperRightX = 250;
            int upperRightY = 525;

            using (FileStream imageStream = new FileStream(Common.Utils.Image.GetResultSlipHeaderLogo(), FileMode.Open))
            {
                page.Resources.Images.Add(imageStream);

                page.Contents.Add(new Aspose.Pdf.Operators.GSave());

                Rectangle rectangle = new(lowerLeftX, lowerLeftY, upperRightX, upperRightY);
                Matrix matrix = new(new double[] { rectangle.URX - rectangle.LLX, 0, 0, rectangle.URY - rectangle.LLY, rectangle.LLX, rectangle.LLY });

                page.Contents.Add(new Aspose.Pdf.Operators.ConcatenateMatrix(matrix));
                XImage ximage = page.Resources.Images[page.Resources.Images.Count];

                page.Contents.Add(new Aspose.Pdf.Operators.Do(ximage.Name));
                page.Contents.Add(new Aspose.Pdf.Operators.GRestore());
            }
        }

        private static Table CreateTableAndSetProperties()
        {
            Table table = new();
            SetTableProperties(table);
            return table;
        }

        private static void SetupPageProperties(Page page)
        {
            page.PageInfo.IsLandscape = true;

            Rectangle r = page.MediaBox;
            double newHeight = r.Width;
            double newWidth = r.Height;
            double newLLX = r.LLX;

            double newLLY = r.LLY + (r.Height - newHeight);
            page.MediaBox = new Rectangle(newLLX, newLLY, newLLX + newWidth, newLLY + newHeight);
            page.CropBox = new Rectangle(newLLX, newLLY, newLLX + newWidth, newLLY + newHeight);
        }

        private static void SetTableProperties(Table table)
        {
            table.ColumnWidths = "150 150 150";
            table.ColumnAdjustment = ColumnAdjustment.AutoFitToWindow;
            table.VerticalAlignment = VerticalAlignment.Bottom;

            table.DefaultCellTextState = new Aspose.Pdf.Text.TextState("Arial", 8f);
            table.DefaultCellBorder = new BorderInfo(BorderSide.All, 0.1f);
            table.Border = new BorderInfo(BorderSide.All, 0.6f);

            MarginInfo margin = new()
            {
                Top = 6f,
                Left = 4f,
                Right = 4f,
                Bottom = 6f
            };

            table.DefaultCellPadding = margin;
        }
    }
}