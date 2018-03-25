namespace DbHander
{
    public interface IProcComponantRepository : IGenericRepository<ProcComponent>
    {
        ProcComponent GetFirstRecord();
    }
}
