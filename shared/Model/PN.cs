namespace shared.Model;

public class PN : Ordination {
	public double antalEnheder { get; set; }
    public List<Dato> dates { get; set; } = new List<Dato>();

    public PN (DateTime startDen, DateTime slutDen, double antalEnheder, Laegemiddel laegemiddel) : base(laegemiddel, startDen, slutDen) {
		this.antalEnheder = antalEnheder;
	}

    public PN() : base(null!, new DateTime(), new DateTime()) {
    }

    /// <summary>
    /// Registrerer at der er givet en dosis på dagen givesDen
    /// Returnerer true hvis givesDen er inden for ordinationens gyldighedsperiode og datoen huskes
    /// Returner false ellers og datoen givesDen ignoreres
    /// </summary>
    public bool givDosis(Dato givesDen) {
        if (givesDen.dato >= startDen && givesDen.dato <= slutDen)
        {
            dates.Add(givesDen);
            return true;
        }
        return false;
    }

    public override double doegnDosis() {
        double sum = 0;

        //laver et tjek på at dates-listen ikke er tom
        if (dates.Count > 0)
            
        {
                //datoer sættes begge til den første, og så bliver de sorteret efterfølgende
            DateTime min = dates.First().dato;
            DateTime max = dates.First().dato;
                
                //finder højeste og laveste dato 
            foreach (Dato d in dates)
            {
                if (d.dato < min)
                {
                    min = d.dato;
                }

                if (d.dato > max)
                {
                    max = d.dato;
                }
            }

                //laver aritmetik for at finde antal dage imellem laveste og højeste dato
            int dage = (int)(max - min).TotalDays + 1;

                //finder gennemsnit af dosis på antal dage
            sum = samletDosis() / dage;
        }
        return sum;
    }


    public override double samletDosis()
    {
        return dates.Count() * antalEnheder;
    }

    public int getAntalGangeGivet()
    {
        return dates.Count();
    }

	public override String getType()
    {
		return "PN";
	}
}
