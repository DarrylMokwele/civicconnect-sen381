using Application.ServiceRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivicConnect.Application.Tests
{
    public class StatusTransitionPolicyTests
    {
        [Fact]
        public void CanTransition_SubmittedToAssigned_ReturnsTrue()
        {
            // Arrange
            var policy = new StatusTransitionPolicy();

            // Act
            var result = policy.CanTransition(
                "Submitted",
                "Assigned");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanTransition_SubmittedToClosed_ReturnsFalse()
        {
            // Arrange
            var policy = new StatusTransitionPolicy();

            // Act
            var result = policy.CanTransition(
                "Submitted",
                "Closed");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanTransition_AssignedToInProgress_ReturnsTrue()
        {
            var policy = new StatusTransitionPolicy();

            var result = policy.CanTransition(
                "Assigned",
                "In Progress");

            Assert.True(result);
        }

        [Fact]
        public void CanTransition_InProgressToResolved_ReturnsTrue()
        {
            var policy = new StatusTransitionPolicy();

            var result = policy.CanTransition(
                "In Progress",
                "Resolved");

            Assert.True(result);
        }

        [Fact]
        public void CanTransition_ResolvedToClosed_ReturnsTrue()
        {
            var policy = new StatusTransitionPolicy();

            var result = policy.CanTransition(
                "Resolved",
                "Closed");

            Assert.True(result);
        }

        [Fact]
        public void CanTransition_ClosedToSubmitted_ReturnsFalse()
        {
            var policy = new StatusTransitionPolicy();

            var result = policy.CanTransition(
                "Closed",
                "Submitted");

            Assert.False(result);
        }
    }
}
