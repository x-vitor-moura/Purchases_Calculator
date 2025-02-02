using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Purchases_Calculator.API.Domain;

public class Purchase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime PurchaseRegistrationDate { get; }
    public Purchase(decimal net, decimal gross, decimal vat, int vatRate)
    {
        Gross = gross;
        Net = net;
        Vat = vat;
        VatRate = vatRate;
        PurchaseRegistrationDate = DateTime.UtcNow;
    }

    public decimal Gross { get; set; }
    public decimal Net { get; set; }
    public decimal Vat { get; set; }
    public int VatRate { get; set; }

}
