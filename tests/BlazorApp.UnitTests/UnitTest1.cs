using BlazorApp.Pages;
using System;
using Bunit;
using NUnit.Framework;

namespace BlazorApp.UnitTests
{

    public class Tests : BunitTestContext
    {
        [Test]
        public void HelloWorldComponentRendersCorrectly()
        {
            // Arrange

            // Act
            var cut = RenderComponent<Pages.Index>();

            // Assert
            cut.MarkupMatches("<h1>Hello, world!</h1>");
        }
    }
}