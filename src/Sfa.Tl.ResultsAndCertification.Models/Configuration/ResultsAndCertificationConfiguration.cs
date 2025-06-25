﻿using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Configuration
{
    public class ResultsAndCertificationConfiguration
    {
        /// <summary>
        /// Gets or sets the redis settings.
        /// </summary>
        /// <value>
        /// The redis settings.
        /// </value>
        public RedisSettings RedisSettings { get; set; }

        /// <summary>
        /// Gets or sets the BLOB storage settings.
        /// </summary>
        /// <value>
        /// The BLOB storage settings.
        /// </value>
        public BlobStorageSettings BlobStorageSettings { get; set; }

        /// <summary>
        /// Gets or sets the data protection settings.
        /// </summary>
        /// <value>
        /// The data protection settings.
        /// </value>
        public DataProtectionSettings DataProtectionSettings { get; set; }

        /// <summary>
        /// Gets or sets the SQL connection string.
        /// </summary>
        /// <value>
        /// The SQL connection string.
        /// </value>
        public string SqlConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the SQL connection string for Integration Tests.
        /// </summary>
        /// <value>
        /// The SQL connection string for Integration Tests.
        /// </value>
        public string IntTestSqlConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the BLOB storage connection string.
        /// </summary>
        /// <value>
        /// The BLOB storage connection string.
        /// </value>
        public string BlobStorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the gov uk notify API key.
        /// </summary>
        /// <value>
        /// The gov uk notify API key.
        /// </value>
        public string GovUkNotifyApiKey { get; set; }

        /// <summary>
        /// Gets or sets the tlevel queried support email address.
        /// </summary>
        /// <value>
        /// The tlevel queried support email address.
        /// </value>
        public string TlevelQueriedSupportEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the technical support email address.
        /// </summary>
        /// <value>
        /// The technical support email address.
        /// </value>
        public string TechnicalSupportEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the technical feedback email address.
        /// </summary>
        /// <value>
        /// The feedback email address.
        /// </value>
        public string FeedbackEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the technical internal notification email address.
        /// </summary>
        /// <value>
        /// The technical internal notification email address.
        /// </value>
        public TechnicalInternalNotificationRecipientsSettings TechnicalInternalNotificationRecipients { get; set; }

        /// <summary>
        /// Gets or sets the soa available date.
        /// </summary>
        /// <value>
        /// The soa available date.
        /// </value>
        public DateTime? SoaAvailableDate { get; set; }

        /// <summary>
        /// Gets or sets the soa rerequest in days.
        /// </summary>
        /// <value>
        /// The soa rerequest in days.
        /// </value>
        public int SoaRerequestInDays { get; set; }

        /// <summary>
        /// Gets or sets the document rerequest in days.
        /// </summary>
        /// <value>
        /// The document rerequest in days.
        /// </value>
        public int DocumentRerequestInDays { get; set; }

        /// <summary>
        /// Gets or sets the Overall results available date.
        /// </summary>
        /// <value>
        /// The Overall results available date.
        /// </value>
        public DateTime? OverallResultsAvailableDate { get; set; }

        /// <summary>
        /// Gets or sets the Overall results calculation date.
        /// </summary>
        /// <value>
        /// The soa available date.
        /// </value>
        public DateTime? OverallResultsCalculationDate { get; set; }

        /// <summary>
        /// Gets or sets the certificate printing batches create start date.
        /// </summary>
        /// <value>
        /// The certificate printing batches create start date.
        /// </value>
        public DateTime? CertificatePrintingBatchesCreateStartDate { get; set; }

        /// <summary>
        /// Gets or sets the dfe sign in settings.
        /// </summary>
        /// <value>
        /// The dfe sign in settings.
        /// </value>
        public DfeSignInSettings DfeSignInSettings { get; set; }

        /// <summary>
        /// Gets or sets the results and certification Internal API settings.
        /// </summary>
        /// <value>
        /// The results and certification Internal API settings.
        /// </value>
        public ResultsAndCertificationInternalApiSettings ResultsAndCertificationInternalApiSettings { get; set; }

        /// <summary>
        /// Gets or sets the ordnance survey API settings.
        /// </summary>
        /// <value>
        /// The ordnance survey API settings.
        /// </value>
        public OrdnanceSurveyApiSettings OrdnanceSurveyApiSettings { get; set; }

        /// <summary>
        /// Gets or sets the printing API settings.
        /// </summary>
        /// <value>
        /// The printing API settings.
        /// </value>
        public PrintingApiSettings PrintingApiSettings { get; set; }

        /// <summary>
        /// Gets or sets the Ucas API settings.
        /// </summary>
        /// <value>
        /// The Ucas API settings.
        /// </value>
        public UcasApiSettings UcasApiSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dev.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dev; otherwise, <c>false</c>.
        /// </value>
        public bool IsDevevelopment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [bypass dfe sign in].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [bypass dfe sign in]; otherwise, <c>false</c>.
        /// </value>
        public bool BypassDfeSignIn { get; set; }

        /// <summary>
        /// Gets or sets the learning record service settings.
        /// </summary>
        /// <value>
        /// The learning record service settings.
        /// </value>
        public LearningRecordServiceSettings LearningRecordServiceSettings { get; set; }

        // <summary>
        /// Gets or sets the key vault uri.
        /// </summary>
        /// <value>
        /// The key vault uri.
        /// </value>
        public string KeyVaultUri { get; set; }

        /// <summary>
        /// Gets or sets ucas data record settings.
        /// </summary>
        /// <value>
        /// The ucas data record settings.
        /// </value>
        public UcasDataSettings UcasDataSettings { get; set; }

        /// <summary>
        /// Gets or sets ovarall results batch settings.
        /// </summary>
        /// <value>
        /// The ovarall resutls batch record settings.
        /// </value>
        public OverallResultBatchSettings OverallResultBatchSettings { get; set; }

        /// <summary>
        /// Gets or sets certificate printing batch settings.
        /// </summary>
        /// <value>
        /// The certificate printing batch record settings.
        /// </value>
        public CertificatePrintingBatchSettings CertificatePrintingBatchSettings { get; set; }

        /// <summary>
        /// Gets or sets the analyst overall result extract settings.
        /// </summary>
        /// <value>
        /// The analyst overall result extract settings.
        /// </value>
        public AnalystOverallResultExtractSettings AnalystOverallResultExtractSettings { get; set; }

        /// <summary>
        /// Gets or sets the specialism Romm extract settings.
        /// </summary>
        /// <value>
        /// The specialism Romm extract settings.
        /// </value>
        public SpecialismRommExtractSettings SpecialismRommExtractSettings { get; set; }

        /// <summary>
        /// Gets or sets the core romm extract settings.
        /// </summary>
        /// <value>
        /// The  core romm extract settings.
        /// </value>
        public CoreRommExtractSettings CoreRommExtractSettings { get; set; }

        /// <summary>
        /// Gets or sets the analyst core result extract settings.
        /// </summary>
        /// <value>
        /// The analyst core result extract settings.
        /// </value>
        public ProviderAddressExtractSettings ProviderAddressExtractSettings { get; set; }

        /// <summary>
        /// Gets or sets the UCAS transfer amendments settings.
        /// </summary>
        /// <value>
        /// The UCAS transfer amendments settings.
        /// </value>
        public UcasTransferAmendmentsSettings UcasTransferAmendmentsSettings { get; set; }

        /// <summary>
        /// Gets or sets the certificate extract settings.
        /// </summary>
        /// <value>
        /// The certificate extract settings.
        /// </value>
        public CertificateTrackingExtractSettings CertificateTrackingExtractSettings { get; set; }

        /// <summary>
        /// Gets or sets industry placement chase big gaps reminder extract settings.
        /// </summary>
        /// <value>
        /// The industry placement chase big gaps reminder extract settings.
        /// </value>
        public IPChaseBigGapsReminderSettings IPChaseBigGapsReminderSettings { get; set; }

        /// <summary>
        /// Gets or sets the one outstanding uln reminder extract settings.
        /// </summary>
        /// <value>
        /// The one oustanding uln reminder extract settings.
        /// </value>
        public IPOneOutstandingUlnReminderSettings IPOneOutstandingUlnReminderSettings { get; set; }

        /// <summary>
        /// Gets or sets first deadline reminder reminder extract settings.
        /// </summary>
        /// <value>
        /// The industry first deadline reminder extract settings.
        /// </value>
        public IPProviderFirstDeadlineReminderSettings IPProviderFirstDeadlineReminderSettings { get; set; }

        /// <summary>
        /// Gets or sets the missed deadline reminder extract settings.
        /// </summary>
        /// <value>
        /// The missed deadline reminder extract settings.
        /// </value>
        public IPMissedDeadlineReminderSettings IPMissedDeadlineReminderSettings { get; set; }

        /// <summary>
        /// Gets service freezee period settings for AO and Provider.
        /// </summary>
        /// <value>
        /// The service freezee period settings for AO and Provider.
        /// </value>
        public ServiceFreezePeriods ServiceFreezePeriodsSettings { get; set; }


    }
}