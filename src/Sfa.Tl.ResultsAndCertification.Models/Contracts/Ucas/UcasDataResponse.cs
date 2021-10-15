using Newtonsoft.Json;
using System.Collections.Generic;

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

        [JsonProperty("detail")]
        public string Detail { get; set; }

        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errors")]
        public List<UcasError> Errors { get; set; }
    }    
}
