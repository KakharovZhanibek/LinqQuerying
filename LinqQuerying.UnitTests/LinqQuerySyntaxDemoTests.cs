using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqQuerying.UnitTests
{
    [TestClass]
    public class LinqQuerySyntaxDemoTests
    {
        [TestMethod]
        public void SelectQueryDemo_I_Test_I()
        {
            // AAA - Arrange, Act, Assert
            // Arrange
            var testData = new int[] { 2, 5, 10, 12 };
            var expcetedOutputData = new int[] { 4, 25, 100, 144 };

            // Act
            var outputData = LinqQuerySyntaxDemo
                .SelectQueryDemo_I(testData)
                .ToArray();

            // Assert
            Assert.IsTrue(expcetedOutputData.Length == 
                outputData.Count());

            for (int i = 0; i < testData.Length; i++)
            {
                Assert.IsTrue(expcetedOutputData[i] == outputData[i]);
            }
        }
    }
}
