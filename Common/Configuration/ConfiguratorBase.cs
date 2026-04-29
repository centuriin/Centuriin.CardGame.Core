namespace Centuriin.CardGame.Core.Common.Configuration;

public abstract class ConfiguratorBase : IConfigurator
{
    private bool _isInitialized;

    public void Setup()
    {
        ThrowIfInitialized();

        SetupCore();

        _isInitialized = true;
    }

    protected abstract void SetupCore();

    protected void ThrowIfInitialized()
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException();
        }
    }
}
