using Newtonsoft.Json;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Ucas
{
    public class UcasDataResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("orgID")]
        public string OrgId { get; set; }

        [JsonProperty("folderID")]
        public string FolderId { get; set; }        

        [JsonProperty("uploadStamp")]
        public string UploadStamp { get; set; }
    }
}
