using Infrastructure.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivicConnect.Infrastructure.Tests
{
    public class ExternalLocationResolverTests
    {
        [Fact]
        public async Task ResolveAsync_EmptyAddress_ReturnsNull()
        {
            // Arrange
            var resolver = new ExternalLocationResolver();

            // Act
            var result = await resolver.ResolveAsync("");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ResolveAsync_WhitespaceAddress_ReturnsNull()
        {
            // Arrange
            var resolver = new ExternalLocationResolver();

            // Act
            var result = await resolver.ResolveAsync("   ");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ResolveAsync_ValidAddress_WithoutConfiguredProvider_ReturnsNull()
        {
            // Arrange
            var resolver = new ExternalLocationResolver();

            // Act
            var result = await resolver.ResolveAsync(
                "Pretoria, South Africa");

            // Assert
            Assert.Null(result);
        }
    }
}
