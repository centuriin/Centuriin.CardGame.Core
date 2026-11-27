using System.Reflection;

using Centuriin.CardGame.Core.Application.Initialization.Configuration;
using Centuriin.CardGame.Core.Application.Modulation;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Application.Tests.Modulation;

public sealed class GameModuleDiscoveryTests
{
    [Fact(DisplayName = "Can discover and construct a module using its public parameterless constructor.")]
    [Trait("Category", "Unit")]
    public void CanDiscoverModule()
    {
        // Arrange
        var assembly = typeof(FakeModule).Assembly;

        // Act
        var modules = GameModuleDiscovery.Discover([assembly]);

        // Assert
        modules.Should().ContainSingle().Which.Should().BeOfType<FakeModule>();
    }

    [Fact(DisplayName = "Can inspect each assembly once when it is supplied repeatedly.")]
    [Trait("Category", "Unit")]
    public void CanIgnoreRepeatedAssembly()
    {
        // Arrange
        var assembly = typeof(FakeModule).Assembly;

        // Act
        var modules = GameModuleDiscovery.Discover([assembly, assembly]);

        // Assert
        modules.Should().ContainSingle().Which.Should().BeOfType<FakeModule>();
    }

    [Fact(DisplayName = "Can ignore assemblies that contain no concrete game modules.")]
    [Trait("Category", "Unit")]
    public void CanIgnoreAssemblyWithoutModules()
    {
        // Arrange
        var assembly = typeof(IGameModule).Assembly;

        // Act
        var modules = GameModuleDiscovery.Discover([assembly]);

        // Assert
        modules.Should().BeEmpty();
    }

    [Fact(DisplayName = "Cannot discover modules without an assembly collection.")]
    [Trait("Category", "Unit")]
    public void CanNotDiscoverWithoutAssemblies()
    {
        // Act
        var exception = Record.Exception(() =>
            GameModuleDiscovery.Discover(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot discover a module without a public parameterless constructor.")]
    [Trait("Category", "Unit")]
    public void CanNotDiscoverModuleWithoutParameterlessConstructor()
    {
        // Arrange
        var module = Mock.Of<IGameModule>(MockBehavior.Strict);

        var assembly = new Mock<Assembly>(MockBehavior.Strict);
        assembly.Setup(x => x.GetHashCode())
            .Returns(1);
        assembly.Setup(x => x.GetTypes())
            .Returns([module.GetType()]);

        // Act
        var exception = Record.Exception(() =>
            GameModuleDiscovery.Discover([assembly.Object]));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    private sealed class FakeModule : IGameModule
    {
        public string Key => "fake";

        public IGameProfile GameProfile { get; } = Mock.Of<IGameProfile>(MockBehavior.Strict);

        public FakeModule() { }

        public void Configure(IConfigurableModuleDependencies dependencies) { }

        public void Configure(IConfigurableEntityInitializers initializers) { }
    }
}
