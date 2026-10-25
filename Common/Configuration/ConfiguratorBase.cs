namespace Centuriin.CardGame.Core.Common.Configuration;

public abstract class ConfiguratorBase : IConfigurator
{
    private bool _isConfigured;

    public void Configure()
    {
        ThrowIfConfigured();

        ConfigureCore();

        _isConfigured = true;
    }

    protected void ThrowIfConfigured()
    {
        if (_isConfigured)
        {
            throw new InvalidOperationException();
        }
    }

    protected abstract void ConfigureCore();
}
