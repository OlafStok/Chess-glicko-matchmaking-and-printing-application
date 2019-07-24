using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_7_Pionnen
{
    class DataSources
    {
        private static readonly DataSources instance = new DataSources();
        public List<Persoon> personen = (List<Persoon>)Persistentie.Laad("personen") ?? new List<Persoon>();
        public List<MatchLijst> matchLijsten = (List<MatchLijst>)Persistentie.Laad("matchLijsten") ?? new List<MatchLijst>();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static DataSources()
        {
        }

        public int GenereerId()
        {
            int id = -1;
            foreach (Persoon p in DataSources.instance.personen)
            {
                if (p.Id > id)
                    id = p.Id;
            }
            return id + 1;
        }

        public int GenereerToernamentNummer()
        {
            int toernamentNummer = -1;
            foreach (MatchLijst t in DataSources.instance.matchLijsten)
            {
                if (t.nummer > toernamentNummer)
                    toernamentNummer = t.nummer;
            }
            return toernamentNummer + 1;
        }

        private DataSources()
        {
        }

        public static DataSources Instance
        {
            get
            {
                instance.personen.Sort(delegate (Persoon p1, Persoon p2)
                {
                    if (p1.glicko.Rating.CompareTo(p2.glicko.Rating) == 0)
                    {
                        return -p1.Id.CompareTo(p2.Id);
                    }
                    return -p1.glicko.Rating.CompareTo(p2.glicko.Rating);
                });

                int scoreTeller = 0;

                for (var i = 0; i < instance.personen.Count; i++)
                {
                    float hogerePersoonScore;
                    float lagerePersoonScore;
                    hogerePersoonScore = i <= 0 ? float.MaxValue : instance.personen[i - 1].Score;

                    if (instance.personen[i].Score == hogerePersoonScore)
                        instance.personen[i].Positie = instance.personen[i - 1].Positie;

                    else
                    {
                        scoreTeller++;
                        instance.personen[i].Positie = scoreTeller;
                    }
                        
                }

                instance.matchLijsten.Sort(delegate (MatchLijst m1, MatchLijst m2)
                {
                    return m1.nummer.CompareTo(m2.nummer);
                });
                return instance;
            }
        }
    }
}