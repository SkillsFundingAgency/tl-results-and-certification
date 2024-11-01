using Aspose.Pdf;
using Aspose.Pdf.Text;

namespace Sfa.Tl.ResultsAndCertification.Application.Models.ResultSlips
{
    public interface IResultSlipRowStyle
    {
        public Color BackgroundColor { get; }
        public TextState RowStyle { get; }
    }
}
