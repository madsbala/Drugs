namespace ordination_test;

using Microsoft.EntityFrameworkCore;

using Service;
using Data;
using static shared.Util;
using shared.Model;

[TestClass]
public class ServiceTest
{
    private DataService service;

    [TestInitialize]
    public void SetupBeforeEachTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: "test-database");
        var context = new OrdinationContext(optionsBuilder.Options);
        service = new DataService(context);
        service.SeedData();
    }

    [TestMethod]
    public void PatientsExist()
    {
        Assert.IsNotNull(service.GetPatienter());
    }

    [TestMethod]
    //opretter en daglig fast som planlagt
    public void OpretDagligFast()
    {
        Patient patient1 = service.GetPatienter().First();
        Laegemiddel lm1 = service.GetLaegemidler().First();

        Assert.AreEqual(1, service.GetDagligFaste().Count());

        service.OpretDagligFast(patient1.PatientId, lm1.LaegemiddelId,
            2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligFaste().Count());
    }

    [TestMethod]
    //skal fejle fordi der gives -10 dage
    public void OpretDagligFastDerFejler()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(2, service.GetDagligFaste().Count());

        service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(-10));

        Assert.AreEqual(3, service.GetDagligFaste().Count());

    }

    
    [TestMethod]
    //opretter en daglig skæv som planlagt
    public void OpretDagligSkaev()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(1, service.GetDagligSkæve().Count());

        service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId,
            new Dosis[] {
                new Dosis(CreateTimeOnly(12, 0, 0), 0.5),
                new Dosis(CreateTimeOnly(12, 40, 0), 1),
                new Dosis(CreateTimeOnly(16, 0, 0), 2.5),
                new Dosis(CreateTimeOnly(18, 45, 0), 3)
            }, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligSkæve().Count());
    }

    
    [TestMethod]
    //skal fejle, da lægemiddel er null
    public void OpretDagligSkaevDerFejler()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = null;

        service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId,
            new Dosis[] {
                new Dosis(CreateTimeOnly(12, 0, 0), 0.5),
                new Dosis(CreateTimeOnly(12, 40, 0), 1),
                new Dosis(CreateTimeOnly(16, 0, 0), 2.5),
                new Dosis(CreateTimeOnly(18, 45, 0), 3)
            }, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligSkæve().Count());
    }

    
    [TestMethod]
    //opretter en gyldig PN
    public void OpretPN()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(4, service.GetPNs().Count());

        service.OpretPN(patient.PatientId, lm.LaegemiddelId, 420, DateTime.Now, DateTime.Now.AddDays(69));

        Assert.AreEqual(5, service.GetPNs().Count());
    }


    [TestMethod]

    public void OpretPNDerFejler()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = null;

        Assert.AreEqual(5, service.GetPNs().Count());

        service.OpretPN(patient.PatientId, lm.LaegemiddelId, 420, DateTime.Now, DateTime.Now.AddDays(69));

        Assert.AreEqual(6, service.GetPNs().Count());
    }

    [TestMethod]
    public void AnvendOrdination()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();
        PN pn = service.GetPNs().First();

        Dato dato = new Dato();

        dato.dato = new DateTime(2020, 10, 20);
        Assert.AreEqual("dosis ikke givet", service.AnvendOrdination(1, dato));

        dato.dato = new DateTime(2021, 01, 05);
        Assert.AreEqual("dosis givet", service.AnvendOrdination(1, dato));

    }

    [TestMethod]
    //Den skal fejle
    public void GetAnbefaletDosisPerDøgn()
    {
        double patientvægt = service.GetAnbefaletDosisPerDøgn(3, 2);
        //Giver ikke korrekt resultat pga. double 134.10000002
        Assert.AreEqual(134.1,patientvægt,1.0);
        Assert.AreEqual(1.536, patientvægt);



    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestAtKodenSmiderEnException()
    {
        // Herunder skal man så kalde noget kode,
        // der smider en exception.

        // Hvis koden _ikke_ smider en exception,
        // så fejler testen.

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }
}
