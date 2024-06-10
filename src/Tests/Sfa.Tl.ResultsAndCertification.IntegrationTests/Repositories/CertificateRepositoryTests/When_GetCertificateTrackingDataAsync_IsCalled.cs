using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.CertificateRepositoryTests
{
    public class When_GetCertificateTrackingDataAsync_IsCalled : BaseTest<PrintCertificate>
    {
        private ILogger<CertificateRepository> _logger;
        private ICertificateRepository _repository;

        public override void Given()
        {
            TqRegistrationPathway firstReg = SeedRegistrationPathway(1111111111, "Peter", "Smith");
            Batch firstBatch = SeedBatch(BatchType.Printing, BatchStatus.Accepted, new DateTime(2024, 1, 1));
            PrintBatchItem firstPrintBatchItem = SeedPrintBatchItem("peter-smith-tracking-id", PrintingBatchItemStatus.AwaitingProcessing, "peter-smith-signed-for-by", new DateTime(2024, 1, 1), firstBatch);
            PrintCertificate firstPrintCertificate = SeedPrintCertificate(PrintCertificateType.Certificate, "peter-smith-learning-details", firstReg, firstPrintBatchItem);

            TqRegistrationPathway secondReg = SeedRegistrationPathway(2222222222, "Jessica", "Johnson");
            Batch secondBatch = SeedBatch(BatchType.NotSpecified, BatchStatus.Created, new DateTime(2023, 5, 31));
            PrintBatchItem secondPrintBatchItem = SeedPrintBatchItem("jessica-johnson-tracking-id", PrintingBatchItemStatus.NotSpecified, "jessica-johnson-signed-for-by", new DateTime(2023, 8, 15), secondBatch);
            PrintCertificate secondPrintCertificate = SeedPrintCertificate(PrintCertificateType.StatementOfAchievement, "jessica-johnson-learning-details", secondReg, secondPrintBatchItem);

            TqRegistrationPathway thirdReg = SeedRegistrationPathway(3333333333, "Thomas", "Evans");
            Batch thirdBatch = SeedBatch(BatchType.Printing, BatchStatus.Accepted, new DateTime(2023, 3, 1));
            PrintBatchItem thirdPrintBatchItem = SeedPrintBatchItem("thomas-evans-tracking-id", PrintingBatchItemStatus.QueuedForPrinting, "thomas-evans-signed-for-by", new DateTime(2023, 3, 6), thirdBatch);
            PrintCertificate thirdPrintCertificate = SeedPrintCertificate(PrintCertificateType.Certificate, "thomas-evans-learning-details", thirdReg, thirdPrintBatchItem);

            DbContext.AddRange(firstPrintCertificate, secondPrintCertificate, thirdPrintCertificate);
            DbContext.SaveChanges();

            _logger = new Logger<CertificateRepository>(new NullLoggerFactory());
            _repository = new CertificateRepository(_logger, DbContext);
        }

        public override Task When()
            => Task.CompletedTask;

        public Task<IList<PrintCertificate>> WhenAsync(DateTime from)
            => _repository.GetCertificateTrackingDataAsync(() => from);

        [Theory]
        [MemberData(nameof(Data))]
        public async void Then_Returns_Expected_Results(DateTime from, List<PrintCertificate> printCertificates)
        {
            IList<PrintCertificate> result = await WhenAsync(from);

            result.Should().BeEquivalentTo(printCertificates,
                opt => opt.Excluding(p => p.Id)
                .Excluding(p => p.CreatedOn)
                .Excluding(p => p.PrintBatchItemId)
                .Excluding(p => p.TqRegistrationPathwayId)
                .Excluding(p => p.TqRegistrationPathway.Id)
                .Excluding(p => p.TqRegistrationPathway.CreatedOn)
                .Excluding(p => p.TqRegistrationPathway.TqRegistrationProfileId)
                .Excluding(p => p.TqRegistrationPathway.TqRegistrationProfile.Id)
                .Excluding(p => p.TqRegistrationPathway.TqRegistrationProfile.CreatedOn)
                .Excluding(p => p.TqRegistrationPathway.TqRegistrationProfile.TqRegistrationPathways)
                .Excluding(p => p.TqRegistrationPathway.PrintCertificates)
                .Excluding(p => p.PrintBatchItem.Id)
                .Excluding(p => p.PrintBatchItem.CreatedOn)
                .Excluding(p => p.PrintBatchItem.PrintCertificates)
                .Excluding(p => p.PrintBatchItem.BatchId)
                .Excluding(p => p.PrintBatchItem.Batch.Id)
                .Excluding(p => p.PrintBatchItem.Batch.CreatedOn)
                .Excluding(p => p.PrintBatchItem.Batch.PrintBatchItems));
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new DateTime(2024, 1, 2),
                        new List<PrintCertificate>()
                    },
                    new object[]
                    {
                        new DateTime(2023, 6, 1),
                        new List<PrintCertificate>
                        {
                            new()
                            {
                                Type = PrintCertificateType.Certificate,
                                LearningDetails = "peter-smith-learning-details",
                                TqRegistrationPathway = new TqRegistrationPathway
                                {
                                    TqRegistrationProfile = new TqRegistrationProfile
                                    {
                                        UniqueLearnerNumber = 1111111111,
                                        Firstname = "Peter",
                                        Lastname = "Smith"
                                    }
                                },
                                PrintBatchItem = new PrintBatchItem
                                {
                                    TrackingId = "peter-smith-tracking-id",
                                    Status = PrintingBatchItemStatus.AwaitingProcessing,
                                    SignedForBy = "peter-smith-signed-for-by",
                                    StatusChangedOn = new DateTime(2024, 1, 1),
                                    Batch = new Batch
                                    {
                                        Type = BatchType.Printing,
                                        Status = BatchStatus.Accepted
                                    }
                                }
                            }
                        }
                    },
                    new object[]
                    {
                        new DateTime(2023, 5, 31),
                        new List<PrintCertificate>
                        {
                            new()
                            {
                                Type = PrintCertificateType.Certificate,
                                LearningDetails = "peter-smith-learning-details",
                                TqRegistrationPathway = new TqRegistrationPathway
                                {
                                    TqRegistrationProfile = new TqRegistrationProfile
                                    {
                                        UniqueLearnerNumber = 1111111111,
                                        Firstname = "Peter",
                                        Lastname = "Smith"
                                    }
                                },
                                PrintBatchItem = new PrintBatchItem
                                {
                                    TrackingId = "peter-smith-tracking-id",
                                    Status = PrintingBatchItemStatus.AwaitingProcessing,
                                    SignedForBy = "peter-smith-signed-for-by",
                                    StatusChangedOn = new DateTime(2024, 1, 1),
                                    Batch = new Batch
                                    {
                                        Type = BatchType.Printing,
                                        Status = BatchStatus.Accepted
                                    }
                                }
                            },
                            new()
                            {
                                Type = PrintCertificateType.StatementOfAchievement,
                                LearningDetails = "jessica-johnson-learning-details",
                                TqRegistrationPathway = new TqRegistrationPathway
                                {
                                    TqRegistrationProfile = new TqRegistrationProfile
                                    {
                                        UniqueLearnerNumber = 2222222222,
                                        Firstname = "Jessica",
                                        Lastname = "Johnson"
                                    }
                                },
                                PrintBatchItem = new PrintBatchItem
                                {
                                    TrackingId = "jessica-johnson-tracking-id",
                                    Status = PrintingBatchItemStatus.NotSpecified,
                                    SignedForBy = "jessica-johnson-signed-for-by",
                                    StatusChangedOn = new DateTime(2023, 8, 15),
                                    Batch = new Batch
                                    {
                                        Type = BatchType.NotSpecified,
                                        Status = BatchStatus.Created
                                    }
                                }
                            }
                        }
                    },
                    new object[]
                    {
                        new DateTime(2023, 3, 1),
                        new List<PrintCertificate>
                        {
                            new()
                            {
                                Type = PrintCertificateType.Certificate,
                                LearningDetails = "peter-smith-learning-details",
                                TqRegistrationPathway = new TqRegistrationPathway
                                {
                                    TqRegistrationProfile = new TqRegistrationProfile
                                    {
                                        UniqueLearnerNumber = 1111111111,
                                        Firstname = "Peter",
                                        Lastname = "Smith"
                                    }
                                },
                                PrintBatchItem = new PrintBatchItem
                                {
                                    TrackingId = "peter-smith-tracking-id",
                                    Status = PrintingBatchItemStatus.AwaitingProcessing,
                                    SignedForBy = "peter-smith-signed-for-by",
                                    StatusChangedOn = new DateTime(2024, 1, 1),
                                    Batch = new Batch
                                    {
                                        Type = BatchType.Printing,
                                        Status = BatchStatus.Accepted
                                    }
                                }
                            },
                            new()
                            {
                                Type = PrintCertificateType.StatementOfAchievement,
                                LearningDetails = "jessica-johnson-learning-details",
                                TqRegistrationPathway = new TqRegistrationPathway
                                {
                                    TqRegistrationProfile = new TqRegistrationProfile
                                    {
                                        UniqueLearnerNumber = 2222222222,
                                        Firstname = "Jessica",
                                        Lastname = "Johnson"
                                    }
                                },
                                PrintBatchItem = new PrintBatchItem
                                {
                                    TrackingId = "jessica-johnson-tracking-id",
                                    Status = PrintingBatchItemStatus.NotSpecified,
                                    SignedForBy = "jessica-johnson-signed-for-by",
                                    StatusChangedOn = new DateTime(2023, 8, 15),
                                    Batch = new Batch
                                    {
                                        Type = BatchType.NotSpecified,
                                        Status = BatchStatus.Created
                                    }
                                }
                            },
                            new()
                            {
                                Type = PrintCertificateType.Certificate,
                                LearningDetails = "thomas-evans-learning-details",
                                TqRegistrationPathway = new TqRegistrationPathway
                                {
                                    TqRegistrationProfile = new TqRegistrationProfile
                                    {
                                        UniqueLearnerNumber = 3333333333,
                                        Firstname = "Thomas",
                                        Lastname = "Evans"
                                    }
                                },
                                PrintBatchItem = new PrintBatchItem
                                {
                                    TrackingId = "thomas-evans-tracking-id",
                                    Status = PrintingBatchItemStatus.QueuedForPrinting,
                                    SignedForBy = "thomas-evans-signed-for-by",
                                    StatusChangedOn = new DateTime(2023, 3, 6),
                                    Batch = new Batch
                                    {
                                        Type = BatchType.Printing,
                                        Status = BatchStatus.Accepted
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }

        private TqRegistrationPathway SeedRegistrationPathway(long uln, string firstName, string lastName)
        {
            TqRegistrationProfile profile = SeedDatabase(() => new TqRegistrationProfile
            {
                UniqueLearnerNumber = uln,
                Firstname = firstName,
                Lastname = lastName
            });
            return SeedDatabase(() => new TqRegistrationPathway
            {
                TqRegistrationProfile = profile
            });
        }

        private Batch SeedBatch(BatchType batchType, BatchStatus batchStatus, DateTime createdOn)
            => SeedDatabase(() => new Batch
            {
                Type = batchType,
                Status = batchStatus,
                CreatedOn = createdOn
            });

        private PrintBatchItem SeedPrintBatchItem(string trackingId, PrintingBatchItemStatus printingBatchItemStatus, string signedForBy, DateTime? statusChangedOn, Batch batch)
            => SeedDatabase(() => new PrintBatchItem
            {
                TrackingId = trackingId,
                Batch = batch,
                Status = printingBatchItemStatus,
                SignedForBy = signedForBy,
                StatusChangedOn = statusChangedOn
            });

        private PrintCertificate SeedPrintCertificate(PrintCertificateType printCertificateType, string learningDetails, TqRegistrationPathway registrationPathway, PrintBatchItem printBatchItem)
            => SeedDatabase(() => new PrintCertificate
            {
                Type = printCertificateType,
                LearningDetails = learningDetails,
                TqRegistrationPathway = registrationPathway,
                PrintBatchItem = printBatchItem
            });

        private T SeedDatabase<T>(Func<T> createEntity)
        {
            T entity = createEntity();

            DbContext.Add(entity);
            return entity;
        }
    }
}