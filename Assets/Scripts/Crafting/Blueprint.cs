public class Blueprint
{
    public string itemName;

    public string Req1;
    public string Req2;

    public int Req1Amount;
    public int Req2Amount;

    public int numOfRequirements;

    public Blueprint(string itemName, int numOfRequirements, string Req1, int Req1Amount, string Req2, int Req2Amount)
    {
        this.itemName = itemName;

        this.numOfRequirements = numOfRequirements;

        this.Req1 = Req1;
        this.Req1Amount = Req1Amount;

        this.Req2 = Req2;
        this.Req2Amount = Req2Amount;
    }
}