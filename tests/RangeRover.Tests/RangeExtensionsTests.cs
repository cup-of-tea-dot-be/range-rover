using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace RangeRover.Tests
{
    public sealed class RangeExtensionsTests
    {
        [Fact]
        public void Merge_SingleRange_ShouldReturnSameRange()
        {
            // Arrange
            var ranges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };
            Range<DateTime>[] expectedResult = ranges;

            // Act
            IEnumerable<Range<DateTime>> result = ranges.Combine();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Merge_TwoNonOverlappingRanges_ShouldReturnSameRanges()
        {
            // Arrange
            var ranges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
                new(DateTime.Parse("2021-04-02 10:00:00"), DateTime.Parse("2021-04-02 11:00:00")),
            };
            var expectedResult = ranges;

            // Act
            IEnumerable<Range<DateTime>> result = ranges.Combine();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Merge_TwoFullyOverlappingRanges_LargestRangeFirst_ShouldReturnMergedRange()
        {
            // Arrange
            var ranges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
                new(DateTime.Parse("2021-04-02 08:10:00"), DateTime.Parse("2021-04-02 08:20:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = ranges.Combine();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Merge_TwoFullyOverlappingRanges_SmallestRangeFirst_ShouldReturnMergedRange()
        {
            // Arrange
            var ranges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:10:00"), DateTime.Parse("2021-04-02 08:20:00")),
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = ranges.Combine();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Merge_TwoPartialOverlappingRanges_AscendingOrder_ShouldReturnMergedRange()
        {
            // Arrange
            var ranges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
                new(DateTime.Parse("2021-04-02 08:50:00"), DateTime.Parse("2021-04-02 09:20:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:20:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = ranges.Combine();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Merge_TwoPartialOverlappingRanges_DescendingOrder_ShouldReturnMergedRange()
        {
            // Arrange
            var ranges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:50:00"), DateTime.Parse("2021-04-02 09:20:00")),
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:20:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = ranges.Combine();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Merge_ConsecutiveRanges_DescendingOrder_ShouldReturnMergedRange()
        {
            // Arrange
            var ranges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 09:00:00"), DateTime.Parse("2021-04-02 10:00:00")),
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
                new(DateTime.Parse("2021-04-02 10:00:00"), DateTime.Parse("2021-04-02 11:00:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 11:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = ranges.Combine();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Merge_MultipleRanges_ShouldReturnMergedRanges()
        {
            // Arrange
            var ranges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:50:00"), DateTime.Parse("2021-04-02 09:20:00")),
                new(DateTime.Parse("2021-04-02 07:55:00"), DateTime.Parse("2021-04-02 09:10:00")),
                new(DateTime.Parse("2021-04-02 10:00:00"), DateTime.Parse("2021-04-02 11:00:00")),
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 07:55:00"), DateTime.Parse("2021-04-02 09:20:00")),
                new(DateTime.Parse("2021-04-02 10:00:00"), DateTime.Parse("2021-04-02 11:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = ranges.Combine();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Intersect_TwoNonOverlappingRanges_ShouldReturnEmpty()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 10:00:00"), DateTime.Parse("2021-04-02 11:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Intersect(secondRanges);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Intersect_TwoConsecutiveRanges_ShouldReturnEmptyEnumerable()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 09:00:00"), DateTime.Parse("2021-04-02 10:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Intersect(secondRanges);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Intersect_TwoRightOverlappingRanges_ShouldReturnIntersection()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:50:00"), DateTime.Parse("2021-04-02 09:20:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:50:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Intersect(secondRanges);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Intersect_TwoLeftOverlappingRanges_ShouldReturnIntersection()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 07:50:00"), DateTime.Parse("2021-04-02 08:10:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 08:10:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Intersect(secondRanges);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Intersect_TwoIdenticalOverlappingRanges_ShouldReturnIntersection()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Intersect(secondRanges);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Intersect_MultipleRanges_ShouldReturnIntersections()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:00:00")),
                new(DateTime.Parse("2021-04-02 10:00:00"), DateTime.Parse("2021-04-02 11:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 07:30:00"), DateTime.Parse("2021-04-02 08:10:00")),
                new(DateTime.Parse("2021-04-02 08:30:00"), DateTime.Parse("2021-04-02 10:30:00")),
                new(DateTime.Parse("2021-04-02 10:50:00"), DateTime.Parse("2021-04-02 11:30:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 08:10:00")),
                new(DateTime.Parse("2021-04-02 08:30:00"), DateTime.Parse("2021-04-02 09:00:00")),
                new(DateTime.Parse("2021-04-02 10:00:00"), DateTime.Parse("2021-04-02 10:30:00")),
                new(DateTime.Parse("2021-04-02 10:50:00"), DateTime.Parse("2021-04-02 11:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Intersect(secondRanges);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Intersect_FromOneBigRange_ShouldReturnIntersections()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 11:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 07:30:00"), DateTime.Parse("2021-04-02 08:10:00")),
                new(DateTime.Parse("2021-04-02 08:30:00"), DateTime.Parse("2021-04-02 10:30:00")),
                new(DateTime.Parse("2021-04-02 10:50:00"), DateTime.Parse("2021-04-02 11:30:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 08:10:00")),
                new(DateTime.Parse("2021-04-02 08:30:00"), DateTime.Parse("2021-04-02 10:30:00")),
                new(DateTime.Parse("2021-04-02 10:50:00"), DateTime.Parse("2021-04-02 11:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Intersect(secondRanges);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Intersect_ToOneBigRange_ShouldReturnIntersections()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 07:30:00"), DateTime.Parse("2021-04-02 08:10:00")),
                new(DateTime.Parse("2021-04-02 08:30:00"), DateTime.Parse("2021-04-02 10:30:00")),
                new(DateTime.Parse("2021-04-02 10:50:00"), DateTime.Parse("2021-04-02 11:30:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 11:00:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 08:10:00")),
                new(DateTime.Parse("2021-04-02 08:30:00"), DateTime.Parse("2021-04-02 10:30:00")),
                new(DateTime.Parse("2021-04-02 10:50:00"), DateTime.Parse("2021-04-02 11:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Intersect(secondRanges);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Subtract_OneLargeRangeFromSmallerRange_ShouldReturnEmptyEnumerable()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:30:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 10:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Subtract(secondRanges);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Subtract_IdenticalRange_ShouldReturnEmptyEnumerable()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 10:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 10:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Subtract(secondRanges);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Subtract_LeftOverlappingRange_ShouldReturnRightPart()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 10:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 09:50:00"), DateTime.Parse("2021-04-02 11:00:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 09:50:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Subtract(secondRanges);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Subtract_RightOverlappingRange_ShouldReturnLeftPart()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 10:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 07:45:00"), DateTime.Parse("2021-04-02 08:15:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:15:00"), DateTime.Parse("2021-04-02 10:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Subtract(secondRanges);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Subtract_OneSmallerRangeFromLargerRange_ShouldReturnOuterRanges()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 10:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:30:00"), DateTime.Parse("2021-04-02 09:00:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 08:30:00")),
                new(DateTime.Parse("2021-04-02 09:00:00"), DateTime.Parse("2021-04-02 10:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Subtract(secondRanges);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Subtract_MultipleRanges_ShouldReturnCorrectDifference()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 9:00:00")),
                new(DateTime.Parse("2021-04-02 10:00:00"), DateTime.Parse("2021-04-02 11:00:00")),
                new(DateTime.Parse("2021-04-02 12:00:00"), DateTime.Parse("2021-04-02 13:00:00")),
                new(DateTime.Parse("2021-04-02 14:00:00"), DateTime.Parse("2021-04-02 15:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:30:00"), DateTime.Parse("2021-04-02 08:45:00")),
                new(DateTime.Parse("2021-04-02 09:45:00"), DateTime.Parse("2021-04-02 10:15:00")),
                new(DateTime.Parse("2021-04-02 10:45:00"), DateTime.Parse("2021-04-02 12:15:00")),
                new(DateTime.Parse("2021-04-02 14:45:00"), DateTime.Parse("2021-04-02 15:15:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 08:30:00")),
                new(DateTime.Parse("2021-04-02 08:45:00"), DateTime.Parse("2021-04-02 09:00:00")),
                new(DateTime.Parse("2021-04-02 10:15:00"), DateTime.Parse("2021-04-02 10:45:00")),
                new(DateTime.Parse("2021-04-02 12:15:00"), DateTime.Parse("2021-04-02 13:00:00")),
                new(DateTime.Parse("2021-04-02 14:00:00"), DateTime.Parse("2021-04-02 14:45:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Subtract(secondRanges);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Subtract_OneLargeRangeIntoMultipleSlices_ShouldReturnCorrectDifference()
        {
            // Arrange
            var firstRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 12:00:00")),
            };
            var secondRanges = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:30:00"), DateTime.Parse("2021-04-02 08:45:00")),
                new(DateTime.Parse("2021-04-02 09:30:00"), DateTime.Parse("2021-04-02 09:45:00")),
                new(DateTime.Parse("2021-04-02 10:30:00"), DateTime.Parse("2021-04-02 11:45:00")),
            };
            var expectedResult = new Range<DateTime>[]
            {
                new(DateTime.Parse("2021-04-02 08:00:00"), DateTime.Parse("2021-04-02 08:30:00")),
                new(DateTime.Parse("2021-04-02 08:45:00"), DateTime.Parse("2021-04-02 09:30:00")),
                new(DateTime.Parse("2021-04-02 09:45:00"), DateTime.Parse("2021-04-02 10:30:00")),
                new(DateTime.Parse("2021-04-02 11:45:00"), DateTime.Parse("2021-04-02 12:00:00")),
            };

            // Act
            IEnumerable<Range<DateTime>> result = firstRanges.Subtract(secondRanges);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
