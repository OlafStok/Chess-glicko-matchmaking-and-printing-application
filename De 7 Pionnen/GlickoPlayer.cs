using System;

namespace WindowsFormsApp2
{
    [Serializable()]
    public class GlickoPlayer
    {
        private double glickoConversion = 173.7178;

        public GlickoPlayer(double rating = 1500.0, double ratingDeviation = 350.0, double volatility = 0.06)
        {
            this.Rating = rating;
            this.RatingDeviation = ratingDeviation;
            this.Volatility = volatility;
        }

        public GlickoPlayer()
        {
            this.Rating = 1500.0;
            this.RatingDeviation = 350.0;
            this.Volatility = 0.06;
        }

        public string Name { get; set; }

        public double Rating { get; set; }

        public double RatingDeviation { get; set; }

        public double Volatility { get; set; }

        public double GlickoRating
        {
            get
            {
                return (this.Rating - 1500.0) / this.glickoConversion;
            }
        }

        public double GlickoRatingDeviation
        {
            get
            {
                return this.RatingDeviation / this.glickoConversion;
            }
        }

        public double GPhi
        {
            get
            {
                return 1.0 / Math.Sqrt(1.0 + 3.0 * Math.Pow(this.GlickoRatingDeviation, 2.0) / Math.Pow(Math.PI, 2.0));
            }
        }

        public Glicko2.GlickoPlayer GetOriginalGlickoPlayer()
        {
            return new Glicko2.GlickoPlayer(this.Rating, this.RatingDeviation, this.Volatility);
        }
    }

}