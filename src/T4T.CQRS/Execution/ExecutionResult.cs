using System;
using System.Collections.Generic;
using System.Linq;

namespace T4T.CQRS.Execution
{
    /// <summary>
    /// The result of executing a query or a command.
    /// Contains a list of <see cref="ExecutionError"/> and <see cref="ExecutionWarning"/>.
    /// The execution is considered successful when no errors occurred.
    /// </summary>
    public class ExecutionResult
    {
        public List<ExecutionError> Errors { get; set; }
        public List<ExecutionWarning> Warnings { get; set; }

        public bool Success => !Errors.Any();

        public ExecutionResult()
        {
            Errors =  new List<ExecutionError>();
            Warnings =  new List<ExecutionWarning>();
        }

        public static ExecutionResult Succeeded()
            => new ExecutionResult();

        public static ExecutionResult NotFoundAsWarning(string message = null)
        {
            var result = new ExecutionResult();
            result.Warnings.Add(ExecutionWarning.NotFound(message));

            return result;
        }

        public static ExecutionResult NotFoundAsError(string message = null)
        {
            var result = new ExecutionResult();
            result.Errors.Add(ExecutionError.NotFound(message));

            return result;
        }

        public static ExecutionResult BadRequest(string message = null)
        {
            var result = new ExecutionResult();
            result.Errors.Add(ExecutionError.BadRequest(message));

            return result;
        }

        /// <summary>
        /// Results in a <code>ExecutionErrorType.BadRequest</code> when the exception is an <see cref="InvalidOperationException"/> or an <see cref="ArgumentException"/>.
        /// In all other cases, an error of type <code>ExecutionErrorType.InternalServerError</code> is returned.
        /// </summary>
        /// <param name="e">The exception that caused the execution to fail.</param>
        /// <returns>An <see cref="ExecutionResult"/> with an <see cref="ExecutionError"/>.</returns>
        public static ExecutionResult FromException(Exception e)
        {
            var result = new ExecutionResult();

            switch (e)
            {
                case InvalidOperationException invalidOperationException:
                case ArgumentException argumentException:
                    result.Errors.Add(ExecutionError.BadRequest(e.Message));
                    break;
                default:
                    result.Errors.Add(ExecutionError.InternalServerError(e.Message));
                    break;
            }

            return result;
        }

        /// <summary>
        /// Cast this <see cref="ExecutionResult"/> instance to derived class <typeparamref name="TResult"/>.
        /// This is useful for returning QueryResults that inherit from <see cref="ExecutionResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the derived class.</typeparam>
        /// <returns>An instance of <typeparamref name="TResult"/>.</returns>
        public TResult As<TResult>()
            where TResult : ExecutionResult
        {
            var result = (TResult)Activator.CreateInstance(typeof(TResult));
            result.Errors = Errors;
            result.Warnings = Warnings;

            return result;
        }
    }
}
