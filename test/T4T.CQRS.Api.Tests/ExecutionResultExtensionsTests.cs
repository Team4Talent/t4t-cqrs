using System.Net;
using Microsoft.AspNetCore.Mvc;
using T4T.CQRS.Execution;
using Xunit;

namespace T4T.CQRS.Api.Tests
{
    public class ExecutionResultExtensionsTests
    {
        private const string ErrorMessage = "Something went wrong.";

        [Fact]
        public void InternalServerError_returns_an_InternalServerError_Result()
        {
            var queryResult = new ExecutionResult();
            queryResult.AddError(ErrorMessage, ExecutionErrorType.InternalServerError);

            var sut = queryResult.ToActionResult() as ObjectResult;

            Assert.NotNull(sut);
            Assert.Equal((int) HttpStatusCode.InternalServerError, sut.StatusCode);
            Assert.Same(queryResult, sut.Value);
        }

        [Fact]
        public void BadRequest_returns_a_BadRequest_Result()
        {
            var queryResult = new ExecutionResult();
            queryResult.AddError(ErrorMessage, ExecutionErrorType.BadRequest);

            var sut = queryResult.ToActionResult() as ObjectResult;

            Assert.NotNull(sut);
            Assert.Equal((int) HttpStatusCode.BadRequest, sut.StatusCode);
            Assert.Same(queryResult, sut.Value);
        }

        [Fact]
        public void NotFound_returns_a_NotFound_Result()
        {
            var queryResult = new ExecutionResult();
            queryResult.AddError(ErrorMessage, ExecutionErrorType.NotFound);

            var sut = queryResult.ToActionResult() as ObjectResult;

            Assert.NotNull(sut);
            Assert.Equal((int) HttpStatusCode.NotFound, sut.StatusCode);
            Assert.Same(queryResult, sut.Value);
        }

        [Fact]
        public void Forbidden_returns_a_Forbidden_Result()
        {
            var queryResult = new ExecutionResult();
            queryResult.AddError(ErrorMessage, ExecutionErrorType.Forbidden);

            var sut = queryResult.ToActionResult() as ObjectResult;

            Assert.NotNull(sut);
            Assert.Equal((int) HttpStatusCode.Forbidden, sut.StatusCode);
            Assert.Same(queryResult, sut.Value);
        }

        [Fact]
        public void Unauthorized_returns_an_Unauthorized_Result()
        {
            var queryResult = new ExecutionResult();
            queryResult.AddError(ErrorMessage, ExecutionErrorType.Unauthorized);

            var sut = queryResult.ToActionResult() as ObjectResult;

            Assert.NotNull(sut);
            Assert.Equal((int) HttpStatusCode.Unauthorized, sut.StatusCode);
            Assert.Same(queryResult, sut.Value);
        }

        [Fact]
        public void Success_returns_an_Ok_Result()
        {
            var queryResult = ExecutionResult.Succeeded();

            var sut = queryResult.ToActionResult() as OkObjectResult;

            Assert.NotNull(sut);
            Assert.Equal((int) HttpStatusCode.OK, sut.StatusCode);
            Assert.Same(queryResult, sut.Value);
        }

        [Fact]
        public void ToCreatedResult_with_an_OkObjectResult_returns_a_Created_Result()
        {
            const string location = "http://t4t.rocks/1";

            var queryResult = ExecutionResult.Succeeded();
            var sut = queryResult.ToCreatedResult(location, "id") as CreatedResult;

            Assert.NotNull(sut);
            Assert.Equal((int) HttpStatusCode.Created, sut.StatusCode);
            
            Assert.Equal(sut.Location, location);
        }

        [Fact]
        public void ToCreatedResult_with_an_ObjectResult_returns_the_ObjetResult()
        {
            var queryResult = new ExecutionResult();
            queryResult.AddError(ErrorMessage, ExecutionErrorType.Unauthorized);

            var sut = queryResult.ToCreatedResult("location", "id") as ObjectResult;

            Assert.NotNull(sut);
            Assert.Equal((int) HttpStatusCode.Unauthorized, sut.StatusCode);
        }
    }
}
