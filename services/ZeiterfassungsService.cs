using Microsoft.EntityFrameworkCore;
using System.IO;
using Zeiterfassung.models;

namespace Zeiterfassung.services
{
    public class ZeiterfassungsService
    {
        private ZeiterfassungsDBContext _dbContext;

        public ZeiterfassungsService(ZeiterfassungsDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public SollzeitModelleZeiten? GetSollzeitZeitenFürDatum(DateTime datum, Person person)
        {
            var personenSollzeit = _dbContext.PersonenSollzeitModelle
                .Include(psm => psm.SollzeitModell)
                .Where(psm => psm.Person == person && psm.GueltigAb <= datum)
                .OrderByDescending(psm => psm.GueltigAb)
                .FirstOrDefault();
            return personenSollzeit == null ? null : _dbContext.SollzeitModelleZeiten
                .Where(szmz => szmz.SollzeitModellId == personenSollzeit.SollzeitModellId && szmz.GueltigAb <= datum)
                .OrderByDescending(szmz => szmz.GueltigAb)
                .FirstOrDefault();
        }

        public IList<Person> getAllPersonen()
        {
            return _dbContext.Personen.ToList();
        }

        public int GetMonatsSollzeiten(DateTime monat, Person person)
        {
            var sollzeitModelleZeiten = GetSollzeitZeitenFürDatum(monat, person);
            int summe = 0;
            if (sollzeitModelleZeiten != null)
            {
                int daysInMonth = DateTime.DaysInMonth(monat.Year, monat.Month);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    var date = new DateTime(monat.Year, monat.Month, day);
                    summe += sollzeitModelleZeiten.FürDatum(date);
                }
            }
            return summe;
        }

        public int GetMonatsSaldo(DateTime monat, Person person)
        {
            int daysInMonth = DateTime.DaysInMonth(monat.Year, monat.Month);
            int saldo = 0;
            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(monat.Year, monat.Month, day);
                var arbeitszeit = _dbContext.Arbeitszeit
                    .FirstOrDefault(a => a.PersonenId == person.Id && a.Datum == date);
                int minuten = arbeitszeit?.Minuten ?? 0;
                saldo += minuten;
            }
            return saldo - GetMonatsSollzeiten(monat, person);
        }

        public void SaveArbeitszeit(Arbeitszeit arbeitszeit)
        {
            _dbContext.Arbeitszeit.Add(arbeitszeit);
            _dbContext.SaveChanges();
        }

        public Arbeitszeit? GetArbeitszeit(DateTime datum, Person person)
        {
            return _dbContext.Arbeitszeit
                .FirstOrDefault(aw => aw.Datum == datum && aw.PersonenId == person.Id);
        }

        private static string FormatIndustrieZeit(int minuten)
        {
            return (minuten / 60.0).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture).Replace('.', ',');
        }

        private static string GetOutputCsvPath()
        {
            var parentDir = Directory.GetParent(Environment.CurrentDirectory)?.FullName;
            if (parentDir == null)
                throw new DirectoryNotFoundException("Parent directory not found.");

            var outputDir = Path.Combine(parentDir, "output");
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var filePath = Path.Combine(outputDir, "Zeitenkonto.csv");
            return filePath;
        }

        public async Task ExportiereAlleZuCsvAsync()
        {
            var personen = await _dbContext.Personen.ToListAsync();
            var arbeitszeiten = await _dbContext.Arbeitszeit.ToListAsync();
            var alleDaten = arbeitszeiten.Select(a => a.Datum)
                .Concat(_dbContext.SollzeitModelleZeiten.Select(sz => sz.GueltigAb))
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
                    var sollzeitZeiten = GetSollzeitZeitenFürDatum(datum, person);
                    int tagesSollzeit = sollzeitZeiten?.FürDatum(datum) ?? 0;

                    var arbeitszeit = arbeitszeiten.FirstOrDefault(a => a.PersonenId == person.Id && a.Datum == datum);
                    int arbeitszeitMinuten = arbeitszeit?.Minuten ?? 0;

                    int tagesSaldo = arbeitszeitMinuten - tagesSollzeit;
                    gesamtSaldo += tagesSaldo;

                    string sollzeitStr = FormatIndustrieZeit(tagesSollzeit);
                    string arbeitszeitStr = FormatIndustrieZeit(arbeitszeitMinuten);
                    string tagesSaldoStr = FormatIndustrieZeit(tagesSaldo);
                    string gesamtSaldoStr = FormatIndustrieZeit(gesamtSaldo);

                    await writer.WriteLineAsync($"{person.Personalnummer};{datum:yyyy-MM-dd};{sollzeitStr};{arbeitszeitStr};{tagesSaldoStr};{gesamtSaldoStr}");
                }
            }
        }
    }
}
