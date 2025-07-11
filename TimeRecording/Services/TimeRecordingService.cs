using Microsoft.EntityFrameworkCore;
using System.IO;
using TimeRecording.Models;
using TimeRecording.Models.Extensions;
using TimeRecording.Setup;

namespace TimeRecording.Services
{
    public class TimeRecordingService
    {
        private readonly TimeRecordingDBContext _dbContext;

        public TimeRecordingService(TimeRecordingDBContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TargetTimeModelTimes? GetTargetTimeModelTimesForDate(DateTime date, Person person)
        {
            var personsTargetTime = _dbContext.PersonTargetTimeModel
                .Include(psm => psm.TargetTimeModel)
                .Where(psm => psm.Person == person && psm.ValidFrom <= date)
                .OrderByDescending(psm => psm.ValidFrom)
                .FirstOrDefault();
            return personsTargetTime == null ? null : _dbContext.TargetTimeModelTimes
                .Where(szmz => szmz.TargetTimeModelId == personsTargetTime.TargetTimeModelId && szmz.ValidFrom <= date)
                .OrderByDescending(szmz => szmz.ValidFrom)
                .FirstOrDefault();
        }

        public IList<Person> GetAllPersons()
        {
            return _dbContext.Person.ToList();
        }

        public int GetTargetTimeForMonth(DateTime month, Person person)
        {
            var targetTimeModelTimes = GetTargetTimeModelTimesForDate(month, person);
            int sum = 0;
            if (targetTimeModelTimes != null)
            {
                int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    var date = new DateTime(month.Year, month.Month, day);
                    sum += targetTimeModelTimes.ForDate(date);
                }
            }
            return sum;
        }

        public int GetMonthlyBalance(DateTime monat, Person person)
        {
            int daysInMonth = DateTime.DaysInMonth(monat.Year, monat.Month);
            int balance = 0;
            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(monat.Year, monat.Month, day);
                var workingTime = _dbContext.WorkingTime
                    .FirstOrDefault(a => a.PersonId == person.Id && a.Date == date);
                int minutes = workingTime?.Minutes ?? 0;
                balance += minutes;
            }
            return balance - GetTargetTimeForMonth(monat, person);
        }

        public void SaveWorkingTime(WorkingTime workingTime)
        {
            _dbContext.WorkingTime.Add(workingTime);
            _dbContext.SaveChanges();
        }

        public WorkingTime? GetWorkingTime(DateTime date, Person person)
        {
            return _dbContext.WorkingTime
                .FirstOrDefault(aw => aw.Date == date && aw.PersonId == person.Id);
        }

        private static string GetOutputCsvPath()
        {
            var parentDir = Directory.GetParent(Environment.CurrentDirectory)?.FullName;
            if (parentDir == null)
                throw new DirectoryNotFoundException("Parent directory not found.");

            var outputDir = Path.Combine(parentDir, "output");
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var filePath = Path.Combine(outputDir, "MonthReport.csv");
            return filePath;
        }

        public async Task ExportiereAlleZuCsvAsync()
        {
            var personen = await _dbContext.Person.ToListAsync();
            var arbeitszeiten = await _dbContext.WorkingTime.ToListAsync();
            var alleDaten = arbeitszeiten.Select(a => a.Date)
                .Concat(_dbContext.TargetTimeModelTimes.Select(sz => sz.ValidFrom))
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            using var writer = new StreamWriter(GetOutputCsvPath(), false, System.Text.Encoding.UTF8);
            await writer.WriteLineAsync("PersNr;Datum;TagesSollzeit;TagesArbeitszeit;TagesSaldo;GesamtSaldo");

            foreach (var person in personen)
            {
                int gesamtSaldo = 0;

                foreach (var datum in alleDaten)
                {
                    var sollzeitZeiten = GetTargetTimeModelTimesForDate(datum, person);
                    int tagesSollzeit = sollzeitZeiten?.ForDate(datum) ?? 0;

                    var arbeitszeit = arbeitszeiten.FirstOrDefault(a => a.PersonId == person.Id && a.Date == datum);
                    int arbeitszeitMinuten = arbeitszeit?.Minutes ?? 0;

                    int tagesSaldo = arbeitszeitMinuten - tagesSollzeit;
                    gesamtSaldo += tagesSaldo;

                    string sollzeitStr = tagesSollzeit.ToIndustryTime();
                    string arbeitszeitStr = arbeitszeitMinuten.ToIndustryTime();
                    string tagesSaldoStr = tagesSaldo.ToIndustryTime();
                    string gesamtSaldoStr = gesamtSaldo.ToIndustryTime();

                    await writer.WriteLineAsync($"{person.PersonnelNumber};{datum:yyyy-MM-dd};{sollzeitStr};{arbeitszeitStr};{tagesSaldoStr};{gesamtSaldoStr}");
                }
            }
        }
    }
}
