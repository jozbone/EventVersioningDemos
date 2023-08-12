namespace Consumer1;

internal class InvoicePostedV2
{
    public string? InvoiceNumber { get; set; }
    public PurchaseOrder PurchaseOrder { get; set; }
    public string Submitter { get; set; }
}

internal class PurchaseOrder
{
    public int Number { get; set; }
    public string Type { get; set; }
}