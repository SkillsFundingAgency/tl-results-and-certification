using Aspose.Pdf;
using Aspose.Pdf.Text;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using System.IO;

namespace Sfa.Tl.ResultsAndCertification.Application.Models.ResultSlips
{
    public partial class ResultSlipsGeneratorService
    {
        public class ResultSlipBuilder
        {
            private readonly ISystemProvider _systemProvider;
            private readonly DownloadOverallResultSlipsData _learner;
            private readonly Page _page;
            private readonly ResultsTableBuilder _resultsTableBuilder;
            private readonly LearnerTableBuilder _learnerTableBuilder;

            private Row BlankRow => new Row()
            {
                Border = new BorderInfo(BorderSide.None),
                Cells = {
                    new Cell
                    {
                        Border = new BorderInfo(BorderSide.None),
                        Paragraphs =
                        {
                            new TextFragment(string.Empty)
                            {
                                TextState = { FontSize = 20 }
                            }
                        }
                    }
                }
            };

            private ResultSlipBuilder(DownloadOverallResultSlipsData learner, Page page, ISystemProvider systemProvider)
            {
                _learner = learner;
                _page = page;
                _systemProvider = systemProvider;
                _resultsTableBuilder = ResultsTableBuilder.Create(_learner);
                _learnerTableBuilder = LearnerTableBuilder.Create(_learner);
            }

            public static ResultSlipBuilder Create(DownloadOverallResultSlipsData learner,
                Page page,
                ISystemProvider systemProvider)
                    => new ResultSlipBuilder(learner, page, systemProvider);

            public Page Build() => _page;

            public ResultSlipBuilder AddLearnerTable()
            {
                _page.Paragraphs.Add(
                    _learnerTableBuilder
                        .BuildTable()
                        .Build());

                return this;
            }

            public ResultSlipBuilder AddResultsTable()
            {
                _page.Paragraphs.Add(
                    _resultsTableBuilder
                        .BuildTable()
                        .Build());

                return this;
            }

            public ResultSlipBuilder AddHeader()
            {
                var headerTable = new Table
                {
                    ColumnWidths = "100%",
                    DefaultCellPadding = new MarginInfo(5, 5, 5, 5),
                    Border = new BorderInfo(BorderSide.None),
                };

                headerTable.Margin.Top = 25;

                Row row = new Row();
                headerTable.Rows.Add(row);

                // blank row
                headerTable.Rows.Add(BlankRow);

                var dateRow = headerTable.Rows.Add().Cells.Add();
                var dateText = new TextFragment($"Date generated: {_systemProvider.UtcNow.ToString("dd MMMM yyyy")}");
                dateText.HorizontalAlignment = HorizontalAlignment.Right;
                dateText.TextState.FontSize = 10;
                dateRow.Paragraphs.Add(dateText);

                // blank row
                headerTable.Rows.Add(BlankRow);

                var title = new TextFragment("T Level Results Slip")
                {
                    TextState =
                {
                    FontSize = 16,
                    FontStyle = FontStyles.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center
                }
                };
                title.HorizontalAlignment = HorizontalAlignment.Center;

                var headerTitleRow = headerTable.Rows.Add().Cells.Add();
                headerTitleRow.Paragraphs.Add(title);

                // blank row
                headerTable.Rows.Add(BlankRow);

                _page.Paragraphs.Add(headerTable);

                return this;
            }

            public ResultSlipBuilder AddFooter()
            {
                var footer = new TextFragment();
                footer.Text = "This is not a formal certificate or statement of achievement.\n" +
                              "Core and Occupational Specialism grades issued by the awarding organisation may be subject to review under post-results services.\n" +
                              "A certificate or statement of achievement is issued to all learners by the Department for Education at the end of their second year of study, following the completion of post-results activities for summer assessments.\n" +
                              "Where a learner has multiple assessment entries for the Core or Occupational Specialism the highest attained grade is used in the calculation of the overall T Level result.";

                footer.TextState.FontSize = 8;
                footer.TextState.LineSpacing = 3;
                footer.TextState.FormattingOptions = new TextFormattingOptions()
                {
                    WrapMode = TextFormattingOptions.WordWrapMode.ByWords,
                    LineSpacing = TextFormattingOptions.LineSpacingMode.FullSize,

                };

                _page.Paragraphs.Add(footer);

                return this;
            }

            public ResultSlipBuilder AddHeaderImage()
            {
                int lowerLeftX = 40;
                int lowerLeftY = 775;
                int upperRightX = 250;
                int upperRightY = 825;

                using (FileStream imageStream = new FileStream(Common.Utils.Image.GetResultSlipHeaderLogo(), FileMode.Open))
                {
                    _page.Resources.Images.Add(imageStream);

                    _page.Contents.Add(new Aspose.Pdf.Operators.GSave());

                    Rectangle rectangle = new(lowerLeftX, lowerLeftY, upperRightX, upperRightY);
                    Aspose.Pdf.Matrix matrix = new(new double[] { rectangle.URX - rectangle.LLX, 0, 0, rectangle.URY - rectangle.LLY, rectangle.LLX, rectangle.LLY });

                    _page.Contents.Add(new Aspose.Pdf.Operators.ConcatenateMatrix(matrix));
                    XImage ximage = _page.Resources.Images[_page.Resources.Images.Count];

                    _page.Contents.Add(new Aspose.Pdf.Operators.Do(ximage.Name));
                    _page.Contents.Add(new Aspose.Pdf.Operators.GRestore());
                }
                return this;
            }
        }
    }
}