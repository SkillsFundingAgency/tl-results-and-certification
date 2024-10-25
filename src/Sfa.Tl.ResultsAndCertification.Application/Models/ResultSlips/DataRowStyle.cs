using Aspose.Pdf;
using Aspose.Pdf.Text;

namespace Sfa.Tl.ResultsAndCertification.Application.Models.ResultSlips
{
    public class DataRowStyle : IResultSlipRowStyle
    {
        public Color BackgroundColor => Color.White;
        public TextState RowStyle
        {
            get
            {
                TextState textState = new TextState();
                textState.FontSize = 11f;
                return textState;
            }
        }
    }


}
