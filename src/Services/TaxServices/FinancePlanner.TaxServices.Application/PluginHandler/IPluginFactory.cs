namespace FinancePlanner.TaxServices.Application.PluginHandler
{
    public interface IPluginFactory
    {
        T GetService<T>(string w4Type);
        void Initialize();
    }
}
