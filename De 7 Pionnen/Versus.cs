using System;

namespace De_7_Pionnen
{
    [Serializable()]
    public class Versus
    {
        public int Id { get; set; }
        private int WitId { get; set; }
        private int ZwartId { get; set; }
        public string Uitslag { get; set; }
        public Persoon Wit
        {
            get { return DataSources.Instance.personen.Find(P => P.Id == WitId); }
            set { WitId = value == null ? -1 : value.Id; }
        }
        public Persoon Zwart
        {
            get { return DataSources.Instance.personen.Find(P => P.Id == ZwartId); }
            set { ZwartId = value == null ? -1 : value.Id; }
        }

        public override string ToString()
        {
            return DataSources.Instance.personen.Find(P => P.Id == WitId).Naam + " - " + DataSources.Instance.personen.Find(P => P.Id == ZwartId).Naam;
        }
    }
}
