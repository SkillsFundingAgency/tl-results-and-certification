using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement
{
    public  class IndustryPlacementDetails
    {
		public string IndustryPlacementStatus { get; set; }

		public int? HoursSpentOnPlacement { get; set; }

		public List<int?> SpecialConsiderationReasons { get; set; }

		public bool IndustryPlacementModelsUsed { get; set; }

		public bool? MultipleEmployerModelsUsed { get; set; }

		public IList<int?> OtherIndustryPlacementModels { get; set; }

		public IList<int?> IndustryPlacementModels { get; set; }		

		public bool? TemporaryFlexibilitiesUsed { get; set; }

		public bool? BlendedTemporaryFlexibilityUsed { get; set; }

		public IList<int?> TemporaryFlexibilities { get; set; }
	}
}
