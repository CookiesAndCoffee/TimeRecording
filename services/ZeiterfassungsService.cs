using Microsoft.EntityFrameworkCore;
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
    }
}
