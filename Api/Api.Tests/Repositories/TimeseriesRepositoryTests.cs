using Api.Repositories;
using Api.Repositories.Configuration;
using Api.Repositories.Models;
using MockQueryable.NSubstitute;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Tests.Repositories
{
    [TestFixture]
    public class TimeseriesRepositoryTests
    {
        private ITimeseriesRepository repository;

        private IGenericDbContext<Timeseries> dbContextMock;

        private const string group1Name = nameof(group1Name);
        private const string group2Name = nameof(group2Name);

        private List<Timeseries> testEntities { get; set; }

        [SetUp]
        public void SetUp()
        {
            testEntities = CreateTestTimeseries();
            var testEntitiesMock = testEntities.AsQueryable().BuildMock();
            dbContextMock = Substitute.For<IGenericDbContext<Timeseries>>();
            dbContextMock.GetQueryable().ReturnsForAnyArgs(testEntitiesMock);

            repository = new TimeseriesRepository(dbContextMock);
        }

        [Test]
        public async Task Query_ReturnsAllTimeSeriesOfTheSameName()
        {
            // Arrange
            var searchName = group1Name;
            var expectedResult = testEntities.Where(x => x.Name == searchName).ToList();

            // Act
            var result = await repository.Query(searchName, null, null);

            // Assert
            Assert.That(result, Is.EquivalentTo(expectedResult));
        }

        [TestCase("01/01/2020", null)]
        [TestCase(null, "12/30/2020")]
        [TestCase("01/01/2020", "12/30/2020")]
        public async Task Query_ReturnsFilteredTimeseries_ForGivenPeriod(DateTime? fromDate, DateTime? toDate)
        {
            // Arrange
            var searchName = group1Name;
            var expectedResult = testEntities
                .Where(x => x.Name == searchName)
                .Where(x => !fromDate.HasValue || x.Timestamp >= fromDate.Value)
                .Where(x => !toDate.HasValue || x.Timestamp <= toDate.Value)
                .ToList();

            // Act
            var from = GetUnixEpoch(fromDate);
            var to = GetUnixEpoch(toDate);
            var result = await repository.Query(searchName, from, to);

            // Assert
            Assert.That(result, Is.EquivalentTo(expectedResult));
        }

        private List<Timeseries> CreateTestTimeseries()
        {
            return new List<Timeseries>
            {
                CreateTimeseries(group1Name, new DateTime(2019, 1, 1), 1),
                CreateTimeseries(group1Name, new DateTime(2020, 6, 1), 42),
                CreateTimeseries(group1Name, new DateTime(2021, 1, 1), 999),
                CreateTimeseries(group2Name, new DateTime(2019, 1, 1), 123),
                CreateTimeseries(group2Name, new DateTime(2020, 6, 1), 42),
                CreateTimeseries(group2Name, new DateTime(2021, 1, 1), 999),
            };
        }

        private Timeseries CreateTimeseries(string name, DateTime timestamp, double value)
        {
            return new Timeseries
            {
                Name = name,
                Timestamp = timestamp,
                Value = value
            };
        }

        private long? GetUnixEpoch(DateTime? date)
        {
            return date.HasValue ? new DateTimeOffset(date.Value).ToUnixTimeSeconds() : null;
        }
    }
}
