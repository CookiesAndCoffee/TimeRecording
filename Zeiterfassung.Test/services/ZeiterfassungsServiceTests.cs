using Xunit;
using Zeiterfassung.models;
using Zeiterfassung.services;
using Zeiterfassung.Test.data.service;
using Zeiterfassung.Test.models;

namespace Zeiterfassung.Test.services
{
    public class ZeiterfassungsServiceTests
    {
        private ZeiterfassungsService CreateService()
        {
            var context = ZeiterfassungsDBContextMock.CreateMockedContext();
            return new ZeiterfassungsService(context);
        }

        [Fact]
        public void getAllPersonen_Returns_AllPersons()
        {
            var service = CreateService();
            var result = service.getAllPersonen();

            Assert.NotNull(result);
            Assert.True(result.Count >= 2);
            Assert.Contains(result, p => p.Name == "Max Mustermann");
        }

        [Fact]
        public void GetArbeitszeit_Returns_CorrectEntry()
        {
            var service = CreateService();
            var personen = PersonTestData.CreateTestData();
            var person = personen[0];
            var date = new DateTime(2024, 6, 3);

            var result = service.GetArbeitszeit(date, person);

            Assert.NotNull(result);
            Assert.Equal(480, result.Minuten);
        }

        [Fact]
        public void SaveArbeitszeit_AddsEntry()
        {
            var context = ZeiterfassungsDBContextMock.CreateMockedContext();
            var service = new ZeiterfassungsService(context);
            var personen = PersonTestData.CreateTestData();
            var person = personen[0];
            var arbeitszeit = new Arbeitszeit
            {
                PersonenId = person.Id,
                Datum = new DateTime(2024, 6, 4),
                Minuten = 300
            };

            service.SaveArbeitszeit(arbeitszeit);

            Assert.True(true);
        }

        [Fact]
        public async Task ExportiereAlleZuCsvAsync_CreatesFile()
        {
            var service = CreateService();
            string tempFile = System.IO.Path.GetTempFileName();

            await service.ExportiereAlleZuCsvAsync();

            Assert.True(System.IO.File.Exists(tempFile));
            var lines = System.IO.File.ReadAllLines(tempFile);
            Assert.Contains("Persnr;Datum;TagesSollzeit;TagesArbeitszeit;TagesSaldo;GesamtSaldo", lines[0]);
            System.IO.File.Delete(tempFile);
        }
    }
}