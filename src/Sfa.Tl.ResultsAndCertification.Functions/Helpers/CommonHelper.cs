using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace Sfa.Tl.ResultsAndCertification.Functions.Helpers
{
    public static class CommonHelper
    {
        public static FunctionLogDetails CreateFunctionLogRequest(string functionName)
        {
            return new FunctionLogDetails
            {
                Name = functionName,
                StartDate = DateTime.UtcNow,
                Status = FunctionStatus.Processing,
                PerformedBy = Constants.FunctionPerformedBy
            };
        }

        public static FunctionLogDetails UpdateFunctionLogRequest(FunctionLogDetails functionLogDetails, FunctionStatus status, string message = null)
        {
            if(functionLogDetails != null)
            {
                functionLogDetails.Status = status;
                functionLogDetails.EndDate = DateTime.UtcNow;
                functionLogDetails.Message = message;
            }
            return functionLogDetails;
        }

        public static EndpointAddress GetLrsEndpointAddress(string baseUri, string endpointName)
        {
            return new EndpointAddress(new Uri($"{baseUri?.TrimEnd('/')}{endpointName}"));
        }

        public static string ComputeSha256Hash(byte[] inputData)
        {
            if (inputData == null)
                return null;
            
            // Create a SHA256   
            using (var sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] hashBytes = sha256Hash.ComputeHash(inputData);

                //return BitConverter.ToString(hashBytes).Replace("-", "");

                // Convert byte array to a string   
                var builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
