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
            try
            {
                string wit = "", zwart = "";
                if (DataSources.Instance.personen.Find(P => P.Id == WitId).Naam != null)
                    wit = DataSources.Instance.personen.Find(P => P.Id == WitId).Naam;

                if (DataSources.Instance.personen.Find(P => P.Id == ZwartId).Naam != null)
                    zwart = DataSources.Instance.personen.Find(P => P.Id == ZwartId).Naam;

                return wit + " - " + zwart;
            } catch (Exception e)
            {
                return "";
            }
        }
    }
}
