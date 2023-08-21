namespace Producer;

internal class InvoicePostedV2
{
    public string? InvoiceNumber { get; set; }
    public int? PurchaseOrderNumber { get; set; }  // breaking change due to the name changing
    public string? submitter { get; set; }
}