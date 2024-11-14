using Aspose.Pdf;
using Aspose.Pdf.Text;

namespace Sfa.Tl.ResultsAndCertification.Application.Models.ResultSlips
{
    public class HeaderRowStyle : IResultSlipRowStyle
    {
        public Color BackgroundColor => Color.LightGray;
        public TextState RowStyle
        {
            get
            {
                TextState textState = new TextState();
                textState.FontSize = 9f;
                textState.FontStyle = FontStyles.Bold;
                return textState;
            }
        }
    }
}
