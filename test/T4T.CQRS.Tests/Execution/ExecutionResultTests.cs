using System;
using System.Collections.Generic;
using T4T.CQRS.Execution;
using Xunit;

namespace T4T.CQRS.Tests.Execution
{
    public class ExecutionResultTests
    {
        [Fact]
        public void Succeeded_creates_an_ExecutionResult_with_Success()
        {
            var sut = ExecutionResult.Succeeded();

            Assert.True(sut.Success);
            Assert.Empty(sut.Errors);
            Assert.Empty(sut.Warnings);
        }

        [Fact]
        public void NotFoundAsWarning_creates_an_ExecutionResult_with_Success_and_a_warning()
        {
            var sut = ExecutionResult.NotFoundAsWarning();

            Assert.True(sut.Success);
            Assert.Empty(sut.Errors);
            Assert.NotEmpty(sut.Warnings);
        }

        [Fact]
        public void NotFoundAsError_creates_an_ExecutionResult_with_errors()
        {
            var sut = ExecutionResult.NotFoundAsError();

            Assert.False(sut.Success);
            Assert.NotEmpty(sut.Errors);
            Assert.Empty(sut.Warnings);

            Assert.True(sut.Errors[0].Type == ExecutionErrorType.NotFound);
        }

        [Fact]
        public void BadRequest_creates_an_ExecutionResult_with_errors()
        {
            var sut = ExecutionResult.BadRequest();

            Assert.False(sut.Success);
            Assert.NotEmpty(sut.Errors);
            Assert.Empty(sut.Warnings);

            Assert.True(sut.Errors[0].Type == ExecutionErrorType.BadRequest);
        }

        [Fact]
        public void Unauthorized_creates_an_ExecutionResult_with_errors()
        {
            var sut = ExecutionResult.Unauthorized();

            Assert.False(sut.Success);
            Assert.NotEmpty(sut.Errors);
            Assert.Empty(sut.Warnings);

            Assert.True(sut.Errors[0].Type == ExecutionErrorType.Unauthorized);
        }

        [Fact]
        public void Forbidden_creates_an_ExecutionResult_with_errors()
        {
            var sut = ExecutionResult.Forbidden();

            Assert.False(sut.Success);
            Assert.NotEmpty(sut.Errors);
            Assert.Empty(sut.Warnings);

            Assert.True(sut.Errors[0].Type == ExecutionErrorType.Forbidden);
        }

        public static IEnumerable<object[]> GetExceptionsAndExecutionErrorTypes()
        {
            yield return new object[] {new InvalidOperationException(), ExecutionErrorType.BadRequest};
            yield return new object[] {new ArgumentException(), ExecutionErrorType.BadRequest};
            yield return new object[] {new UnauthorizedAccessException(), ExecutionErrorType.Unauthorized};
            yield return new object[] {new NullReferenceException(), ExecutionErrorType.InternalServerError};
        }

        [Theory]
        [MemberData(nameof(GetExceptionsAndExecutionErrorTypes))]
        public void FromException_creates_an_ExecutionResult_with_an_appropriate_Error(
            Exception exception, 
            ExecutionErrorType type)
        {
            var sut = ExecutionResult.FromException(exception);

            Assert.False(sut.Success);
            Assert.NotEmpty(sut.Errors);
            Assert.Empty(sut.Warnings);

            Assert.True(sut.Errors[0].Type == type);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class ExecutionResultDerivedType : ExecutionResult
        { }

        [Fact]
        public void As_casts_the_ExecutionError_to_the_Derived_Type()
        {
            var result = ExecutionResult.Succeeded();
            var sut = result.As<ExecutionResultDerivedType>();

            Assert.IsType<ExecutionResultDerivedType>(sut);
            Assert.True(sut.Success);
        }

        [Fact]
        public void AddWarning_adds_a_warning()
        {
            var sut = ExecutionResult.Succeeded();

            Assert.True(sut.Success);
            Assert.Empty(sut.Warnings);

            sut.AddWarning("That's some warning you've got there.");

            Assert.True(sut.Success);
            Assert.NotEmpty(sut.Warnings);
        }

        [Fact]
        public void AddError_adds_an_error()
        {
            var sut = ExecutionResult.Succeeded();

            Assert.True(sut.Success);
            Assert.Empty(sut.Errors);

            sut.AddError("That's some error you've got there.", ExecutionErrorType.InternalServerError);

            Assert.False(sut.Success);
            Assert.NotEmpty(sut.Errors);
        }
    }
}
