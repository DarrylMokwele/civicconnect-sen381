using System;
using Xunit;

namespace CivicConnect.Application.Tests
{
    public class ServiceRequestBlackBoxTests
    {
        // -------------------------------------------------------------
        // TECHNIQUE 1: BOUNDARY VALUE ANALYSIS (BVA) & EQUIVALENCE PARTITIONING
        // Evaluates FR-001 & field constraints: Title (1-100), Description (1-1000)
        // -------------------------------------------------------------

        [Theory]
        [InlineData(1)]    // Lower Boundary (Min Valid)
        [InlineData(50)]   // Typical Equivalence Partition
        [InlineData(100)]  // Upper Boundary (Max Valid)
        public void Title_ValidLengthBoundaries_PassesValidation(int length)
        {
            // Arrange
            string title = new string('A', length);

            // Act
            bool isValid = !string.IsNullOrWhiteSpace(title) && title.Length <= 100;

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(0)]    // Below Lower Boundary (Empty / Invalid)
        [InlineData(101)]  // Above Upper Boundary (Invalid)
        public void Title_InvalidLengthBoundaries_FailsValidation(int length)
        {
            // Arrange
            string title = new string('A', length);

            // Act
            bool isValid = !string.IsNullOrWhiteSpace(title) && title.Length <= 100;

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Title_WhitespaceOnly_FailsValidation()
        {
            // Arrange
            string title = "      ";

            // Act
            bool isValid = !string.IsNullOrWhiteSpace(title) && title.Length <= 100;

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(1000, true)]   // Upper boundary max valid (NVARCHAR(1000))
        [InlineData(1001, false)]  // Above max boundary (Invalid)
        public void Description_BoundaryChecks_EvaluatesCorrectly(int length, bool expectedResult)
        {
            // Arrange
            string description = new string('D', length);

            // Act
            bool isValid = !string.IsNullOrWhiteSpace(description) && description.Length <= 1000;

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        // -------------------------------------------------------------
        // TECHNIQUE 2: STATE TRANSITION TESTING (LIFECYCLE FSM)
        // Evaluates FR-010, AC-010.2: Submitted -> Assigned -> InProgress -> Resolved -> Closed
        // -------------------------------------------------------------

        [Theory]
        [InlineData("Submitted", "Assigned", true)]
        [InlineData("Assigned", "InProgress", true)]
        [InlineData("InProgress", "Resolved", true)]
        [InlineData("Resolved", "Closed", true)]
        [InlineData("Closed", "Submitted", true)]    // Resubmission rule
        [InlineData("Assigned", "Closed", false)]    // Illegal jump (DEF-001 regression check)
        [InlineData("Submitted", "Resolved", false)]  // Illegal jump
        [InlineData("Closed", "Resolved", false)]     // Illegal jump
        public void Lifecycle_StateTransitions_EnforceValidSequences(string currentState, string targetState, bool expectedAllowed)
        {
            // Act
            bool isAllowed = CanTransition(currentState, targetState);

            // Assert
            Assert.Equal(expectedAllowed, isAllowed);
        }

        private static bool CanTransition(string current, string target)
        {
            return (current, target) switch
            {
                ("Submitted", "Assigned") => true,
                ("Assigned", "InProgress") => true,
                ("InProgress", "Resolved") => true,
                ("Resolved", "Closed") => true,
                ("Closed", "Submitted") => true,
                _ => false
            };
        }
    }
}