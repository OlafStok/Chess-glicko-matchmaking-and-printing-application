using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_7_Pionnen
{
    [Serializable()]
    public class MatchLijst
    {
        public List<Versus> versuses = new List<Versus>();
        public int nummer;
        public bool huidigeLijst = true;

        public MatchLijst() { }

        public MatchLijst(List<Versus> versuses, int nummer)
        {
            this.versuses = versuses;
            this.nummer = nummer;
        }

        public static MatchLijst GenereerMatchLijst()
        {
            List<Versus> versusLijst = new List<Versus>();
            List<Persoon> aanwezigePersonen = new List<Persoon>();
            foreach (Persoon p in DataSources.Instance.personen)
            {
                if (p.Aanwezig && p.Id > -1)
                    aanwezigePersonen.Add(p);
            }

            //match personen op met elkaar
            Random random = new Random();
            while (aanwezigePersonen.Count > 1)
            {
                Debug.WriteLine("aanwezige personen: " + aanwezigePersonen.Count + " - persoon 1: " + aanwezigePersonen[0].Naam);
                Versus versus = new Versus();
                int rand = random.Next(0, 101);
                int randIndex = rand >= 55 ? 0 : 0 + 1;
                Persoon p1 = new Persoon(), p2 = new Persoon();
                p1 = aanwezigePersonen[0];
                for (int j = 1; j < aanwezigePersonen.Count; j++)
                {
                    bool vorigeTegenstander = false;
                    foreach (Persoon p in p1.vorigeTegenstanders)
                    {
                        if (aanwezigePersonen[j].Id == p.Id)
                            vorigeTegenstander = true;
                    }
                    if (!vorigeTegenstander) {

                        p2 = aanwezigePersonen[j];
                        aanwezigePersonen.Remove(aanwezigePersonen.Find(persoon => persoon.Id == p1.Id));
                        aanwezigePersonen.Remove(aanwezigePersonen.Find(persoon => persoon.Id == p2.Id));
                        break;
                    } else
                    {
                        if (aanwezigePersonen.Count <= 4) {
                            foreach (Persoon p in p1.vorigeTegenstanders)
                            {
                                if (p.Id == aanwezigePersonen[j].Id)
                                    p2 = p;
                            }
                            aanwezigePersonen.Remove(aanwezigePersonen.Find(persoon => persoon.Naam.Equals(p1.Naam)));
                            aanwezigePersonen.Remove(aanwezigePersonen.Find(persoon => persoon.Naam.Equals(p2.Naam)));
                            break;
                        }
                    }
                }

                versus.Id = versusLijst.Count;
                versus.Wit = randIndex == 1 ? p2 : p1;
                versus.Zwart = versus.Wit == p1 ? p2 : p1;
                versusLijst.Add(versus);
                Debug.WriteLine("Versus added!");
            }

            MatchLijst matchLijst = new MatchLijst(versusLijst, DataSources.Instance.GenereerToernamentNummer());

            DataSources.Instance.matchLijsten.Add(matchLijst);
            Persistentie.Opslaan(DataSources.Instance.matchLijsten, "matchLijsten");

            return matchLijst;
        }
    }
}
