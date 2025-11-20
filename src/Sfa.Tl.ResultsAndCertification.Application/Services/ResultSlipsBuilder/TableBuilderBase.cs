using Aspose.Pdf;
using Aspose.Pdf.Text;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;

namespace Sfa.Tl.ResultsAndCertification.Application.Services.ResultSlipsBuilder
{
    public abstract class TableBuilderBase
    {
        protected DownloadOverallResultSlipsData _data;

        protected MarginInfo tableCellPadding = new MarginInfo(0, 5, 5, 5);
        protected MarginInfo sectionHeaderCellMargin = new MarginInfo(0, 10, 0, 8);
        protected float defaultFontSize = 11f;

        protected TextFragment SeperatorFrament => new TextFragment("——————————————————————————————————————————————————");

    }
}
