﻿using Xunit;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Linq;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_GetMany_Is_Called : BaseTest<TlRoute>
    {
        private IEnumerable<TlRoute> _result;
        private IList<TlRoute> _data;

        public override void Given()
        {
            _data = new TlRouteBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public override void When()
        {
            _result = Repository.GetManyAsync().ToList();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Paths_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_First_Path_Fields_Have_Expected_Values()
        {
            var testData = _data.FirstOrDefault();
            var result = _result.FirstOrDefault();
            testData.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().BeEquivalentTo(testData.Name);
            result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            result.CreatedOn.Should().Be(Constants.CreatedOn);
            result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
