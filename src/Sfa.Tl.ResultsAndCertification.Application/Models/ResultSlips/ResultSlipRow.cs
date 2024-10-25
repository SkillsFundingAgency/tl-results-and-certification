using Aspose.Pdf;
using Aspose.Pdf.Text;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Models.ResultSlips
{
    public class ResultSlipRow
    {
        private readonly IResultSlipRowStyle _tableStyle;
        protected readonly Row row;
        private Cell cell;
        private TextFragment fragment;

        public ResultSlipRow(IResultSlipRowStyle tableStyle)
        {
            _tableStyle = tableStyle;

            row = new Row();
            row.BackgroundColor = _tableStyle.BackgroundColor;
        }

        public Row GetRow(IList<ResultSlipRowProperty> rowProperties)
        {
            row.DefaultCellTextState = _tableStyle.RowStyle;

            foreach (ResultSlipRowProperty property in rowProperties)
            {
                cell = new Cell();
                fragment = new TextFragment(property.Name ?? string.Empty);

                if (property.ColSpan > 0)
                    cell.ColSpan = ((int)property.ColSpan);

                cell.Paragraphs.Add(fragment);
                row.Cells.Add(cell);
            }
            return row;
        }
    }
}
