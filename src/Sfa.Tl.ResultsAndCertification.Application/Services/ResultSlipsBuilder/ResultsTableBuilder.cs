using Aspose.Pdf;
using Aspose.Pdf.Text;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;

namespace Sfa.Tl.ResultsAndCertification.Application.Services.ResultSlipsBuilder
{
    public class ResultsTableBuilder : TableBuilderBase
    {
        private readonly Table _table;

        private ResultsTableBuilder(DownloadOverallResultSlipsData data)
        {
            _data = data;

            _table = new Table
            {
                ColumnWidths = "50% 50%",
                DefaultCellPadding = tableCellPadding,
                Border = new BorderInfo(BorderSide.None),
                DefaultCellBorder = new BorderInfo(BorderSide.None),
            };
        }

        public static ResultsTableBuilder Create(DownloadOverallResultSlipsData data)
            => new ResultsTableBuilder(data);

        public ResultsTableBuilder BuildTable()
        {
            AddSectionHeader(DownloadOverallResultSlipsHeader.TitleCore, "")
                .AddSubSectionHeader($"{_data.CoreComponent} ({_data.CoreCode})", "")
                .AddRow(DownloadOverallResultSlipsHeader.HighestAttainedGradeSeries, _data.CoreAssessmentSeries)
                .AddRow(DownloadOverallResultSlipsHeader.CoreGrade, _data.CoreResult)
                .AddSeparator()
                .AddSectionHeader(DownloadOverallResultSlipsHeader.TitleSpecialism, "")
                .AddSubSectionHeader($"{_data.SpecialismComponent} ({_data.SpecialismCode})", "")
                .AddRow(DownloadOverallResultSlipsHeader.HighestAttainedGradeSeries, _data.SpecialismAssessmentSeries)
                .AddRow(DownloadOverallResultSlipsHeader.SpecialismGrade, _data.SpecialismResult)
                .AddSeparator()
                .AddSectionHeader(DownloadOverallResultSlipsHeader.IndustryPlacement, _data.IndustryPlacementStatus)
                .AddSeparator();

            return this;
        }

        private ResultsTableBuilder AddRow(string label, string value)
        {
            var row = _table.Rows.Add();
            var labelCell = row.Cells.Add();
            var valueCell = row.Cells.Add();

            var labelText = new TextFragment(label)
            {
                TextState = { FontSize = defaultFontSize }
            };
            labelCell.Paragraphs.Add(labelText);

            var valueText = new TextFragment(value ?? string.Empty)
            {
                TextState = { FontSize = defaultFontSize }
            };
            valueCell.Paragraphs.Add(valueText);

            return this;
        }

        private ResultsTableBuilder AddSubSectionHeader(string label, string value)
        {
            var row = _table.Rows.Add();
            var labelCell = row.Cells.Add();

            labelCell.Margin = sectionHeaderCellMargin;
            labelCell.ColSpan = 2;

            var labelText = new TextFragment(label)
            {
                TextState = { FontSize = defaultFontSize, FontStyle = FontStyles.Bold }
            };
            labelCell.Paragraphs.Add(labelText);

            return this;
        }

        private ResultsTableBuilder AddSectionHeader(string label, string value)
        {
            var row = _table.Rows.Add();

            var labelCell = row.Cells.Add();
            var valueCell = row.Cells.Add();

            labelCell.Margin = sectionHeaderCellMargin;
            valueCell.Margin = sectionHeaderCellMargin;

            var labelText = new TextFragment(label)
            {
                TextState = { FontSize = defaultFontSize, FontStyle = FontStyles.Bold }
            };
            labelCell.Paragraphs.Add(labelText);

            var valueText = new TextFragment(value ?? string.Empty)
            {
                TextState = { FontSize = defaultFontSize }
            };
            valueCell.Paragraphs.Add(valueText);

            return this;
        }

        private ResultsTableBuilder AddSeparator()
        {
            _table.Rows.Add().Cells.Add().ColSpan = 2;
            _table.Rows[_table.Rows.Count - 1].Cells[0].Paragraphs.Add(SeperatorFrament);
            return this;
        }

        public Table Build() => _table;
    }

}