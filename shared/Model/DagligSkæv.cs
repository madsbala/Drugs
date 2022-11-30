namespace shared.Model;

public class DagligSkæv : Ordination {
    public List<Dosis> doser { get; set; } = new List<Dosis>();

    public DagligSkæv(DateTime startDen, DateTime slutDen, Laegemiddel laegemiddel) : base(laegemiddel, startDen, slutDen) {
	}

    public DagligSkæv(DateTime startDen, DateTime slutDen, Laegemiddel laegemiddel, Dosis[] doser) : base(laegemiddel, startDen, slutDen) {
        this.doser = doser.ToList();
    }    

    public DagligSkæv() : base(null!, new DateTime(), new DateTime()) {
    }

	public void opretDosis(DateTime tid, double antal) {
        doser.Add(new Dosis(tid, antal));
    }

	public override double samletDosis() {
		return base.antalDage() * doegnDosis();
	}
	//samlet dosis, talt sammen af døgndosis ganget sammen af antal dage

	public override double doegnDosis()
	{
		double sum = 0;
		for (int i = 0; i < doser.Count(); i++)
		{
			sum = doser[i].antal;
		}
		return sum;
	}
	//en sum af den samlede antal doser på en dag, talt sammen i et for-loop


	public override String getType() {
		return "DagligSkæv";
	}
}
