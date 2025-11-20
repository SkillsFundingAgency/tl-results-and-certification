using Aspose.Pdf;
using Aspose.Pdf.Text;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;

namespace Sfa.Tl.ResultsAndCertification.Application.Models.ResultSlips
{
    public class LearnerTableBuilder : TableBuilderBase
    {

        private readonly Table _table;

        private LearnerTableBuilder(DownloadOverallResultSlipsData data)
        {
            _data = data;

            _table = new Table
            {
                ColumnWidths = "30% 70%",
                DefaultCellPadding = tableCellPadding,
                Border = new BorderInfo(BorderSide.None),
                DefaultCellBorder = new BorderInfo(BorderSide.None),
            };
        }

        public static LearnerTableBuilder Create(DownloadOverallResultSlipsData data)
            => new LearnerTableBuilder(data);

        public LearnerTableBuilder BuildTable()
        {
            this.AddSubSectionHeader(_data.Tlevel, "")
                .AddRow(DownloadOverallResultSlipsHeader.LearnerName, _data.LearnerName)
                .AddRow(DownloadOverallResultSlipsHeader.Uln, _data.Uln.ToString())
                .AddRow(DownloadOverallResultSlipsHeader.ProviderName, _data.ProviderName)
                .AddRow(DownloadOverallResultSlipsHeader.ProviderUkprn, _data.ProviderUkprn)
                .AddRow(DownloadOverallResultSlipsHeader.OverallResult, _data.OverallResult)
                .AddSeparator();

            return this;
        }

        private LearnerTableBuilder AddRow(string label, string value)
        {
            var row = _table.Rows.Add();
            var labelCell = row.Cells.Add();
            var valueCell = row.Cells.Add();

            var labelText = new TextFragment(label)
            {
                TextState = {
                    FontSize = defaultFontSize,
                    FontStyle = FontStyles.Bold
                },
            };
            labelCell.Paragraphs.Add(labelText);

            var valueText = new TextFragment(value ?? string.Empty)
            {
                TextState = { FontSize = defaultFontSize }
            };
            valueCell.Paragraphs.Add(valueText);

            return this;
        }

        private LearnerTableBuilder AddSubSectionHeader(string label, string value)
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

        private LearnerTableBuilder AddSeparator()
        {
            _table.Rows.Add().Cells.Add().ColSpan = 2;
            _table.Rows[_table.Rows.Count - 1].Cells[0].Paragraphs.Add(SeperatorFrament);
            return this;
        }

        public Table Build() => _table;
    }

}