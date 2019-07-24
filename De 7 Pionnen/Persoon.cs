using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2;

namespace De_7_Pionnen
{
    [Serializable()]
    public class Persoon
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public int Positie { get; set; }
        public int OudePositie;
        public int Stijging { get; set; }
        public int Gespeeld { get; set; }
        public int Gewonnen { get; set; }
        public int Verloren { get; set; }
        public int Gelijkspel { get; set; }
        public float Score { get; set; }
        public bool Aanwezig { get; set; }

        public List<Persoon> vorigeTegenstanders = new List<Persoon>();
        public GlickoPlayer glicko { get; set; } = new GlickoPlayer();


        public Persoon()
        {
        }

        public Persoon(int id, string naam, int positie, int stijging, int gespeeld, int gewonnen, int verloren, int gelijkspel, float score, bool aanwezig, GlickoPlayer glicko)
        {
            Id = id;
            Naam = naam;
            Positie = positie;
            Stijging = stijging;
            Gespeeld = gespeeld;
            Gewonnen = gewonnen;
            Verloren = verloren;
            Gelijkspel = gelijkspel;
            Score = score;
            Aanwezig = aanwezig;
            this.glicko = glicko;
        }

        public Persoon(int id, string naam)
        {
            this.Id = id;
            this.Naam = naam;
            Aanwezig = true;
        }

        public override string ToString()
        {
            return Naam + " - " + Id;
        }
    }
}
